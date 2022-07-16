using System.ComponentModel.DataAnnotations;

namespace DOC_RASCH.Data.Entities
{
    public class Status
    {
        public int Id { get; set; }

        [Display(Name = "Estatus")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

    }
}
