using GestionParcAuto.Data;
using GestionParcAuto.Models;
using GestionParcAuto.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestionParcAuto.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace GestionParcAuto.Controllers
{
    /// <summary>
    /// Controller of the expertises
    /// </summary>
    [Authorize]    
    public class ExpertisesController : Controller
    {

        ApplicationDbContext _context;
        UserManager<User> _userManager;

        public ExpertisesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Page index
        /// </summary>
        /// <returns>Index view</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Page create
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            ViewBag.VehicleSelect = new SelectList(_context.Vehicles, "Id", "Registration");
            ViewBag.UserSelect = new SelectList(_context.Users, "Id", "Fullname");


            return View();
        }

        #region GET

        public async Task<IActionResult> GetExpertises()
        {
            return new JsonResult(_context.Expertises.Include(x => x.User).Include(x => x.Vehicle).Select(x => new { x.Id, Date = x.Date.ToString("dd.MM.yyyy"), User = x.User.FullName ?? "", Registration = x.Vehicle.Registration, Make = x.Vehicle.Make }).ToList());
        }

        #endregion

        #region POST

        /// <summary>
        /// Post action for removing an expertise
        /// </summary>
        /// <param name="id">expertise id</param>
        /// <returns>result with alert message</returns>
        /// <exception cref="Exception">Expertise was not found</exception>
        [HttpPost]
        public async Task<IActionResult> RemoveExpertise(int id)
        {
            Expertise? expertise = _context.Expertises.Where(x => x.Id == id).First();

            if (expertise == null)
                throw new Exception($"Unable to find expertise with id:{id}");

            _context.Expertises.Remove(expertise);

            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                Success = true,
                Message = "Expertise supprimé avec succès."
            });
        }

        /// <summary>
        /// Action create expertise
        /// </summary>
        /// <param name="expertise">View model</param>
        /// <returns>Redirect to Index with message</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateExpertiseViewModel model)
        {
            Vehicle vehicle = _context.Vehicles.Where(x => x.Id == model.VehicleId).First();
            User? user = await _userManager.FindByIdAsync(model.UserId ?? "");

            Expertise expertise = new Expertise()
            {
                Date = model.Date,
                Status = false,
                Vehicle = vehicle,
                User = user,
            };

            await _context.Expertises.AddAsync(expertise);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new Message() { Title = "Ajout d'une expertise", Text = "Expertise ajoutée avec succès." });
        }

        #endregion
    }
}
