using GestionParcAuto.Models;
using GestionParcAuto.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GestionParcAuto.Controllers
{
    public class UsersController : Controller
    {
        ApplicationDbContext _context;
        UserManager<User> _userManager;
        RoleManager<IdentityRole> _roleManager;

        public UsersController(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Data

        public async Task<IActionResult> GetUsers()
        {
            return new JsonResult(_userManager.Users.Select(x => new { x.Id, x.FullName,  x.Email }).ToList());
        }

        #endregion

        #region POST

        public async Task<IActionResult> RemoveUser(string id)
        {
            User? user = await _userManager.FindByIdAsync(id);

            if(user != null) 
                await _userManager.DeleteAsync(user);
            else
                throw new Exception("Error finding user with id " + id);

            return new JsonResult(new
            {
                Success = true,
                Message = "Utilisateur retiré avec succès."
            });
        }

        #endregion
    }
}
