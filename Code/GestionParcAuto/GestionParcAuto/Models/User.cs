using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GestionParcAuto.Models
{
    public class User : IdentityUser
    {
        public string FullName 
        { 
            get => $"{Name} {Surname}"; 
        }
        [Display(Name = "Prénom")]
        public string Name { get; set; }
        [Display(Name = "Nom")]
        public string Surname { get; set; }
    }
}
