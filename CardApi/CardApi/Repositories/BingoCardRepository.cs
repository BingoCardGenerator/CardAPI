using CardApi.DBContext;
using CardApi.Interfaces;
using CardApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CardApi.Repositories
{
    /// <summary>
    /// A repository that handels all the bingo card releated requests.
    /// </summary>
    public class BingoCardRepository : IBingoCardRepository
    {
        private readonly CardContext _context;

        public BingoCardRepository(CardContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<IEnumerable<BingoCardModel>>> GetAllBingoCards()
        {
            var bingocards = (await _context.BingoCards.ToListAsync());

            if(bingocards == null)
            {
                return new ServiceResponse<IEnumerable<BingoCardModel>>
                {
                    Data = bingocards,
                    SuccesFull = false,
                    ServiceResultCode = ServiceResultCode.NotFound,
                    Message = "404: No cards found."
                };
            }

            return new ServiceResponse<IEnumerable<BingoCardModel>>
            {
                Data = bingocards,
                SuccesFull = bingocards.Count > 0,
                ServiceResultCode = bingocards.Count > 0 ? ServiceResultCode.Ok : ServiceResultCode.NotFound,
                Message = bingocards.Count > 0 ? "200: bingocards found." : "404: No bingocards found."
            };
        }

        public async Task<ServiceResponse<BingoCardModel>> GetBingoCardById(Guid cardid)
        {
            var message = String.Empty;

            BingoCardModel? card = await _context.BingoCards.FirstOrDefaultAsync(c => c.Id == cardid);

            if(card == null || card.Name == String.Empty)
            {
                return new ServiceResponse<BingoCardModel>
                {
                    Data = card,
                    SuccesFull = false,
                    ServiceResultCode = ServiceResultCode.NotFound,
                    Message = "404: Card was not found."
                };
            }

            return new ServiceResponse<BingoCardModel>
            {
                Data = card,
                SuccesFull = true,
                ServiceResultCode = ServiceResultCode.Ok,
                Message = "200 : Card was succesfully found and returned."
            };
        }

        public async Task<ServiceResponse> CreateBingoCard(BingoCardForCreationModel newbingocard)
        {
            var message = ValidateNewBingoCard(newbingocard);

            if (string.IsNullOrEmpty(message))
            {
                BingoCardModel newcard = new BingoCardModel
                {
                    Name = newbingocard.Name,
                    Columns = newbingocard.Columns,
                    Rows = newbingocard.Rows
                };

                await _context.BingoCards.AddAsync(newcard);
                await _context.SaveChangesAsync();
            }

            return new ServiceResponse
            {
                SuccesFull = String.IsNullOrEmpty(message),
                ServiceResultCode = String.IsNullOrEmpty(message) ? ServiceResultCode.Ok : ServiceResultCode.BadRequest,
                Message = String.IsNullOrEmpty(message) ? "200: Card succesfully added." : $"400: {message}."
            };
        }

        public async Task<ServiceResponse> GenerateBingoCard(Guid cardid, IEnumerable<Guid> selectedchallenges)
        {
            BingoCardModel? card = GetBingoCardById(cardid).Result.Data;
            List<Guid> selectedChallenges = selectedchallenges.ToList();

            if(card == null)
            {
                return new ServiceResponse
                {
                    SuccesFull = false,
                    ServiceResultCode = ServiceResultCode.NotFound,
                    Message = "404: Card was not found."
                };
            }


            if (CardIsGenerated(card.Id))
            {
                return new ServiceResponse
                {
                    SuccesFull = false,
                    ServiceResultCode = ServiceResultCode.UnprocessableEntity,
                    Message = "422: Card already has challenges assiged."
                };
            }

            int numberOfChallenges = card.Rows * card.Columns;
            if(!EnoughSelectedChallenges(numberOfChallenges, selectedChallenges.Count))
            {
                return new ServiceResponse
                {
                    SuccesFull = false,
                    ServiceResultCode = ServiceResultCode.UnprocessableEntity,
                    Message = "422: Not enough challenges in the selected list."
                };
            }

            for (int n = 1; n <= numberOfChallenges; n++)
            {
                Random r = new Random();
                int index = r.Next(0, selectedChallenges.Count);
                var selectedchallengeid = selectedChallenges[index];
                selectedChallenges.RemoveAt(index);

                await _context.BingoCardChallenges.AddAsync(new BingoCardChallengeModel
                {
                    BingoCardId = card.Id,
                    ChallengeId = selectedchallengeid,
                    Position =  n
                });

                await _context.SaveChangesAsync();
            }

            return new ServiceResponse
            {
                SuccesFull = true,
                ServiceResultCode = ServiceResultCode.Ok,
                Message = $"200 : succesfully generated bingocard with name: {card.Name} and id: {card.Id}."
            };
        }

        /// <summary>
        /// Validates of all values for a new bingo card are entered and within restricitons.
        /// </summary>
        /// <param name="newbingocard"></param>
        /// <returns>A message if anything is wrong.</returns>
        private static string ValidateNewBingoCard(BingoCardForCreationModel newbingocard)
        {
            if (CardNameEmpty(newbingocard.Name)) return "Please enter a name.";
            if (!HasColumsAndRows(newbingocard.Columns, newbingocard.Rows)) return "Columns and Rows may not be 0.";

            return String.Empty;
        }

        private bool CardIsGenerated(Guid cardid)
        {
            if(_context.BingoCardChallenges.Any(b => b.BingoCardId == cardid)) return true;
            return false;
        }

        private static bool EnoughSelectedChallenges(int required, int selected)
        {
            if(selected < required) return false;
            return true;
        }

        private static bool CardNameEmpty(string cardName)
        {
            if(cardName == null) return true;
            return false;
        }

        private static bool HasColumsAndRows(int colums, int rows)
        {
            if (colums > 0 && rows > 0 ) return true;
            return false;
        }

    }
}
