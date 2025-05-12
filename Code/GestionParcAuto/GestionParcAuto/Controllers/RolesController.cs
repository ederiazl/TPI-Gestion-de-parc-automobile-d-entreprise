using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GestionParcAuto.Models;

namespace TestASP.Controllers
{
    /// <summary>
    /// Controller of the roles
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        #region Page

        /// <summary>
        /// Page index
        /// </summary>
        /// <returns>Index view</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Page users
        /// </summary>
        /// <param name="id">Role id</param>
        /// <returns>View users</returns>
        public async Task<IActionResult> Users(string id)
        {
            IdentityRole? role = await _roleManager.FindByIdAsync(id);

            if (role != null)
                return View(role);
            else
                return NotFound();
        }

        /// <summary>
        /// Page add users
        /// </summary>
        /// <param name="id">Role Id</param>
        /// <returns>Add users view</returns>
        public async Task<IActionResult> AddUsers(string id)
        {
            IdentityRole? role = await _roleManager.FindByIdAsync(id);

            if (role != null)
                return View(role);
            else
                return NotFound();
        }
        #endregion

        #region Post

        /// <summary>
        /// Remove user action
        /// </summary>
        /// <param name="roleId">Role Id</param>
        /// <param name="userId">User Id</param>
        /// <returns>Result with message</returns>
        /// <exception cref="Exception">Unable to find user or role</exception>
        [HttpPost]
        public async Task<IActionResult> RemoveUser(string roleId, string userId)
        {
            IdentityRole? role = await _roleManager.FindByIdAsync(roleId);
            User? user = await _userManager.FindByIdAsync(userId);

            if (role != null && user != null)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, role.Name);

                if (result.Succeeded)
                {
                    return new JsonResult(new
                    {
                        Success = true,
                        Message = "Utilisateur retiré avec succès."
                    });
                }
                else
                {
                    throw new Exception("Unable to remove user from role.");
                }
            }
            else
            {
                throw new Exception("Invalid user or role id");
            }
        }

        /// <summary>
        /// Add user action
        /// </summary>
        /// <param name="roleId">Role Id</param>
        /// <param name="userId">User id</param>
        /// <returns>Result with message</returns>
        /// <exception cref="Exception">Unable to find role or user</exception>
        [HttpPost]
        public async Task<IActionResult> AddUser(string roleId, string userId)
        {
            IdentityRole? role = await _roleManager.FindByIdAsync(roleId);
            User? user = await _userManager.FindByIdAsync(userId);

            if (role != null && user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, role.Name);

                if (result.Succeeded)
                {
                    return new JsonResult(new
                    {
                        Success = true,
                        Message = "Utilisateur ajouté avec succès."
                    });
                }
                else
                {
                    throw new Exception("Unable to add user to role.");
                }
            }
            else
            {
                throw new Exception("Invalid user or role id");
            }
        }
        #endregion

        #region Data
        /// <summary>
        /// Get Role action
        /// </summary>
        /// <returns>Result with list of roles</returns>
        [HttpGet]
        public IActionResult GetRoles()
        {
            return new JsonResult(_roleManager.Roles.ToList());
        }

        /// <summary>
        /// Get users action
        /// </summary>
        /// <param name="id">Role id</param>
        /// <returns>Result with user in role</returns>
        /// <exception cref="Exception">Invalid role id</exception>
        [HttpGet]
        public async Task<IActionResult> GetUsers(string id)
        {
            IdentityRole? role = await _roleManager.FindByIdAsync(id);

            if(role != null)
            {
                return new JsonResult(await _userManager.GetUsersInRoleAsync(role.Name));
            }
            else
            {
                throw new Exception("Invalid role id");
            }
        }

        /// <summary>
        /// Get users to add action
        /// </summary>
        /// <param name="id">role id</param>
        /// <returns>Result with users that can be added to role</returns>
        /// <exception cref="Exception">Invalid role id</exception>
        [HttpGet]
        public async Task<IActionResult> GetUsersToAdd(string id)
        {
            IdentityRole? role = await _roleManager.FindByIdAsync(id);

            if (role != null)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
                return new JsonResult(_userManager.Users.Where(x => !usersInRole.Contains(x)));
            }
            else
                throw new Exception("Invalid role id");
        }
        #endregion
    }
}
