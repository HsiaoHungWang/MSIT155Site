using Microsoft.AspNetCore.Mvc;

namespace MSIT155Site.Controllers
{
    public class HomeworkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
