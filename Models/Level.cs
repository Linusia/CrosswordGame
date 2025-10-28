using System.ComponentModel.DataAnnotations;

namespace CrosswordGame.Models
{
    public class Level
    {
        [Key]
        public int LevelId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Difficulty { get; set; }

        [Required]
        public string Letters { get; set; } = string.Empty;

        public virtual ICollection<Word> Words { get; set; } = new List<Word>();
    }
}