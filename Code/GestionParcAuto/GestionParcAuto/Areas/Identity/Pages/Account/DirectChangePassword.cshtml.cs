using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionParcAuto.Models;

namespace GestionParcAuto.Areas.Identity.Pages.Account
{
    public class DirectChangePasswordModel : PageModel
    {
        private UserManager<User> _userManager;
        public DirectChangePasswordModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Ancien mot de passe")]
            [DataType(DataType.Password)]
            public string OldPassword { get; set; }
            [Required]
            [StringLength(100, ErrorMessage = "Le {0} doit faire entre {2} et {1} caractères de long.", MinimumLength = 6)]
            [Display(Name = "Nouveau mot de passe")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmer le mot de passe")]
            [Compare("Password", ErrorMessage = "Le mot de passe et la confirmation ne correspondent pas.")]
            public string ConfirmPassword { get; set; }

        }
        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            string returnUrl = Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            User user = await _userManager.GetUserAsync(User);

            if(user == null)
                return Page();

            var res = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.Password);

            if (res.Succeeded)
            {
                user.ChangePassword = false;
                await _userManager.UpdateAsync(user);
                return LocalRedirect(returnUrl);
            }

            return Page();
        }
    }
}
