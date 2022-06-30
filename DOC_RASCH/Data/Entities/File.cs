using System.ComponentModel.DataAnnotations;

namespace DOC_RASCH.Data.Entities
{
    public class File
    {
        public int Id { get; set; }


        [Display(Name = "Nombre de la Carpeta")]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "El {0} no debe tener carácteres especiales.")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FileName { get; set; }

        [Display(Name = "Descripción del contenido")]
        [MaxLength(250, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        [Display(Name = "Nombre de la empresa")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int BusinessId { get; set; }

        public Business Business { get; set; }

        public byte Active { get; set; }

        [Display(Name = "Url de la Carpeta")]
        [MaxLength(150, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        public string Url { get; set; }
    }
}
