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
              .Where(x => x.BusinessId == id && x.Active == 1).ToListAsync());
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
            TempData["FileId"] = idF;
            return View(await _context.Documents
              .Where(x => x.SectionId == id && x.Active == 1).ToListAsync());
        }
        public async Task ReturnSection(int id)
        {

            var sec = _context.Sections.Where(x => x.Id == id).FirstOrDefault();

            await IndexSection(sec.FileId);
        }

    }
}
