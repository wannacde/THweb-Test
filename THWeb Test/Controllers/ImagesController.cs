using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace THWeb_Test.Controllers
{
    public class ImagesController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public ImagesController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult GetImage(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return NotFound();
            }

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "uploads", filename);
            
            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            var imageBytes = System.IO.File.ReadAllBytes(imagePath);
            return File(imageBytes, "image/jpeg");
        }
    }
}