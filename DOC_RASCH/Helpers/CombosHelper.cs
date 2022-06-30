using Microsoft.AspNetCore.Mvc.Rendering;
using DOC_RASCH.Data;
using System.Collections.Generic;
using System.Linq;

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
            List<SelectListItem> list = _context.Business.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = $"{x.Id}"
            })
                .OrderBy(x => x.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una empresa...]",
                Value = "0"
            });

            return list;
        }
    }
}