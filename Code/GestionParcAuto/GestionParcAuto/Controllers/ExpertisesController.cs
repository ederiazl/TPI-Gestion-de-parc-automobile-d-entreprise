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
        public IActionResult Index(Message message)
        {
            if (message.Text != null)
            {
                ViewBag.Message = Json(message);
            }

            return View();
        }

        /// <summary>
        /// Page create
        /// </summary>
        /// <returns>Create view</returns>
        public IActionResult Create()
        {
            CreateVehicleSelectList(null);
            CreateEmployeeSelectList(null);

            return View();
        }

        /// <summary>
        /// Page edit
        /// </summary>
        /// <returns>Edit view</returns>
        public IActionResult Edit(int id)
        {
            Expertise expertise = _context.Expertises.Where(x => x.Id == id).Include(x => x.Vehicle).Include(x => x.User).First();

            CreateVehicleSelectList(expertise.Vehicle.Id);
            CreateEmployeeSelectList(expertise.User?.Id);

            EditExpertiseViewModel vm = new EditExpertiseViewModel()
            {
                Id = id,
                Date = expertise.Date,
                VehicleId = expertise.Vehicle.Id,
                UserId = expertise.User?.Id,
                Note = expertise.Note,
                Status = expertise.Status,
            };

            return View(vm);
        }

        /// <summary>
        /// Page detail
        /// </summary>
        /// <returns>Detail view</returns>
        public IActionResult Detail(int id)
        {
            Expertise expertise = _context.Expertises.Where(x => x.Id == id).Include(x => x.Vehicle).Include(x => x.User).First();

            return View(expertise);
        }

        #region GET

        public async Task<IActionResult> GetExpertises()
        {
            return new JsonResult(_context.Expertises.Include(x => x.User).Include(x => x.Vehicle).Select(x => new { x.Id, Date = x.Date.ToString("dd.MM.yyyy HH:mm"), User = x.User.FullName ?? "", Registration = x.Vehicle.Registration, Make = x.Vehicle.Make, x.Status }).ToList());
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
                Message = "Expertise supprimée avec succès."
            });
        }

        /// <summary>
        /// Post action for approving an expertise
        /// </summary>
        /// <param name="id">expertise id</param>
        /// <returns>result with alert message</returns>
        /// <exception cref="Exception">Expertise was not found</exception>
        [HttpPost]
        public async Task<IActionResult> ValidateExpertise(int id, string note)
        {
            Expertise? expertise = _context.Expertises.Where(x => x.Id == id).First();

            if (expertise == null)
                throw new Exception($"Unable to find expertise with id:{id}");

            expertise.Status = true;
            expertise.Note = note;

            _context.Expertises.Update(expertise);

            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                Success = true,
                Message = "Expertise a été validée avec succès."
            });
        }

        /// <summary>
        /// Post action for unvalidate an expertise
        /// </summary>
        /// <param name="id">expertise id</param>
        /// <returns>result with alert message</returns>
        /// <exception cref="Exception">Expertise was not found</exception>
        [HttpPost]
        public async Task<IActionResult> UnValidateExpertise(int id)
        {
            Expertise? expertise = _context.Expertises.Where(x => x.Id == id).First();

            if (expertise == null)
                throw new Exception($"Unable to find expertise with id:{id}");

            expertise.Status = false;
            expertise.Note = "";

            _context.Expertises.Update(expertise);

            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                Success = true,
                Message = "Expertise a été dévalidée avec succès."
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

        /// <summary>
        /// Action edit expertise
        /// </summary>
        /// <param name="expertise">View model</param>
        /// <returns>Redirect to Index with message</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(EditExpertiseViewModel model)
        {
            Vehicle vehicle = _context.Vehicles.Where(x => x.Id == model.VehicleId).First();
            User? user = await _userManager.FindByIdAsync(model.UserId ?? "");

            Expertise expertise = _context.Expertises.Where(x => x.Id == model.Id).First();

            expertise.Date = model.Date;
            expertise.Status = model.Status;
            expertise.Vehicle = vehicle;
            expertise.User = user;
            expertise.Note = model.Note;

            _context.Update(expertise);

            int modifications = _context.SaveChanges();

            return RedirectToAction("Index", new Message() { Title = "Modification de l'expertise", Text = "Expertise modifiée avec succès." });
        }

        #endregion

        #region private

        private void CreateVehicleSelectList(int? selectedId)
        {
            ViewBag.VehicleSelectList = new SelectList(_context.Vehicles, "Id", "Registration", selectedId);
        }

        private void CreateEmployeeSelectList(string? selectedId)
        {
            ViewBag.UserSelectList = new SelectList(_userManager.GetUsersInRoleAsync("Employee").Result, "Id", "UserName", selectedId);
        }

        #endregion
    }
}
