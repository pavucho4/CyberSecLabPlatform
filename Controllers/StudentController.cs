using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CyberSecLabPlatform.Data;
using CyberSecLabPlatform.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CyberSecLabPlatform.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Показ назначенных студенту сценариев
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var assignments = await _context.StudentAttackAssignments
                .Include(a => a.Scenario)
                    .ThenInclude(s => s.AttackType)
                .Where(a => a.StudentId == user.Id)
                .ToListAsync();

            return View(assignments);
        }

        // Запуск симуляции для выбранного задания
        public async Task<IActionResult> ExecuteScenario(int assignmentId)
        {
            var user = await _userManager.GetUserAsync(User);

            var assignment = await _context.StudentAttackAssignments
                .Include(a => a.Scenario)
                    .ThenInclude(s => s.Steps)
                .FirstOrDefaultAsync(a => a.Id == assignmentId && a.StudentId == user.Id);

            if (assignment == null)
                return NotFound();

            // Берём первый шаг сценария по Order (предполагается, что есть поле Order для сортировки шагов)
            var firstStep = assignment.Scenario.Steps.OrderBy(step => step.Id).FirstOrDefault();
            if (firstStep == null)
                return BadRequest("В сценарии нет шагов.");

            var simulation = new AttackSimulationState
            {
                ScenarioId = assignment.ScenarioId,
                CurrentStep = firstStep.Id.ToString(),
                HistoryJson = "[]",
                ResultJson = "",
                UserId = user.Id,
                IsCompleted = false
            };

            _context.AttackSimulationStates.Add(simulation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(SimulateStep), new { simulationId = simulation.Id });
        }

        [HttpGet]
        public async Task<IActionResult> SimulateStep(int simulationId)
        {
            var user = await _userManager.GetUserAsync(User);
            var simulation = await _context.AttackSimulationStates
                .Include(s => s.Scenario)
                    .ThenInclude(s => s.Steps)
                        .ThenInclude(step => step.Options)
                .FirstOrDefaultAsync(s => s.Id == simulationId && s.UserId == user.Id);

            if (simulation == null)
                return NotFound();

            var history = JsonConvert.DeserializeObject<List<string>>(simulation.HistoryJson) ?? new List<string>();

            bool isValidStep = int.TryParse(simulation.CurrentStep, out int currentStepId);
            ScenarioStep currentStep = null;
            if (isValidStep)
            {
                currentStep = simulation.Scenario.Steps.FirstOrDefault(s => s.Id == currentStepId);
            }

            var availableActions = currentStep?.Options
                .Select(o => new AvailableAction
                {
                    ActionId = o.Id,
                    ActionName = o.OptionText
                }).ToList() ?? new List<AvailableAction>();

            var model = new SimulationViewModel
            {
                Simulation = simulation,
                History = history,
                Result = simulation.ResultJson,
                CurrentStep = currentStep,
                AvailableActions = availableActions
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SimulateStep(int simulationId, int actionId)
        {
            var user = await _userManager.GetUserAsync(User);
            var simulation = await _context.AttackSimulationStates
                .Include(s => s.Scenario)
                    .ThenInclude(s => s.Steps)
                        .ThenInclude(step => step.Options)
                .FirstOrDefaultAsync(s => s.Id == simulationId && s.UserId == user.Id);

            if (simulation == null || simulation.IsCompleted)
                return NotFound();

            var history = JsonConvert.DeserializeObject<List<string>>(simulation.HistoryJson) ?? new List<string>();

            bool isValidStep = int.TryParse(simulation.CurrentStep, out int currentStepId);
            if (!isValidStep)
                return BadRequest("Некорректный текущий шаг.");

            var currentStep = simulation.Scenario.Steps.FirstOrDefault(s => s.Id == currentStepId);
            if (currentStep == null)
                return BadRequest("Шаг не найден.");

            var selectedOption = currentStep.Options.FirstOrDefault(o => o.Id == actionId);
            if (selectedOption == null)
                return BadRequest("Выбранное действие недействительно.");

            // Для модели ScenarioStepOption в твоём описании нет поля Result, 
            // добавим для логики здесь, можно его создать или временно заменить на OptionText
            var resultMessage = selectedOption.OptionText;

            string nextStepId = selectedOption.NextStepId != 0 ? selectedOption.NextStepId.ToString() : null;

            if (string.IsNullOrEmpty(nextStepId))
            {
                // Если nextStepId == 0, значит это конец сценария
                simulation.IsCompleted = true;
            }

            // Добавляем запись в историю
            history.Add($"[{DateTime.Now:HH:mm:ss}] Действие: {selectedOption.OptionText} - Результат: {resultMessage}");

            simulation.HistoryJson = JsonConvert.SerializeObject(history);
            simulation.ResultJson = resultMessage;
            simulation.CurrentStep = nextStepId ?? simulation.CurrentStep;

            // Отметим выполнение задания, если симуляция завершена
            if (simulation.IsCompleted)
            {
                var assignment = await _context.StudentAttackAssignments
                    .FirstOrDefaultAsync(a => a.StudentId == user.Id && a.ScenarioId == simulation.ScenarioId && !a.Completed);

                if (assignment != null)
                {
                    assignment.Completed = true;
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(SimulateStep), new { simulationId = simulation.Id });
        }
    }

    // Модель для передачи действия в вьюху
    public class AvailableAction
    {
        public int ActionId { get; set; }
        public string ActionName { get; set; }
    }

    // Модель для передачи данных в вьюху симуляции
    public class SimulationViewModel
    {
        public AttackSimulationState Simulation { get; set; }
        public List<string> History { get; set; }
        public string Result { get; set; }
        public ScenarioStep CurrentStep { get; set; }
        public List<AvailableAction> AvailableActions { get; set; }
    }
}
