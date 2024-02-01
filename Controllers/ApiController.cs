using Microsoft.AspNetCore.Mvc;
using MSIT155Site.Models;
using MSIT155Site.Models.DTO;
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
            Thread.Sleep(3000);

            //int x = 10;
            //int y = 0;
            //int z = x / y;
            return Content("Content 你好!!","text/plain",Encoding.UTF8);
        }

        //public IActionResult Register(string name, int age = 28)
        public IActionResult Register(UserDTO _user)
        {
            if(string.IsNullOrEmpty(_user.Name))
            {
                _user.Name = "guest";
            }
            // return Content($"Hello {_user.Name}, {_user.Age}歲了, 電子郵件是 {_user.Email}","text/plain", Encoding.UTF8);
            return Content($"{_user.Avatar?.FileName} - {_user.Avatar?.ContentType} - {_user.Avatar?.Length}");
        }

        //讀取城市
        public IActionResult Cities() {           
           var cities = _context.Addresses.Select(a => a.City).Distinct();
            return Json(cities);
        }

        //根據城市名稱讀取鄉鎮區
        public IActionResult District(string city)
        {
           
            var districts = _context.Addresses.Where(a=>a.City == city).Select(a=>a.SiteId).Distinct();
            return Json(districts);
        }

        //根據鄉鎮區名稱讀取路名

        public IActionResult Avatar(int id=1)
        {
            Member? member = _context.Members.Find(id);
            if(member != null)
            {
                byte[] img = member.FileData;
                if(img != null)
                {
                    return File(img, "image/jpeg");
                }
            }

            return NotFound();
        }

    }
}
