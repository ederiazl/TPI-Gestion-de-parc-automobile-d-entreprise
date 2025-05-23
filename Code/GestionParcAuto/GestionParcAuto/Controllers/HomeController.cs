using GestionParcAuto.Classes;
using GestionParcAuto.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionParcAuto.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            if (User.IsInRole("Employee"))
                return RedirectToAction("Index", "Vehicles");
            return View();
        }

        #region Data

        public IActionResult GetNextExpertises() => new JsonResult(_context.Expertises.Where(x => x.Date >= DateTime.Now && x.Status == false).OrderBy(x => x.Date).Take(10).Include(x => x.Vehicle).Include(x => x.User).Select(x => new {x.Id, Date = x.Date.ToString("dd.MM.yyyy HH:mm"), x.Vehicle.Make, x.Vehicle.Model, x.Vehicle.Registration, User = x.User.FullName ?? ""}).ToList());

        public IActionResult GetLastVehicles() => new JsonResult(_context.Vehicles.Where(x => x.Status == VehicleStatus.NORMAL).OrderByDescending(x => x.Id).Take(10).Include(x => x.Expertises).Select(x => new { x.Id, x.Make, x.Model, x.Registration, x.Mileage, NextExpertise = x.Expertises.Select(y => y.Date).Max().ToString("dd.MM.yyyy HH:mm") ?? "" }));

        #endregion
    }
}
