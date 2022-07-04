using Microsoft.AspNetCore.Mvc.Rendering;
using DOC_RASCH.Data;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore;

namespace DOC_RASCH.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboBusiness()
        {
            List<SelectListItem> list = _context.Business.Where(x => x.Active == 1).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = $"{x.Id}"
            })
                .OrderBy(x => x.Text)
                .ToList(); ;

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una empresa...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboFile()
        {
            List<SelectListItem> list = _context.Files
                .Where(x => x.Active == 1)
                .Include(x => x.Business)
                .Select(x => new SelectListItem
            {
                Text = x.FileName + " || Cliente: "+x.Business.Name,
                Value = $"{x.Id}"
            })
                .OrderBy(x => x.Text)
                .ToList(); ;

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una Carpeta...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboSection()
        {
            List<SelectListItem> list = _context.Sections
                .Where(x => x.Active == 1)
                .Include(x => x.File)
                .Include(x => x.File.Business)
                .Select(x => new SelectListItem
                {
                    Text = x.FileName + " || Carpeta: " + x.File.FileName + " || Cliente: "+x.File.Business.Name,
                    Value = $"{x.Id}"
                })
                .OrderBy(x => x.Text)
                .ToList(); ;

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una Carpeta...]",
                Value = "0"
            });

            return list;
        }
    }
}