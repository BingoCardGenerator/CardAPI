using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardApi.Models
{
    /// <summary>
    /// A base model for the 'join table' between Bingo card and challenge.
    /// </summary>
    public class BingoCardChallengeForModificationModel
    {
        /// <summary>
        /// The id of the bingo card.
        /// </summary>
        [Required]
        public Guid BingoCardId { get; set; }

        /// <summary>
        /// The id of the Challenge
        /// </summary>
        [Required]
        public Guid ChallengeId { get; set; }

        /// <summary>
        /// The position of the challenge on the bingo card.
        /// </summary>
        [Required]
        public int Position { get; set; }

    }

    /// <summary>
    /// A model to use when you add a new challenge to a bingo card.
    /// </summary>
    public class BingoCardChallengeForCreationModel : BingoCardChallengeForModificationModel
    {
    }

    /// <summary>
    /// A model detailing a challenge on a bingo card.
    /// </summary>
    public class BingoCardChallengeModel : BingoCardChallengeForModificationModel
    {
    }
}
