using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DOC_RASCH.Models
{
    public class SectionViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre de la Sección")]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "El {0} no debe tener carácteres especiales.")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FileName { get; set; }

        [Display(Name = "Descripción del contenido")]
        [MaxLength(250, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        [Display(Name = "Nombre del Cliente")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int FileId { get; set; }

        public IEnumerable<SelectListItem> Files { get; set; }

        public byte Active { get; set; }

        [Display(Name = "Url de la Carpeta")]
        [MaxLength(150, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        public string Url { get; set; }
    }
}
