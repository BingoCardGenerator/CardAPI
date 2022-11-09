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
                    //TODO: Probably add filling of bingo card here.
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
            if (newbingocard.Name == string.Empty) return "Please enter a name.";
            if (newbingocard.Columns <= 0) return $"{newbingocard.Columns} is not a valid amount of columns. Must be higher then 0.";
            if (newbingocard.Rows <= 0) return $"{newbingocard.Rows} is not a valid amount of Rows. Must be higher then 0.";

            return String.Empty;
        }

    }
}
