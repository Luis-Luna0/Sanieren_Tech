using DOC_RASCH.Data;
using DOC_RASCH.Data.Entities;
using DOC_RASCH.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DOC_RASCH.Helpers;

namespace DOC_RASCH.Controllers
{
    public class FileController : Controller
    {

        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;

        public FileController(DataContext context, ICombosHelper combosHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
        }

        // GET: FileController
        public async Task<IActionResult> Index()
        {
            return View(await _context.Files.Where(y => y.Active ==1)
                .Include(x => x.Business)
                .ToListAsync());
        }

        // GET: FileController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            FileViewModel model = new FileViewModel
            {
                Business = _combosHelper.GetComboBusiness()
            };

            return View(model);
        }

        // POST: FileController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FileViewModel model)
        {
            if (ModelState.IsValid)
            {                
                var business = _context.Business
                            .Single(b => b.Id == model.BusinessId);

                string operativeDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CarpetasOperativas", business.Name);

                string directory = Path.Combine(Directory.GetCurrentDirectory(), operativeDirectory, model.FileName);

                System.IO.Directory.CreateDirectory(directory);

                model.Url = directory;

                Data.Entities.File fileEntiti = new Data.Entities.File();

                fileEntiti.FileName = model.FileName;
                fileEntiti.Description = model.Description;
                fileEntiti.BusinessId = model.BusinessId;
                fileEntiti.Active = model.Active;
                fileEntiti.Url = model.Url;

                _context.Add(fileEntiti);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: FileController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FileController/Edit/5
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

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            Data.Entities.File file = await _context.Files
                .FirstOrDefaultAsync(x => x.Id == id);
            if (file == null)
            {
                return NotFound();
            }

            file.Active=0;
            _context.Update(file);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        
    }
}
