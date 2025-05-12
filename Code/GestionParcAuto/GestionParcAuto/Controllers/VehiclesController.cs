using GestionParcAuto.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionParcAuto.Classes;
using GestionParcAuto.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace GestionParcAuto.Controllers
{
    /// <summary>
    /// Controller of the vehicles
    /// </summary>
    [Authorize]
    public class VehiclesController : Controller
    {
        ApplicationDbContext _context;

        public VehiclesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Page index
        /// </summary>
        /// <param name="message">Message to show at page start</param>
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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            CreateStatusSelectList(null);

            return View();
        }

        /// <summary>
        /// Page edit
        /// </summary>
        /// <param name="id">Vehicle id</param>
        /// <returns>Edit view</returns>
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            Vehicle? vm = _context.Vehicles.Where(x => x.Id == id).First();

            if (vm == null)
                return NotFound();

            CreateStatusSelectList(vm.Status);

            return View(vm);
        }

        /// <summary>
        /// Page detail
        /// </summary>
        /// <param name="id">Vehicle id</param>
        /// <returns>Detail view</returns>
        public IActionResult Detail(int id)
        {
            Vehicle? vm = _context.Vehicles.Where(x => x.Id == id).First();

            if (vm == null)
                return NotFound();

            return View(vm);
        }

        #region Data

        /// <summary>
        /// Get vehicle action
        /// </summary>
        /// <returns>Result with list of vehicles</returns>
        public async Task<IActionResult> GetVehicles()
        {
            var vehicles = _context.Vehicles.Select(x => new { x.Id, x.Model, x.Registration, x.Make, x.Type, x.Mileage, x.VIN }).ToList();
            return new JsonResult(vehicles);
        }

        /// <summary>
        /// Get expertise action
        /// </summary>
        /// <param name="id">Vehicle id</param>
        /// <returns>Result with list of expertises</returns>
        public async Task<IActionResult> GetExpertises(int id)
        {
            var expertises = _context.Vehicles.Where(x => x.Id == id).Include(x => x.Expertises).ThenInclude(x => x.User).SelectMany(x => x.Expertises).Select(x => new { x.Id, x.Date, x.Status, user = x.User.FullName}).ToList();
            return new JsonResult(expertises);
        }

        #endregion

        #region POST

        /// <summary>
        /// Remove vehicle action
        /// </summary>
        /// <param name="id">Vehicle id</param>
        /// <returns>Result with message to display</returns>
        /// <exception cref="Exception">Unable to find vehicle</exception>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RemoveVehicle(int id)
        {
            Vehicle? vehicle = _context.Vehicles.Where(x => x.Id == id).First();

            if (vehicle == null)
                throw new Exception($"Unable to find vehicle with id:{id}");

            _context.Vehicles.Remove(vehicle);

            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                Success = true,
                Message = "Véhicule supprimé avec succès."
            });
        }

        /// <summary>
        /// Create vehicle action
        /// </summary>
        /// <param name="vehicle">Vehicle</param>
        /// <param name="image">Image file</param>
        /// <returns>Redirect to index with message</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Vehicle vehicle, IFormFile image)
        {
            if (image != null)
            {
                vehicle.Image = ReadFully(image.OpenReadStream());
            }

            if (!ModelState.IsValid)
            {
                CreateStatusSelectList(null);
                return View(vehicle);
            }

            await _context.Vehicles.AddAsync(vehicle);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new Message { Title = "Ajout d'un véhicule", Text = "Le véhicule à été ajouté avec succès." });
        }

        /// <summary>
        /// Edit vehicle action
        /// </summary>
        /// <param name="vehicle">vehicle</param>
        /// <param name="image">Image file</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(Vehicle vehicle, IFormFile? image)
        {
            Vehicle? dbVehicle = _context.Vehicles.Where(x => x.Id == vehicle.Id).First();

            if (dbVehicle == null)
                return NotFound();

            _context.Entry(dbVehicle).CurrentValues.SetValues(vehicle);

            if (image != null)
                dbVehicle.Image = ReadFully(image.OpenReadStream());
            else
                dbVehicle.Image = _context.Entry(dbVehicle).Property(x => x.Image).OriginalValue;

            if (!ModelState.IsValid)
            {
                CreateStatusSelectList(dbVehicle.Status);
                return View(dbVehicle);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new Message { Title = "Modification d'un véhicule", Text = "Le véhicule à été modifié avec succès." });
        }

        #endregion

        #region private

        /// <summary>
        /// Generates status select list
        /// </summary>
        /// <param name="selected">Status selected</param>
        private void CreateStatusSelectList(char? selected)
        {
            var list = new List<object>()
            {
                new
                {
                    Val = VehicleStatus.NORMAL,
                    Display = "Normal"
                },
                new
                {
                    Val = VehicleStatus.SOLD,
                    Display = "Vendu"
                },
                new
                {
                    Val = VehicleStatus.REPARE,
                    Display = "Réparation"
                }
            };
            ViewBag.SelectList = new SelectList(list, "Val", "Display", selected);
        }

        /// <summary>
        /// Read stream
        /// </summary>
        /// <param name="input">Stream</param>
        /// <returns>Byte array</returns>
        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }

            #endregion
        }
    }
}
