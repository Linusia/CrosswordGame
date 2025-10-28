using Microsoft.EntityFrameworkCore;
using CrosswordGame.Models;

namespace CrosswordGame.Data
{
    public class CrosswordContext : DbContext
    {
        public CrosswordContext(DbContextOptions<CrosswordContext> options) : base(options)
        {
        }

        public DbSet<Level> Levels { get; set; }
        public DbSet<Word> Words { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Level>().HasData(
                new Level { LevelId = 1, Name = "Poziom 1 - Łatwy", Difficulty = 1, Letters = "AKOTMLPYS" },
                new Level { LevelId = 2, Name = "Poziom 2 - Średni", Difficulty = 2, Letters = "RZĘDŁUGIC" },
                new Level { LevelId = 3, Name = "Poziom 3 - Trudny", Difficulty = 3, Letters = "QWERTYUIO" }
            );

            modelBuilder.Entity<Word>().HasData(
                new Word { WordId = 1, LevelId = 1, Text = "KOT" },
                new Word { WordId = 2, LevelId = 1, Text = "LAS" },
                new Word { WordId = 3, LevelId = 1, Text = "DOM" },
                new Word { WordId = 4, LevelId = 1, Text = "MY" },
                new Word { WordId = 5, LevelId = 1, Text = "PYT" },
                new Word { WordId = 6, LevelId = 2, Text = "RZĘD" },
                new Word { WordId = 7, LevelId = 2, Text = "DŁUG" },
                new Word { WordId = 8, LevelId = 2, Text = "GIC" },
                new Word { WordId = 9, LevelId = 3, Text = "QWERTY" },
                new Word { WordId = 10, LevelId = 3, Text = "TYPE" },
                new Word { WordId = 11, LevelId = 3, Text = "WRITE" }
            );
        }
    }
}