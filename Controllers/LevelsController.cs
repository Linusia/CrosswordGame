using Microsoft.AspNetCore.Mvc;

public class LevelsController : Controller
{
    public IActionResult Index(int page = 1)
    {
        return View("Levels", page);
    }
}
