using CyberSecLabPlatform.Data;
using CyberSecLabPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CyberSecLabPlatform.Controllers
{
    [Authorize(Roles = "Analyst")]
    public class AnalystController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnalystController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Дашборд: список студентов
        public async Task<IActionResult> Index()
        {
            var students = await _userManager.GetUsersInRoleAsync("Student");
            return View(students);
        }

        // Просмотр результатов студента (с оценками)
        public async Task<IActionResult> StudentsResults(string studentId)
        {
            var results = await _context.StudentAttackAssignments
                .Include(s => s.Scenario)
                    .ThenInclude(sc => sc.AttackType)
                .Where(s => s.StudentId == studentId)
                .ToListAsync();

            ViewBag.StudentId = studentId;
            return View(results);
        }

        // Форма назначения сценариев студентам
        public async Task<IActionResult> AssignAttacks()
        {
            ViewBag.Students = await _userManager.GetUsersInRoleAsync("Student");
            ViewBag.Scenarios = await _context.Scenarios.Include(s => s.AttackType).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssignAttacks(string studentId, int scenarioId)
        {
            if (!_context.StudentAttackAssignments.Any(sa => sa.StudentId == studentId && sa.ScenarioId == scenarioId))
            {
                _context.StudentAttackAssignments.Add(new StudentAttackAssignment
                {
                    StudentId = studentId,
                    ScenarioId = scenarioId,
                    Completed = false,
                    Score = 0
                });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AssignAttacks));
        }

        // ======= Новый функционал: управление сценариями ========

        // Список всех сценариев
        public async Task<IActionResult> ManageScenarios()
        {
            var scenarios = await _context.Scenarios
                .Include(s => s.AttackType)
                .ToListAsync();
            return View(scenarios);
        }

        // Создание нового сценария — GET
        public IActionResult CreateScenario()
        {
            ViewBag.AttackTypes = _context.AttackTypes.ToList();
            return View();
        }

        // Создание сценария — POST
        [HttpPost]
        public async Task<IActionResult> CreateScenario(Scenario model)
        {
            if (ModelState.IsValid)
            {
                _context.Scenarios.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageScenarios));
            }
            ViewBag.AttackTypes = _context.AttackTypes.ToList();
            return View(model);
        }

        // Редактирование сценария — GET
        public async Task<IActionResult> EditScenario(int id)
        {
            var scenario = await _context.Scenarios
                .Include(s => s.Steps)
                    .ThenInclude(step => step.Options)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (scenario == null)
                return NotFound();

            ViewBag.AttackTypes = _context.AttackTypes.ToList();
            return View(scenario);
        }

        // Редактирование сценария — POST
        [HttpPost]
        public async Task<IActionResult> EditScenario(Scenario model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AttackTypes = _context.AttackTypes.ToList();
                return View(model);
            }

            var scenario = await _context.Scenarios
                .Include(s => s.Steps)
                    .ThenInclude(step => step.Options)
                .FirstOrDefaultAsync(s => s.Id == model.Id);

            if (scenario == null)
                return NotFound();

            // Обновляем простые поля
            scenario.Title = model.Title;
            scenario.Description = model.Description;
            scenario.AttackTypeId = model.AttackTypeId;

            // Логика обновления шагов и опций — лучше реализовывать через отдельный интерфейс,
            // здесь можно оставить только обновление основных полей для краткости

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageScenarios));
        }

        // Просмотр деталей сценария (с шагами и опциями)
        public async Task<IActionResult> ScenarioDetails(int id)
        {
            var scenario = await _context.Scenarios
                .Include(s => s.AttackType)
                .Include(s => s.Steps)
                    .ThenInclude(step => step.Options)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (scenario == null)
                return NotFound();

            return View(scenario);
        }
    }
}
