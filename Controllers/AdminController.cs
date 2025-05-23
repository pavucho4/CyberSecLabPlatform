using CyberSecLabPlatform.Models;
using CyberSecLabPlatform.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CyberSecLabPlatform.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Дашборд админа: список типов атак
        public IActionResult Index()
        {
            var attackTypes = _context.AttackTypes.ToList();
            return View(attackTypes);
        }

        // Форма создания нового типа атаки
        public IActionResult CreateAttackType()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAttackType(AttackType model)
        {
            if (ModelState.IsValid)
            {
                _context.AttackTypes.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
