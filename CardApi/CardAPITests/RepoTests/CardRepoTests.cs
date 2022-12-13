namespace CardAPITests.RepoTests
{
    public class CardRepoTests
    {
        private CardContext _context;
        private BingoCardRepository _repository;

        public CardRepoTests()
        {
            var options =
                new DbContextOptionsBuilder<CardContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new CardContext(options);
            _repository = new BingoCardRepository(_context);
        }

        [Fact]
        public async Task GetCards_WithPopulatedTable_ShouldReturnAllCards()
        {
            //Arrange
            await _context.BingoCards.AddRangeAsync(
                new BingoCardModel { Id = Guid.NewGuid(), Name = "Card 1" },
                new BingoCardModel { Id = Guid.NewGuid(), Name = "Card 2" },
                new BingoCardModel { Id = Guid.NewGuid(), Name = "Card 3" }
                );
            await _context.SaveChangesAsync();

            //Act
            var results = await _repository.GetAllBingoCards();
            var resultsdata = results.Data ?? new List<BingoCardModel>();

            //Assert
            resultsdata
                .Should()
                .NotBeEmpty()
                .And.HaveCount(3);

            results.SuccesFull
                .Should()
                .BeTrue();

            results.ServiceResultCode
                .Should()
                .Be(ServiceResultCode.Ok);
        }
    }
}
