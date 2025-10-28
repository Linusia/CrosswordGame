using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrosswordGame.Data;
using CrosswordGame.Models;
using System.Text.Json;

namespace CrosswordGame.Controllers
{
    public class HomeController : Controller
    {
        private readonly CrosswordContext _context;

        public HomeController(CrosswordContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Game(int levelId = 1)
        {
            var level = _context.Levels
                .Include(l => l.Words)
                .FirstOrDefault(l => l.LevelId == levelId);

            if (level == null)
            {
                return NotFound();
            }

            var gameState = new GameState
            {
                CurrentLevel = levelId,
                AvailableLetters = level.Letters.ToList(),
                FoundWords = new List<string>(),
                CurrentWord = ""
            };

            HttpContext.Session.SetObject("GameState", gameState);

            ViewBag.Level = level;
            return View();
        }

        [HttpPost]
        public IActionResult CheckWord(string word)
        {
            var gameState = HttpContext.Session.GetObject<GameState>("GameState");

            if (gameState == null)
            {
                return Json(new { success = false, message = "Sesja wygas³a" });
            }

            var level = _context.Levels
                .Include(l => l.Words)
                .FirstOrDefault(l => l.LevelId == gameState.CurrentLevel);

            if (level == null)
            {
                return Json(new { success = false, message = "Poziom nie istnieje" });
            }

            var validWord = level.Words.FirstOrDefault(w => w.Text.Equals(word, StringComparison.OrdinalIgnoreCase));

            if (validWord != null && !gameState.FoundWords.Contains(word.ToUpper()))
            {
                if (CanFormWord(word, gameState.AvailableLetters))
                {
                    gameState.FoundWords.Add(word.ToUpper());
                    RemoveLettersFromPool(word, gameState.AvailableLetters);
                    HttpContext.Session.SetObject("GameState", gameState);

                    return Json(new
                    {
                        success = true,
                        message = "Dobre s³owo!",
                        foundWords = gameState.FoundWords,
                        availableLetters = new string(gameState.AvailableLetters.ToArray())
                    });
                }
                else
                {
                    return Json(new { success = false, message = "Brak wystarczaj¹cych liter" });
                }
            }
            else if (gameState.FoundWords.Contains(word.ToUpper()))
            {
                return Json(new { success = false, message = "S³owo ju¿ u¿yte" });
            }
            else
            {
                return Json(new { success = false, message = "Nieprawid³owe s³owo" });
            }
        }

        [HttpPost]
        public IActionResult ResetLevel()
        {
            var gameState = HttpContext.Session.GetObject<GameState>("GameState");

            if (gameState != null)
            {
                var level = _context.Levels.FirstOrDefault(l => l.LevelId == gameState.CurrentLevel);
                if (level != null)
                {
                    gameState.AvailableLetters = level.Letters.ToList();
                    gameState.FoundWords.Clear();
                    gameState.CurrentWord = "";
                    HttpContext.Session.SetObject("GameState", gameState);
                }
            }

            return RedirectToAction("Game", new { levelId = gameState?.CurrentLevel ?? 1 });
        }

        public IActionResult SelectLevel(int levelId)
        {
            return RedirectToAction("Game", new { levelId });
        }

        private bool CanFormWord(string word, List<char> availableLetters)
        {
            var tempLetters = new List<char>(availableLetters);

            foreach (char c in word.ToUpper())
            {
                if (!tempLetters.Remove(c))
                {
                    return false;
                }
            }

            return true;
        }

        private void RemoveLettersFromPool(string word, List<char> availableLetters)
        {
            foreach (char c in word.ToUpper())
            {
                availableLetters.Remove(c);
            }
        }
    }

    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T? GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
        }
    }
}