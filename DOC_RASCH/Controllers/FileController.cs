using DOC_RASCH.Data;
using DOC_RASCH.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DOC_RASCH.Controllers
{
    public class FileController : Controller
    {

        private readonly DataContext _context;

        public FileController(DataContext context)
        {
            _context = context;
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

        public List<Business> GetBusinesses()
        {
            var consulta = from datos in _context.Business select datos;
            return consulta.ToList();
        }

        public ActionResult Create()
        {
            List<Business> listadept = GetBusinesses();
            ViewBag.Departamentos = listadept;
            return View();
        }

        // POST: FileController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FileName,Description,BusinessId,Active,Url")] Data.Entities.File file)
        {
            if (ModelState.IsValid)
            {
                var nameDirectory = from datos in _context.Business where datos.Id == file.Business.Id select datos.Name;
                var business = _context.Business
                            .Single(b => b.Id == file.BusinessId);

                string operativeDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CarpetasOperativas", business.Name);

                string directory = Path.Combine(Directory.GetCurrentDirectory(), operativeDirectory, file.FileName);

                System.IO.Directory.CreateDirectory(directory);

                file.Url = directory;

                _context.Add(file);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
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

            /*borrado del usuario*/
            file.Active=0;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        
    }
}
