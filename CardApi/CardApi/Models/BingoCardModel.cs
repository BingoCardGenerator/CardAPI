using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardApi.Models
{
    /// <summary>
    /// The base class all bingo card models are based on.
    /// </summary>
    public class BingoCardForModificationModel
    {
        /// <summary>
        /// The name of the bingo card.
        /// </summary>
        [Required]
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// The number of columns on the bingo card.
        /// </summary>
        [Required]
        public int Columns { get; set; }

        /// <summary>
        /// The number of rows on the bingo card
        /// </summary>
        [Required]
        public int Rows { get; set; }

        /// <summary>
        /// The challenges on a bingo card
        /// </summary>
        public ICollection<BingoCardChallengeModel>? BingoCardChallenge { get; set; }
    }

    /// <summary>
    /// Model for a bingo card that has yet to be created.
    /// </summary>
    public class BingoCardForCreationModel : BingoCardForModificationModel
    {

    }

    /// <summary>
    /// Model for a bingo card.
    /// </summary>
    public class BingoCardModel : BingoCardForModificationModel
    {
        /// <summary>
        /// A unique identifier to keep bingocards appart.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set;} 
    }
}
