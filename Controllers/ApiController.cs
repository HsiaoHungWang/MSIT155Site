using Microsoft.AspNetCore.Mvc;
using MSIT155Site.Models;
using MSIT155Site.Models.DTO;
using Newtonsoft.Json;
using System.Text;

namespace MSIT155Site.Controllers
{
    public class ApiController : Controller
    {
        private readonly MyDBContext _context;
        private readonly IWebHostEnvironment _environment;
        public ApiController(MyDBContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
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
        // public IActionResult Register(UserDTO _user)
        [HttpPost]
             public IActionResult Register(Member _user, IFormFile Avatar)
        {
            if(string.IsNullOrEmpty(_user.Name))
            {
                _user.Name = "guest";
            }
            //todo
            //1. 只允許上傳圖檔
            //2. 圖檔最大2M
            //3. 檔案名稱重複處理

            //string uploadPath = @"C:\Shared\AjaxWorkspace\MSIT155Site\wwwroot\uploads\a.jpg";
            string fileName = "empty.jpg";
            if(Avatar != null)
            {
                fileName = Avatar.FileName;
            }
            string uploadPath = Path.Combine(_environment.WebRootPath, "uploads", fileName);

            using (var fileStream = new FileStream(uploadPath, FileMode.Create))
            {
                Avatar?.CopyTo(fileStream);
            }

            // return Content($"Hello {_user.Name}, {_user.Age}歲了, 電子郵件是 {_user.Email}","text/plain", Encoding.UTF8);
            //return Content($"{_user.Avatar?.FileName} - {_user.Avatar?.ContentType} - {_user.Avatar?.Length}");

            //新增到資料庫
            _user.FileName = fileName;
            //轉成二進位
            byte[]? imgByte = null;
            using (var memoryStream = new MemoryStream())
            {
                Avatar?.CopyTo(memoryStream);
                imgByte = memoryStream.ToArray();
            }
            _user.FileData = imgByte;


            _context.Members.Add(_user);
            _context.SaveChanges();
            
            
            return Content(uploadPath);
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

        //景點資料
        [HttpPost]
        public IActionResult Spots([FromBody]SearchDTO _search)
        {
            return Json(_search);
        }

    }
}
