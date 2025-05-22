using System.ComponentModel.DataAnnotations;

namespace GestionParcAuto.ViewModels
{
    public class CreateExpertiseViewModel
    {
        [Display(Name  = "Date")]
        [Required(ErrorMessage = "Le champ {0} doit être rempli.")]
        public DateTime Date { get; set; }
        [Display(Name = "Véhicule")]
        [Required(ErrorMessage = "Le véhicule est obligatoire.")]
        public int VehicleId { get; set; }
        [Display(Name = "Employé")]
        public string? UserId { get; set; }
    }
}
