using System;
using System.ComponentModel.DataAnnotations;

namespace DOC_RASCH.Data.Entities
{
    public class Document
    {
        public int Id { get; set; }

        [Display(Name = "Nombre del Documento")]
        [MaxLength(50, ErrorMessage = "El nombre del Archivo no puede tener más de 50 carácteres.")]
        [Required(ErrorMessage = "El Nombre del Documento es Oblgatorio.")]
        public string FileName { get; set; }

        [Display(Name = "Fecha de Registro")]
        public DateTime Date_In { get; set; }

        [Display(Name = "Última Modificación")]
        public DateTime Date_Mod { get; set; }

        [Display(Name = "Url del documento")]
        [Required(ErrorMessage = "El Url del Documento es Oblgatorio.")]
        public string Url { get; set; }

        [Display(Name = "Url corta del documento")]
        public string? ShortUrl { get; set; }

        [Display(Name = "Estatus")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Status Status { get; set; }
        public int StatusId { get; set; }

        [Required(ErrorMessage = "El estatus es Obligatorio.")]
        public byte Active { get; set; }

        [Display(Name = "Formato del Documento")]
        [Required(ErrorMessage = "El Formato es Obligatorio.")]
        public int Format { get; set; }

        [Display(Name = "Seccion a la que pertenece")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int SectionId { get; set; }
        public Section Section { get; set; }
    }
}
