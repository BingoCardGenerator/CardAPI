using CardApi.DBContext;
using CardApi.Interfaces;
using CardApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CardApi.Controllers
{
    /// <summary>
    /// Controller route for bingo cards.
    /// </summary>
    [ApiController]
    [Route("Api/BingoCard")]
    public class BingoCardController : BingoCardApiController
    {
        /// <summary>
        /// the service object to handle bingo cards.
        /// </summary>
        private readonly IBingoCardRepository _bingoCardRepository;

        public BingoCardController(IBingoCardRepository repo)
        {
            _bingoCardRepository = repo;
        }

        /// <summary>
        /// Api enpoint that gets all the bingo cards from the database.
        /// </summary>
        /// <returns>
        /// A list of all the bingo cards in the database.
        /// 200: If the search was completed succesfully.
        /// 404: if the no bingo cards were found. 
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<BingoCardModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("/bingocards")]
        public async Task<IActionResult> GetAllBingoCards()
        {
            return GetActionResult(await _bingoCardRepository.GetAllBingoCards());
        }


        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BingoCardModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("/bingocards/{id}")]
        public async Task<IActionResult> GetById(Guid cardid)
        {
            return GetActionResult(await _bingoCardRepository.GetBingoCardById(cardid));
        }

        /// <summary>
        /// API endpoint to create and add a new bingo cards to the database.
        /// </summary>
        /// <param name="forCreationModel">The bingo card that has to be created</param>
        /// <returns>
        /// 200: If the bingo card was created succesfully.
        /// 400: If the bingo card could not be created. 
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("/bingocard")]
        public async Task<IActionResult> CreateNewBlankBingoCard(BingoCardForCreationModel forCreationModel)
        {
            var result = await _bingoCardRepository.CreateBingoCard(forCreationModel);

            return GetActionResult(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost("/generate/bingocardchallenge")]
        public async Task<IActionResult> GenerateBingoCard(Guid cardid, IEnumerable<Guid> selectedchallenges)
        {
            return GetActionResult(await _bingoCardRepository.GenerateBingoCard(cardid, selectedchallenges));
        }
    }
}
