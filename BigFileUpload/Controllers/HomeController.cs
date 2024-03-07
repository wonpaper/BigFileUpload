using BigFileUpload.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace BigFileUpload.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment environment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            this.environment = environment;
        }
       

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //[RequestSizeLimit(1024 * 1024 * 1024)]
        public async Task<IActionResult> Index(ICollection<IFormFile> file1)
        {
            var uploadFolder = String.Empty;
            string fileName1 = String.Empty;
            int fileSize1 = 0;

            uploadFolder = Path.Combine(environment.WebRootPath, "files/Notice");
            foreach (var file in file1)
            {
                if (file.Length > 0)
                {
                    fileSize1 = Convert.ToInt32(file.Length);
                    // 파일명 중복 처리
                    fileName1 = Dul.FileUtility.GetFileNameWithNumbering(
                        uploadFolder, Path.GetFileName(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"')));
                    // 파일 업로드
                    using (var fileStream = new FileStream(
                        Path.Combine(uploadFolder, fileName1), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }

            return View(file1);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
