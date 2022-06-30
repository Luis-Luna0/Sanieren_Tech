
using System.ComponentModel.DataAnnotations;

namespace DOC_RASCH.Data.Entities
{
    public class Business
    {
        public int Id { get; set; }

        [Display(Name = "Nombre del Cliente")]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "El {0} no debe tener carácteres especiales.")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        public int Active { get; set; }
    }
}