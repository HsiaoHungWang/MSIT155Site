using Microsoft.AspNetCore.Mvc;
using MSIT155Site.Models;
using System.Text;

namespace MSIT155Site.Controllers
{
    public class ApiController : Controller
    {
        private readonly MyDBContext _context;
        public ApiController(MyDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return Content("Content 你好!!","text/plain",Encoding.UTF8);
        }

        public IActionResult Cities() {           
           var cities = _context.Addresses.Select(a => a.City).Distinct();
            return Json(cities);
        }

    }
}
