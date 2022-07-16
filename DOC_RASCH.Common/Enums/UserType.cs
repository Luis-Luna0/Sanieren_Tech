using System.ComponentModel.DataAnnotations;

namespace DOC_RASCH.Common.Enums
{
    public enum UserType
    {
        [Display(Name = "Administrador")]
        Admin =1,
        [Display(Name = "Cliente")]
        User =2,
    }
}
