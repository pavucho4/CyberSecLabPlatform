using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CyberSecLabPlatform.Data;
using CyberSecLabPlatform.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CyberSecLabPlatform.Controllers
{
    public class IncidentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IncidentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Доступен всем авторизованным пользователям
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var incidents = await _context.Incidents.ToListAsync();
            return View(incidents);
        }

        // Просмотр деталей инцидента
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var incident = await _context.Incidents.FirstOrDefaultAsync(x => x.Id == id);
            if (incident == null)
                return NotFound();

            return View(incident);
        }

        // Только Admin может создать
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Incident incident)
        {
            if (ModelState.IsValid)
            {
                _context.Incidents.Add(incident);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(incident);
        }
    }
}
