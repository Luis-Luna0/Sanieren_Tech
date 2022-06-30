using DOC_RASCH.Data;
using DOC_RASCH.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DOC_RASCH.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BusinessController : Controller
    {
        private readonly DataContext _context;

        public BusinessController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Business.Where(x => x.Active ==1).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Business business)
        {
            try
            {
                var businessName = business.Name;

                for (int i = 0; i <= businessName.Length; i++)
                {
                    if (i == 0)
                    {
                        if ((businessName.Substring(i, 1)).All(char.IsDigit))
                        {
                            ViewBag.BusinessName = "Primera letra es Número";
                            return View(business);
                        }
                    }

                }


                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                businessName = myTI.ToTitleCase(business.Name);
                businessName = Regex.Replace(businessName, @"\s", "");
                business.Name = businessName;



                if ((business.Name).All(char.IsDigit))
                {
                    ViewBag.BusinessName = "El Nombre del cliente no puede ser solo númerico.";
                    return View(business);
                }
                else
                {
                    _context.Add(business);

                    var NombreCarpetaOp = business.Name;

                    string pathString = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CarpetasOperativas", NombreCarpetaOp);

                    System.IO.Directory.CreateDirectory(pathString);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    ModelState.AddModelError(string.Empty, "Ya existe una empresa con este nombre.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return View(business);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Business business = await _context.Business.FindAsync(id);
            if (business == null)
            {
                return NotFound();
            }

            return View(business);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Business business)
        {
            if (id != business.Id)
            {
                return NotFound();
            }


                try
                {
                    _context.Update(business);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este tipo de documento.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                
            }
            return View(business);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Business business = await _context.Business
                .FirstOrDefaultAsync(m => m.Id == id);
            if (business == null)
            {
                return NotFound();
            }

            business.Active = 0;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}