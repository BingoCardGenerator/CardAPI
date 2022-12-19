using Microsoft.EntityFrameworkCore;
using CardApi.Models;

namespace CardApi.DBContext
{

    /// <summary>
    /// The context for the BingoCardDatabase.
    /// </summary>
    public class CardContext : DbContext
    {
        public CardContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// The table in wich bingo cards are stored.
        /// </summary>
        public DbSet<BingoCardModel> BingoCards { get; set; } = null!;
        public DbSet<BingoCardChallengeModel> BingoCardChallenges { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BingoCardModel>()
                .HasKey(b => b.Id);
            modelBuilder.Entity<BingoCardChallengeModel>()
                .HasKey(bc => new { bc.BingoCardId, bc.ChallengeId });
            modelBuilder.Entity<BingoCardChallengeModel>()
                .HasOne(bc => bc.BingoCard)
                .WithMany(b => b.Challenges);
        }
    }
}
