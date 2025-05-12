using System.ComponentModel.DataAnnotations;

namespace GestionParcAuto.ViewModels
{
    public class CreateExpertiseViewModel
    {
        [Display(Name  = "Date")]
        public DateTime Date { get; set; }
        [Display(Name = "Véhicule")]
        [Required]
        public int VehicleId { get; set; }
        [Display(Name = "Employé")]
        public string? UserId { get; set; }
    }
}
