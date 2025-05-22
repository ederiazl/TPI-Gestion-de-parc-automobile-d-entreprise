using System.ComponentModel.DataAnnotations;
using GestionParcAuto.Models;
namespace GestionParcAuto.ViewModels
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Le champ {0} doit être rempli.")]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Le champ {0} doit être rempli.")]
        [Display(Name = "Prénom")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Le champ {0} doit être rempli.")]
        [Display(Name = "Nom")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Le champ {0} doit être rempli.")]
        [Display(Name = "Mot de passe temporaire")]
        [StringLength(100, ErrorMessage = "Le {0} doit faire au moins {2} et au maximum {1} caractères de long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string TempPassword { get; set; }
        public bool ChangePassword { get; set; }
    }
}
