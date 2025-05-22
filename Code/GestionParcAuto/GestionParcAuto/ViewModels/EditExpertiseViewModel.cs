using System.ComponentModel.DataAnnotations;

namespace GestionParcAuto.ViewModels
{
    public class EditExpertiseViewModel
    {
        public int Id { get; set; }
        [Display(Name  = "Date")]
        [Required(ErrorMessage = "Le champ {0} doit être rempli.")]
        public DateTime Date { get; set; }
        [Display(Name = "Véhicule")]
        [Required(ErrorMessage = "Le véhicule est obligatoire.")]
        public int VehicleId { get; set; }
        [Display(Name = "Employé")]
        public string? UserId { get; set; }
        [Display(Name = "Statut")]
        public bool Status { get; set; }
        [Display(Name = "Remarques")]
        public string? Note { get; set; }
    }
}
