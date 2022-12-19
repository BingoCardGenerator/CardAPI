using CardApi.DBContext;
using CardApi.Interfaces;
using CardApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
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

            foreach (BingoCardModel card in bingocards)
            {
                card.Challenges = GetAllChallengesOfCard(card.Id);
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

            //card.Challenges = GetAllChallengesOfCard(cardid);

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
        /// Gets the all the challenges on the bingo card.
        /// </summary>
        /// <param name="cardid"> the id of the card.</param>

        private List<BingoCardChallengeModel> GetAllChallengesOfCard(Guid cardid)
        {
            List<BingoCardChallengeModel> challenges = new List<BingoCardChallengeModel>();
            if (CardIsGenerated(cardid))
            {
                challenges = _context.BingoCardChallenges.Where(b => b.BingoCardId == cardid).ToList();
            }

            return challenges;
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

        /// <summary>
        /// Checks if a bingo card is generated.
        /// </summary>
        /// <param name="cardid">The id of the card to check.</param>
        /// <returns>Wheter a card is generated or not.</returns>
        private bool CardIsGenerated(Guid cardid)
        {
            if(_context.BingoCardChallenges.Any(b => b.BingoCardId == cardid)) return true;
            return false;
        }

        /// <summary>
        /// Checks if enough challenges have been selected to fill the bingo card.
        /// </summary>
        /// <param name="required">How many challenges are needed to fill the card.</param>
        /// <param name="selected">How many challenges have been selected.</param>
        /// <returns>Wheter enough cards have been selected.</returns>
        private static bool EnoughSelectedChallenges(int required, int selected)
        {
            if(selected < required) return false;
            return true;
        }

        /// <summary>
        /// Checks if a card has a name.
        /// </summary>
        /// <param name="cardName">The name prop of the card.</param>
        /// <returns>Wheter a card has a name.</returns>
        private static bool CardNameEmpty(string cardName)
        {
            if(cardName == null) return true;
            if (cardName == "") return true;
            return false;
        }

        /// <summary>
        /// Checks if a card has both a number of columns and a number of rows.
        /// </summary>
        /// <param name="colums"> The column prop of the card.</param>
        /// <param name="rows">The row prop of the card.</param>
        /// <returns>Wheter the card has rows and columns.</returns>
        private static bool HasColumsAndRows(int colums, int rows)
        {
            if (colums > 0 && rows > 0 ) return true;
            return false;
        }

    }
}
