using DOC_RASCH.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System;
using DOC_RASCH.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace DOC_RASCH.Controllers
{
    public class UsersShowController : Controller
    {
        private readonly DataContext _context;


        public UsersShowController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string email = User.Identity.Name;
            var user = _context.Users.Where(y => y.Email == email).FirstOrDefault();
            int id = user.BusinessId;


            return View(await _context.Files
              .Where(x => x.BusinessId == id && x.Active==1).ToListAsync());
        }


        public async Task<IActionResult> IndexSection(int id)
        {
            return View(await _context.Sections
              .Where(x => x.FileId == id && x.Active == 1).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexDocument(int id, int idF)
        {
            TempData["FileId"]=idF;
            return View(await _context.Documents
              .Where(x => x.SectionId == id && x.Active == 1).ToListAsync());
        }
        public async Task ReturnSection(int id)
        {
            
            var sec = _context.Sections.Where(x => x.Id == id).FirstOrDefault();

            await IndexSection(sec.FileId);
        }

        public FileResult DownloadWord(int id)
        {

            var query = from x in _context.Documents
                     .Where(x => x.Id == id)
                        select x.ShortUrl;
            string shortUrl = "";
            foreach (var item in query)
            {
                shortUrl = item;
            }


            return File(virtualPath: shortUrl, contentType: "application/docx", fileDownloadName: shortUrl.Remove(0, 9));
        }

        public FileResult DownloadExcel(int id)
        {

            var query = from x in _context.Documents
                     .Where(x => x.Id == id)
                        select x.ShortUrl;
            string shortUrl = "";
            foreach (var item in query)
            {
                shortUrl = item;
            }


            return File(virtualPath: shortUrl, contentType: "application/docx", fileDownloadName: shortUrl.Remove(0, 9));
        }

        public async Task<IActionResult> Create([Bind("Id,FileName,Date_In,Date_Mod,Url,Word,Active,Format,SectionId")] Document document)
        {
            if (ModelState.IsValid)
            {
                ViewBag.message = "Documento subido correctamente";
                DateTime thisDay = DateTime.Now;
                document.Date_In = thisDay;
                document.Date_Mod = thisDay;
                document.Url = TempData["ruta"].ToString();
                document.Format = Convert.ToInt32(TempData["Formato"]);
                document.SectionId = (int)document.SectionId;
                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(document);
        }
      
        //public async IActionResult RegisDocto(IFormFile file, int SectionId)
        //{
        //    try
        //    {
        //        if (file != null)
        //        {
        //            string filename = file.FileName;
        //            filename = Path.GetFileName(filename);
        //            string uploadFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", filename);
        //            ViewBag.message = "El Documento se ha subido exitosamente.";
        //            TempData["ruta"] = uploadFilePath;
        //            var stream = new FileStream(uploadFilePath, FileMode.Create);
        //            file.CopyToAsync(stream);

        //            string ext = System.IO.Path.GetExtension(file.FileName);
        //            string format1 = ".pdf";
        //            string format2 = ".docx";
        //            string format3 = ".xlsx";

        //            bool op1 = string.Equals(ext, format1);

        //            bool op2 = string.Equals(ext, format2);

        //            bool op3 = string.Equals(ext, format3);

        //            if (ext.Equals(format1))
        //            {
        //                TempData["Formato"] = 1;

        //            }
        //            else if (ext.Equals(format2))
        //            {
        //                TempData["Formato"] = 2;
        //            }
        //            else if (ext.Equals(format3))
        //            {
        //                TempData["Formato"] = 3;
        //            }
        //            else
        //            {
        //                TempData["Formato"] = 0;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.message = "Error " + ex.Message.ToString();
        //    }

        //   await Create(SectionId);
        //}

        public IActionResult Create(int id)
        {
            TempData["SectionId"] = id;
            return View();
        }
    }
}
