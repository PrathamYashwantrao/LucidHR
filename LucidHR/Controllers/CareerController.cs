using LucidHR.Controllers;
using LucidHR.Data;
using LucidHR.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Crypto.Tls;


namespace LucidHR.Controllers
{
    public class CareerController : Controller
    {
         ApplicationDbContext db;

        public CareerController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddEmp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmp(Career c, IFormFile FileData)
        {
            if (FileData != null && FileData.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await FileData.CopyToAsync(memoryStream);
                    //c.PdfFileName = pdfFile.FileName;
                    c.FileData = memoryStream.ToArray();
                    c.ContentType = FileData.ContentType;
                }
            }
            else
            {
                // Handle the case where no PDF is uploaded
                c.FileData = null;
                c.ContentType = null;
              
            }

            db.Careers.Add(c);
            await db.SaveChangesAsync();

            return RedirectToAction("Index", "Crm");
        }
    }
}
