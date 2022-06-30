using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DOC_RASCH.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboBusiness();


    }
}