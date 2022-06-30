using DOC_RASCH.Data;
using DOC_RASCH.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DOC_RASCH.Controllers
{
    public class DocumentController : Controller
    {
        private readonly DataContext _context;

        public DocumentController(DataContext context)
        {
            _context = context;
        }

        // GET: DocumentController
        public async Task<IActionResult> Index()
        {
            return _context.Documents != null ?
                View(await _context.Documents.ToListAsync()) :
                          Problem("Entity set 'DataContext.Documents'  is null.");
        }

        // GET: DocumentController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }
        public IActionResult Create()
        {
            List<Section> listaSections = GetSections();
            ViewBag.Secciones = listaSections;
            return View();
        }

        public List<Section> GetSections()
        {
            var consulta = from datos in _context.Sections select datos;
            return consulta.ToList();
        }

        public IActionResult RegisDocto()
        {
            return View();
        }

        public FileResult DownloadWord(int id)
        {

            var query = from x in _context.Documents
                     .Where(x=> x.Id ==id) select x.ShortUrl;
            string shortUrl="";
            foreach (var item in query)
            {
                shortUrl= item;
            }
            

            return File(virtualPath: shortUrl , contentType: "application/docx", fileDownloadName: shortUrl.Remove(0,9));
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

        // GET: DocumentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        [HttpPost]
        public IActionResult RegisDocto(IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    string filename = file.FileName;
                    filename = Path.GetFileName(filename);

                    //string CaOp = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CarpetasOperativas", Business.Name);

                    //string CaFi = Path.Combine(Directory.GetCurrentDirectory(), CaOp, File.FileName);

                    //string CaSe = Path.Combine(Directory.GetCurrentDirectory(), CaFi, Section.FileName);

                    //string uploadFilePath = Path.Combine(Directory.GetCurrentDirectory(), CaSe, filename);

                    string uploadFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", filename);
                    ViewBag.message = "El Documento se ha subido exitosamente.";
                    TempData["ruta"] = uploadFilePath;
                    var stream = new FileStream(uploadFilePath, FileMode.Create);
                    file.CopyToAsync(stream);

                    string ext = System.IO.Path.GetExtension(file.FileName);
                    string format1 = ".pdf";
                    string format2 = ".docx";
                    string format3 = ".xlsx";

                    bool op1 = string.Equals(ext, format1);

                    bool op2 = string.Equals(ext, format2);

                    bool op3 = string.Equals(ext, format3);

                    if (ext.Equals(format1))
                    {
                        TempData["Formato"] = 1;

                    } else if (ext.Equals(format2))
                    {
                        TempData["Formato"] = 2;
                    }
                    else if (ext.Equals(format3))
                    {
                        TempData["Formato"] = 3;
                    }
                    else
                    {
                        TempData["Formato"] = 0;
                    }                    
                }
            }
            catch (Exception ex)
            {
                ViewBag.message = "Error " + ex.Message.ToString();
            }
            return RedirectToAction("Create", "Document");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            return View(document);
        }

        // POST: DocumentsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FileName,Date_In,Date_Mod,Url,Word,Active,Format")] Document document)
        {
            if (id != document.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(document);
        }

        // GET: DocumentsController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Documents == null)
            {
                return Problem("Entity set 'DataContext.Documents'  is null.");
            }
            var document = await _context.Documents.FindAsync(id);
            if (document != null)
            {
                _context.Documents.Remove(document);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
            return (_context.Documents?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
