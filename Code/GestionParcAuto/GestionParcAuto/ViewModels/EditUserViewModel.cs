using System.ComponentModel.DataAnnotations;

namespace GestionParcAuto.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }
        [Display(Name = "Prénom")]
        public string? Name { get; set; }
        [Display(Name = "Nom")]
        public string? Surname { get; set; }
        public bool ChangePassword { get; set; }
    }
}
