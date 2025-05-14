using System.ComponentModel.DataAnnotations;

namespace GestionParcAuto.ViewModels
{
    public class EditExpertiseViewModel
    {
        public int Id { get; set; }
        [Display(Name  = "Date")]
        public DateTime Date { get; set; }
        [Display(Name = "Véhicule")]
        [Required]
        public int VehicleId { get; set; }
        [Display(Name = "Employé")]
        public string? UserId { get; set; }
        [Display(Name = "Statut")]
        public bool Status { get; set; }
        [Display(Name = "Remarques")]
        public string? Note { get; set; }
    }
}
