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

        // GET: Показ формы запуска сценария
        [HttpGet]
        public async Task<IActionResult> ExecuteScenario(int assignmentId)
        {
            var user = await _userManager.GetUserAsync(User);

            var assignment = await _context.StudentAttackAssignments
                .Include(a => a.Scenario)
                    .ThenInclude(s => s.AttackType)
                .FirstOrDefaultAsync(a => a.Id == assignmentId && a.StudentId == user.Id);

            if (assignment == null)
                return NotFound();

            return View(assignment);
        }

        // POST: Запуск симуляции для выбранного задания
        [HttpPost]
        public async Task<IActionResult> ExecuteScenario(int assignmentId, string studentResponse)
        {
            var user = await _userManager.GetUserAsync(User);

            var assignment = await _context.StudentAttackAssignments
                .Include(a => a.Scenario)
                    .ThenInclude(s => s.Steps)
                .FirstOrDefaultAsync(a => a.Id == assignmentId && a.StudentId == user.Id);

            if (assignment == null)
                return NotFound();

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

        // GET: Показ текущего шага симуляции
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

            if (simulation.IsCompleted)
            {
                var assignment = await _context.StudentAttackAssignments
                    .FirstOrDefaultAsync(a => a.StudentId == user.Id && a.ScenarioId == simulation.ScenarioId && a.Completed);

                if (assignment != null)
                    return RedirectToAction(nameof(ScenarioResult), new { assignmentId = assignment.Id });

                return View("SimulationCompleted");
            }

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

        // POST: Обработка выбора действия на шаге
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

            var resultMessage = selectedOption.OptionText;

            string nextStepId = selectedOption.NextStepId?.ToString();

            history.Add($"[{DateTime.Now:HH:mm:ss}] Действие: {selectedOption.OptionText} - Результат: {resultMessage}");

            if (string.IsNullOrEmpty(nextStepId))
{
    // Если не указан следующий шаг, но в сценарии есть еще шаги после текущего — пробуем взять их
    var orderedSteps = simulation.Scenario.Steps.OrderBy(s => s.Id).ToList();
    var currentIndex = orderedSteps.FindIndex(s => s.Id == currentStep.Id);

    if (currentIndex >= 0 && currentIndex + 1 < orderedSteps.Count)
    {
        var nextStep = orderedSteps[currentIndex + 1];
        nextStepId = nextStep.Id.ToString();
    }
    else
    {
        // Действительно конец сценария
        simulation.IsCompleted = true;
        nextStepId = null;
    }
}


            simulation.HistoryJson = JsonConvert.SerializeObject(history);
            simulation.ResultJson = resultMessage;
            simulation.CurrentStep = nextStepId ?? simulation.CurrentStep;

            var assignment = await _context.StudentAttackAssignments
                .FirstOrDefaultAsync(a => a.StudentId == user.Id && a.ScenarioId == simulation.ScenarioId && !a.Completed);

            if (assignment != null)
            {
                if (selectedOption.IsCorrect)
                    assignment.Score += currentStep.ScoreIfSuccess;
                else
                    assignment.Score += currentStep.ScoreIfFail;

                if (simulation.IsCompleted)
                {
                    assignment.Completed = true;
                    // assignment.CompletedAt = DateTime.UtcNow; // Если нужно, можно раскомментировать
                }
            }

            await _context.SaveChangesAsync();

            if (simulation.IsCompleted)
            {
                // Теперь ищем уже завершённое задание для корректного редиректа
                assignment = await _context.StudentAttackAssignments
                    .FirstOrDefaultAsync(a => a.StudentId == user.Id && a.ScenarioId == simulation.ScenarioId && a.Completed);

                return RedirectToAction(nameof(ScenarioResult), new { assignmentId = assignment?.Id });
            }

            return RedirectToAction(nameof(SimulateStep), new { simulationId = simulation.Id });
        }

        // GET: Показ результата выполненного сценария
        [HttpGet]
        public async Task<IActionResult> ScenarioResult(int assignmentId)
        {
            var user = await _userManager.GetUserAsync(User);

            var assignment = await _context.StudentAttackAssignments
                .Include(a => a.Scenario)
                    .ThenInclude(s => s.AttackType)
                .FirstOrDefaultAsync(a => a.Id == assignmentId && a.StudentId == user.Id);

            if (assignment == null || !assignment.Completed)
                return NotFound();

            var model = new ScenarioResult
            {
                Assignment = assignment,
                Score = assignment.Score,
                IsPassed = true, // Здесь по умолчанию true
                CompletedAt = DateTime.UtcNow
            };

            return View(model);
        }

        // POST: Удаление назначения
        [HttpPost]
        public async Task<IActionResult> DeleteAssignment(int assignmentId)
        {
            var user = await _userManager.GetUserAsync(User);

            var assignment = await _context.StudentAttackAssignments
                .FirstOrDefaultAsync(a => a.Id == assignmentId && a.StudentId == user.Id);

            if (assignment == null)
                return NotFound();

            var simulations = await _context.AttackSimulationStates
                .Where(s => s.UserId == user.Id && s.ScenarioId == assignment.ScenarioId)
                .ToListAsync();

            if (simulations.Any())
                _context.AttackSimulationStates.RemoveRange(simulations);

            _context.StudentAttackAssignments.Remove(assignment);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
