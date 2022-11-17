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
                    Message = "404: No challenges found."
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
                Message = String.IsNullOrEmpty(message) ? "200: Challenge succesfully added." : $"400: {message}."
            };
        }

        /// <summary>
        /// Validates of all values for a new bingo card are entered and within restricitons
        /// </summary>
        /// <param name="newbingocard"></param>
        /// <returns>A message if anything is wrong</returns>
        private static string ValidateNewBingoCard(BingoCardForCreationModel newbingocard)
        {
            if (!CardNameEmpty(newbingocard.Name)) return "Please enter a name.";
            if (!HasColumsAndRows(newbingocard.Columns, newbingocard.Rows)) return "Columns and Rows may not be 0.";

            return String.Empty;
        }

        private static bool CardNameEmpty(string cardName)
        {
            if(cardName == null) return false;
            return true;
        }

        private static bool HasColumsAndRows(int colums, int rows)
        {
            if (colums > 0 && rows > 0 ) return true;
            return false;
        }

    }
}
