using GestionParcAuto.Models;
using GestionParcAuto.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GestionParcAuto.ViewModels;
using GestionParcAuto.Classes;
using System.Text;
using System.Xml.Linq;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authorization;

namespace GestionParcAuto.Controllers
{
    /// <summary>
    /// Controller of the users
    /// </summary>
    [Authorize(Roles = "Admin")]
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

        /// <summary>
        /// Page index
        /// </summary>
        /// <param name="message">message to show at page start</param>
        /// <returns>Index view</returns>
        public IActionResult Index(Message message)
        {
            if(message.Text != null)
            {
                ViewBag.Message = Json(message);
            }

            return View();
        }

        /// <summary>
        /// Page create
        /// </summary>
        /// <returns>Create view</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Page Edit
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User view</returns>
        [HttpGet]
        public IActionResult Edit(string id)
        {
            User? user = _userManager.FindByIdAsync(id).Result;

            if (user == null) 
                return NotFound();

            EditUserViewModel model = new EditUserViewModel() 
            { 
                Id = user.Id, 
                Email = user.Email, 
                Name = user.Name, 
                Surname = user.Surname,
                ChangePassword = user.ChangePassword,
            };
            return View(model);
        }

        #region Data

        /// <summary>
        /// Get user action
        /// </summary>
        /// <returns>Result with list of users</returns>
        public async Task<IActionResult> GetUsers()
        {
            List<User> Users = _userManager.Users.ToList();
            return new JsonResult(Users.Select(x => new { x.Id, x.FullName,  x.Email, roles = _userManager.GetRolesAsync(x).Result }).ToList());
        }

        #endregion

        #region POST

        /// <summary>
        /// Remove user action
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Result with message to show</returns>
        /// <exception cref="Exception">Impossible to remove user</exception>
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

        /// <summary>
        /// Create action
        /// </summary>
        /// <param name="vm">view model</param>
        /// <returns>Index action with message</returns>
        [HttpPost]
        public IActionResult Create(CreateUserViewModel vm)
        {
            User user = new User 
            { 
                Name = vm.Name, 
                Surname = vm.Surname, 
                Email = vm.Email, 
                UserName = vm.Email,
                ChangePassword = vm.ChangePassword
            };
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

            return RedirectToAction("Index", new Message { Title = "Création d'un utilisateur", Text = "L'utilisateur à été créée avec succès." });
        }
        
        /// <summary>
        /// Edit action
        /// </summary>
        /// <param name="vm">View model</param>
        /// <returns>Index action with message</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            User? user = _userManager.FindByIdAsync(vm.Id).Result;

            if (user == null)
                return NotFound();

            user.ChangePassword = vm.ChangePassword;
            var res = await _userManager.UpdateAsync(user);

            if (!res.Succeeded)
                return View(vm);

            return RedirectToAction("Index", new Message { Title = "Modification de l'utilisateur", Text = "L'utilisateur à été modifié avec succès." });
        }

        #endregion
    }
}
