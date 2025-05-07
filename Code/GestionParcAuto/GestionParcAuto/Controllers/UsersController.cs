using GestionParcAuto.Models;
using GestionParcAuto.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GestionParcAuto.ViewModels;
using GestionParcAuto.Classes;
using System.Text;
using System.Xml.Linq;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

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

        public IActionResult Index(Message message)
        {
            if(message.Text != null)
            {
                ViewBag.Message = Json(message);
            }

            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        #region Data

        public async Task<IActionResult> GetUsers()
        {
            List<User> Users = _userManager.Users.ToList();
            return new JsonResult(Users.Select(x => new { x.Id, x.FullName,  x.Email, roles = _userManager.GetRolesAsync(x).Result }).ToList());
        }

        #endregion

        #region POST

        public async Task<ActionResult> CreateRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });

            await _roleManager.CreateAsync(new IdentityRole { Name = "Employee" });

            return new JsonResult(new
            {
                Success = true,
                Message = "Utilisateur retiré avec succès."
            });
        }

        public async Task<IActionResult> RemoveUser(string id)
        {
            if (id == _userManager.GetUserId(this.User))
                throw new Exception("Impossible de supprimer l'utilisateur actuel.");

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

        [HttpPost]
        public IActionResult Create(CreateUserViewModel vm)
        {
            User user = new User { Name = vm.Name, Surname = vm.Surname, Email = vm.Email, UserName = vm.Email };
            //CreateUserViewModel user = new();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var res = Task.Run(async () => await _userManager.CreateAsync(user, vm.TempPassword)).Result;

            if (!res.Succeeded)
            {
                return View(vm);
            }

            ViewBag.Message = new Message { Title = "Création d'un utilisateur", Text = "L'utilisateur à été créée avec succès." };

            return View("Index");
        }

        #endregion
    }
}
