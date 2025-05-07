using System.ComponentModel.DataAnnotations;

namespace GestionParcAuto.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public bool CheckAdmin { get; set; }
        public bool CheckEmployee { get; set; }
    }
}
