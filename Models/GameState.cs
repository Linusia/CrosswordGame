namespace CrosswordGame.Models
{
    public class GameState
    {
        public int CurrentLevel { get; set; }
        public List<char> AvailableLetters { get; set; }
        public List<string> FoundWords { get; set; }
        public string CurrentWord { get; set; }

        public GameState()
        {
            AvailableLetters = new List<char>();
            FoundWords = new List<string>();
            CurrentWord = "";
        }
    }
}