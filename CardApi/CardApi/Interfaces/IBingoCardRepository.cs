using CardApi.Models;

namespace CardApi.Interfaces
{
    /// <summary>
    /// Defines a contract that represents the service that handels all bingo card releated requests.
    /// </summary>
    public interface IBingoCardRepository
    {
        /// <summary>
        /// Gets all bingo cards from the database.
        /// </summary>
        /// <returns> a collection of all the bingo cards.</returns>
        Task<ServiceResponse<IEnumerable<BingoCardModel>>> GetAllBingoCards();

        /// <summary>
        /// Validates and creates a new bingo card
        /// </summary>
        /// <param name="newbingocard">The bingo card that needs to be created</param>
        Task<ServiceResponse> CreateBingoCard(BingoCardForCreationModel newbingocard);
    }
}
