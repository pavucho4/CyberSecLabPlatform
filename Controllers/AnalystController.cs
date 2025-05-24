using CyberSecLabPlatform.Data;
using CyberSecLabPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        // ======= DASHBOARD =======

        public async Task<IActionResult> Index()
        {
            var students = await _userManager.GetUsersInRoleAsync("Student");
            return View(students);
        }

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

        // ======= ASSIGN SCENARIOS TO STUDENTS =======

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

        // ======= MANAGE SCENARIOS =======

        public async Task<IActionResult> ManageScenarios()
        {
            var scenarios = await _context.Scenarios
                .Include(s => s.AttackType)
                .ToListAsync();
            return View(scenarios);
        }

        public IActionResult CreateScenario()
        {
            ViewBag.AttackTypes = _context.AttackTypes.ToList();
            return View();
        }

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

        [HttpPost]
public async Task<IActionResult> EditScenario(Scenario model)
{
    if (!ModelState.IsValid)
    {
        Console.WriteLine("ModelState is invalid. Validation errors:");

        foreach (var key in ModelState.Keys)
        {
            var state = ModelState[key];
            foreach (var error in state.Errors)
            {
                Console.WriteLine($" - Key: {key}, Error: {error.ErrorMessage}");
            }
        }

        ViewBag.AttackTypes = _context.AttackTypes.ToList();
        return View(model);
    }

    var scenario = await _context.Scenarios
        .Include(s => s.Steps)
            .ThenInclude(step => step.Options)
        .FirstOrDefaultAsync(s => s.Id == model.Id);

    if (scenario == null)
    {
        Console.WriteLine($"Scenario with Id={model.Id} not found");
        return NotFound();
    }

    // Обновляем поля сценария
    scenario.Title = model.Title;
    scenario.Description = model.Description;
    scenario.AttackTypeId = model.AttackTypeId;
    scenario.MaxScore = model.MaxScore;
    scenario.IsActive = model.IsActive;
    scenario.Complexity = model.Complexity;
    scenario.Instructions = model.Instructions;

    var incomingStepIds = model.Steps?.Where(s => s.Id != 0).Select(s => s.Id).ToList() ?? new List<int>();

    // Удаляем шаги, которых нет в модели
    var stepsToRemove = scenario.Steps.Where(s => !incomingStepIds.Contains(s.Id)).ToList();
    Console.WriteLine($"Steps to remove: {stepsToRemove.Count}");
    foreach (var stepToRemove in stepsToRemove)
    {
        Console.WriteLine($"Removing Step Id={stepToRemove.Id}, Text={stepToRemove.StepText}");
        _context.ScenarioStepOptions.RemoveRange(stepToRemove.Options);
        _context.ScenarioSteps.Remove(stepToRemove);
    }

    // Обновляем или добавляем шаги
    foreach (var stepModel in model.Steps ?? new List<ScenarioStep>())
    {
        ScenarioStep step;

        if (stepModel.Id != 0)
        {
            step = scenario.Steps.FirstOrDefault(s => s.Id == stepModel.Id);
            if (step == null)
            {
                Console.WriteLine($"Step with Id={stepModel.Id} not found in scenario");
                continue;
            }

            step.StepText = stepModel.StepText;
            step.IsFinal = stepModel.IsFinal;
            step.ScoreIfSuccess = stepModel.ScoreIfSuccess;
            step.ScoreIfFail = stepModel.ScoreIfFail;

            var incomingOptionIds = stepModel.Options?.Where(o => o.Id != 0).Select(o => o.Id).ToList() ?? new List<int>();

            var optionsToRemove = step.Options.Where(o => !incomingOptionIds.Contains(o.Id)).ToList();
            Console.WriteLine($"Step Id={step.Id} Options to remove: {optionsToRemove.Count}");
            foreach (var optionToRemove in optionsToRemove)
            {
                Console.WriteLine($"Removing Option Id={optionToRemove.Id}, Text={optionToRemove.OptionText}");
                _context.ScenarioStepOptions.Remove(optionToRemove);
            }

            foreach (var optionModel in stepModel.Options ?? new List<ScenarioStepOption>())
            {
                ScenarioStepOption option;

                if (optionModel.Id != 0)
                {
                    option = step.Options.FirstOrDefault(o => o.Id == optionModel.Id);
                    if (option == null)
                    {
                        Console.WriteLine($"Option with Id={optionModel.Id} not found in step Id={step.Id}");
                        continue;
                    }

                    option.OptionText = optionModel.OptionText;
                    option.IsCorrect = optionModel.IsCorrect;
                    option.NextStepId = optionModel.NextStepId;
                }
                else
                {
                    option = new ScenarioStepOption
                    {
                        OptionText = optionModel.OptionText,
                        IsCorrect = optionModel.IsCorrect,
                        NextStepId = optionModel.NextStepId,
                        ScenarioStep = step
                    };
                    Console.WriteLine($"Adding new option to Step Id={step.Id}: Text={option.OptionText}");
                    _context.ScenarioStepOptions.Add(option);
                }
            }
        }
        else
        {
            step = new ScenarioStep
            {
                Scenario = scenario,
                StepText = stepModel.StepText,
                IsFinal = stepModel.IsFinal,
                ScoreIfSuccess = stepModel.ScoreIfSuccess,
                ScoreIfFail = stepModel.ScoreIfFail,
                Options = new List<ScenarioStepOption>()
            };
            Console.WriteLine($"Adding new step: Text={step.StepText}");
            _context.ScenarioSteps.Add(step);

            foreach (var optionModel in stepModel.Options ?? new List<ScenarioStepOption>())
            {
                var option = new ScenarioStepOption
                {
                    OptionText = optionModel.OptionText,
                    IsCorrect = optionModel.IsCorrect,
                    NextStepId = optionModel.NextStepId,
                    ScenarioStep = step
                };
                Console.WriteLine($"Adding new option to new Step: Text={option.OptionText}");
                _context.ScenarioStepOptions.Add(option);
            }
        }
    }

    try
    {
        var saveResult = await _context.SaveChangesAsync();
        Console.WriteLine($"SaveChangesAsync completed successfully, changes saved: {saveResult}");
    }
    catch (System.Exception ex)
    {
        Console.WriteLine($"Exception on SaveChangesAsync: {ex.Message}");
        ModelState.AddModelError("", "Ошибка при сохранении сценария.");
        ViewBag.AttackTypes = _context.AttackTypes.ToList();
        return View(model);
    }

    ViewBag.AttackTypes = _context.AttackTypes.ToList();

    // Перезагрузка сценария для отображения свежих данных
    var updatedScenario = await _context.Scenarios
        .Include(s => s.Steps)
            .ThenInclude(step => step.Options)
        .FirstOrDefaultAsync(s => s.Id == model.Id);

    if (updatedScenario == null)
    {
        Console.WriteLine("Updated scenario not found after save");
        return NotFound();
    }

    Console.WriteLine($"Returning updated scenario Id={updatedScenario.Id} with {updatedScenario.Steps.Count} steps");

    return View(updatedScenario);
}

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
