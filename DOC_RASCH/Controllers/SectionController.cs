using DOC_RASCH.Data;
using DOC_RASCH.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

namespace DOC_RASCH.Controllers
{
    public class SectionController : Controller
    {

        private readonly DataContext _context;

        public SectionController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Sections.Where(y => y.Active==1)
                .Include(x => x.File)
                .ToListAsync());
        }

        // GET: SectionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public List<Data.Entities.File> GetFiles()
        {
            var consulta = from datos in _context.Files.Where(x => x.Active == 1) select datos;
            return consulta.ToList();
        }

        // GET: SectionController/Create
        public ActionResult Create()
        {
            List<Data.Entities.File> listaFiles = GetFiles();
            ViewBag.Carpetas = listaFiles;
            return View();
        }

        // POST: SectionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FileName,Description,FileId,Active,Url")] Section section)
        {
            if (ModelState.IsValid)
            {
                var fileName = _context.Files
                            .Single(b => b.Id == section.FileId);

                var businessName = _context.Business
                    .Single(b => b.Id == fileName.BusinessId);

                string CaOp = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CarpetasOperativas", businessName.Name);

                string route = Path.Combine(Directory.GetCurrentDirectory(), CaOp, fileName.FileName);

                string directory = Path.Combine(Directory.GetCurrentDirectory(), route, section.FileName);

                System.IO.Directory.CreateDirectory(directory);

                section.Url = directory;

                _context.Add(section);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: SectionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SectionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SectionController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Section section = await _context.Sections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (section == null)
            {
                return NotFound();
            }

            section.Active = 0;
            _context.Update(section);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
