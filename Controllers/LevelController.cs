using Microsoft.AspNetCore.Mvc;

public class LevelController : Controller
{
    public IActionResult Play(int id)
    {
        return Content($"Tu będzie poziom nr {id}");
    }
}
