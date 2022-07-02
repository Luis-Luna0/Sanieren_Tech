using DOC_RASCH.Data;
using DOC_RASCH.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using DOC_RASCH.Helpers;
using DOC_RASCH.Models;

namespace DOC_RASCH.Controllers
{
    public class SectionController : Controller
    {

        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;

        public SectionController(DataContext context, ICombosHelper combosHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
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

        // GET: SectionController/Create
        public ActionResult Create()
        {
            SectionViewModel model = new SectionViewModel
            {
                Files = _combosHelper.GetComboFile()
            };
            return View(model);
        }

        // POST: SectionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var fileName = _context.Files
                            .Single(b => b.Id == model.FileId);

                var businessName = _context.Business
                    .Single(b => b.Id == fileName.BusinessId);

                string CaOp = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CarpetasOperativas", businessName.Name);

                string route = Path.Combine(Directory.GetCurrentDirectory(), CaOp, fileName.FileName);

                string directory = Path.Combine(Directory.GetCurrentDirectory(), route, model.FileName);

                System.IO.Directory.CreateDirectory(directory);

                model.Url = directory;

                Section sectionEtinti = new Section();

                sectionEtinti.FileName = model.FileName;
                sectionEtinti.Description = model.Description;
                sectionEtinti.FileId = model.FileId;
                sectionEtinti.Active = model.Active;
                sectionEtinti.Url = model.Url;

                _context.Add(sectionEtinti);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
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
