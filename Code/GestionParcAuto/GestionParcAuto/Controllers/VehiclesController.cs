using GestionParcAuto.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionParcAuto.Classes;

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

        #region Data

        public async Task<IActionResult> GetVehicles()
        {
            return new JsonResult(_context.Vehicles.Select(x => new { }));
        }

        #endregion
    }
}
