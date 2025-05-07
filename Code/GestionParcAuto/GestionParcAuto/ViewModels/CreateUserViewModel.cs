using System.ComponentModel.DataAnnotations;
using GestionParcAuto.Models;
namespace GestionParcAuto.ViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [Display(Name = "Mot de passe temporaire")]
        [StringLength(100, ErrorMessage = "Le {0} doit faire au moins {2} et au maximum {1} caractères de long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string TempPassword { get; set; }
    }
}
