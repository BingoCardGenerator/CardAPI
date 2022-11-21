using CardApi.Models;
using Microsoft.AspNetCore.Mvc;

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
        /// Gets a single card by it's id.
        /// </summary>
        /// <param name="cardid"> The id of the card to get.</param>
        /// <returns>The requested card.</returns>
        Task<ServiceResponse<BingoCardModel>> GetBingoCardById(Guid cardid);

        /// <summary>
        /// Validates and creates a new bingo card.
        /// </summary>
        /// <param name="newbingocard">The bingo card that needs to be created.</param>
        Task<ServiceResponse> CreateBingoCard(BingoCardForCreationModel newbingocard);

        /// <summary>
        /// Generates a bingo card based on a given list of selected challenges.
        /// </summary>
        /// <param name="cardid"> The id of the bingo card that has to be generated.</param>
        /// <param name="selectedchallenges"> A list with the id's of the selected challenges.</param>
        Task<ServiceResponse> GenerateBingoCard(Guid cardid, [FromBody] IEnumerable<Guid> selectedchallenges);

        /// <summary>
        /// Gets all the challenges ascociated with a bingo card.
        /// </summary>
        /// <param name="cardid">The id of the bingo card.</param>
        /// <returns>A list of challenges if the bingo card has any.</returns>
        Task<ServiceResponse<IEnumerable<BingoCardChallengeModel>>> GetAllChallengesOfCard(Guid cardid);
    }
}
