using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GestionParcAuto.Models
{
    public class Expertise
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Le champ {0} doit être rempli.")]
        public DateTime Date { get; set; }
        [Display(Name = "Statut")]
        public bool Status { get; set; }
        [Display(Name = "Remarques")]
        public string? Note { get; set; }
        [Display(Name = "Véhicule")]
        [Required(ErrorMessage = "Le véhicule est obligatoire.")]
        public Vehicle Vehicle { get; set; }
        [AllowNull]
        [Display(Name = "Employé")]
        public User? User { get; set; }
    }
}
