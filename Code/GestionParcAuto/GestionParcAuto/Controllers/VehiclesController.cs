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
            CreateStatusSelectList(null);

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

        #endregion

        #region private

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
