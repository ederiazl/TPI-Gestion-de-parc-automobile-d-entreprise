using GestionParcAuto.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionParcAuto.Classes;
using GestionParcAuto.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionParcAuto.Controllers
{
    [Authorize]
    public class VehiclesController : Controller
    {
        ApplicationDbContext _context;

        public VehiclesController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index(Message message)
        {
            if (message.Text != null)
            {
                ViewBag.Message = Json(message);
            }

            return View();
        }

        public IActionResult Create()
        {
            CreateStatusSelectList();

            return View();
        }

        #region Data

        public async Task<IActionResult> GetVehicles()
        {
            var vehicles = _context.Vehicles.Select(x => new { x.Id, x.Model, x.Registration, x.Make, x.Type, x.Mileage, x.VIN }).ToList();
            return new JsonResult(vehicles);
        }

        #endregion

        #region POST

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

        [HttpPost]
        public async Task<IActionResult> Create(Vehicle vehicle, IFormFile image)
        {
            if (image != null)
            {
                string path = Path.GetTempFileName();
                vehicle.Image = System.IO.File.ReadAllBytes(path);
            }

            if (!ModelState.IsValid)
            {
                CreateStatusSelectList();
                return View(vehicle);
            }

            await _context.Vehicles.AddAsync(vehicle);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new Message { Title = "Ajout d'un véhicule", Text = "Le véhicule à été ajouté avec succès." });
        }

        #endregion

        #region private

        private void CreateStatusSelectList()
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
            ViewBag.SelectList = new SelectList(list, "Val", "Display");
        }

        #endregion
    }
}
