using System.ComponentModel.DataAnnotations;

namespace GestionParcAuto.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        [Display(Name = "Image")]
        public byte[]? Image { get; set; }
        [Display(Name = "Marque")]
        [Required(ErrorMessage = "Le champ {0} est requis")]
        public string Make { get; set; }
        [Display(Name = "Modèle")]
        [Required(ErrorMessage = "Le champ {0} est requis")]
        public string Model { get; set; }
        [Display(Name = "Type")]
        public string? Type { get; set; }
        [Display(Name = "Date d'immatriculation")]
        public DateTime? RegistrationDate { get; set; }
        [Display(Name = "Kilométrage")]
        public int? Mileage { get; set; } //KM
        [Display(Name = "Immatriculation")]
        public string? Registration { get; set; }
        [Display(Name = "VIN")]
        public string? VIN { get; set; }
        [Display(Name = "Statut")]
        public char Status { get; set; } = 'N';

        public virtual ICollection<Expertise> Expertises { get; set; } = new List<Expertise>();
    }
}
