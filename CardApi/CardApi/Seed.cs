using CardApi.Models;
using CardApi.DBContext;

namespace CardApi
{
    public class Seed
    {
        private readonly CardContext _context;
        public Seed(CardContext context)
        {
            _context = context;
        }

        public void SeedContext()
        {
            if (!_context.BingoCards.Any())
            {
                var bingoCards = new List<BingoCardModel>()
                {
                    new BingoCardModel
                    {
                        Name = "TestCard 1",
                        Columns = 5,
                        Rows = 5
                    },

                    new BingoCardModel
                    {
                        Name = "TestCard 2",
                        Columns = 6,
                        Rows = 6
                    }, 
                    
                    new BingoCardModel
                    {
                        Name = "TestCard 3",
                        Columns = 4,
                        Rows = 4
                    }

                };

                _context.BingoCards.AddRange(bingoCards);
                Console.WriteLine("Adding cards");
                _context.SaveChanges();
            }
        }
    }
}
