using GestionParcAuto.Data;
using GestionParcAuto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionParcAuto.Controllers
{
    [Authorize]
    public class ExpertisesController : Controller
    {

        ApplicationDbContext _context;

        public ExpertisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region POST

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

        #endregion
    }
}
