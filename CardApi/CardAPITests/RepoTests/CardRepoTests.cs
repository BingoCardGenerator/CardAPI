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
            PopulateDatabase();

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

        [Fact]
        public async Task GetCard_WithUnpoplatedTable_ShouldReturnUnsucessfull()
        {
            //Act
            var results = await _repository.GetAllBingoCards();

            ///Assert
            results.SuccesFull
                .Should()
                .BeFalse();
            results.ServiceResultCode
                .Should()
                .Be(ServiceResultCode.NotFound);
        }

        [Fact]
        public async Task CreateCard_ThatIsValid_ShouldReturnSuccesfull()
        {
            //Act
            var result = await _repository.CreateBingoCard(
                new BingoCardForCreationModel
                {
                    Name = "TestCard",
                    Rows = 2,
                    Columns = 2
                }
                );
            
            //Assert
            result.SuccesFull
                .Should()
                .BeTrue();
            result.ServiceResultCode
                .Should()
                .Be(ServiceResultCode.Ok);
        }

        [Fact]
        public async Task CreateCard_ThatIsNotValid_ShouldRetrunUnsuccesful()
        {
            //Act
            var result = await _repository.CreateBingoCard(
                new BingoCardForCreationModel
                {
                    Name = "",
                    Rows = 2,
                    Columns = 2
                }
                );

            //Assert
            result.SuccesFull
                .Should()
                .BeFalse();
            result.ServiceResultCode
                .Should()
                .Be(ServiceResultCode.BadRequest);

        }

        private async void PopulateDatabase()
        {
            await _context.BingoCards.AddRangeAsync(
                new BingoCardModel { Id = Guid.NewGuid(), Name = "Card 1" },
                new BingoCardModel { Id = Guid.NewGuid(), Name = "Card 2" },
                new BingoCardModel { Id = Guid.NewGuid(), Name = "Card 3" }
                );
            await _context.SaveChangesAsync();
        }
    }
}
