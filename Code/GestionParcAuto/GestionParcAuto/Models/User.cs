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
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
