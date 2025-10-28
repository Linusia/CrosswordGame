using System.ComponentModel.DataAnnotations;

namespace CrosswordGame.Models
{
    public class Word
    {
        [Key]
        public int WordId { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;

        [Required]
        public int LevelId { get; set; }

        public virtual Level Level { get; set; } = null!;
    }
}