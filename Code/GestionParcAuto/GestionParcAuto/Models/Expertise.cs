using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GestionParcAuto.Models
{
    public class Expertise
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [Display(Name = "Statut")]
        public bool Status { get; set; }
        public string? Note { get; set; }
        [Display(Name = "Véhicule")]
        public Vehicle Vehicle { get; set; }
        [AllowNull]
        [Display(Name = "Employé")]
        public User? User { get; set; }
    }
}
