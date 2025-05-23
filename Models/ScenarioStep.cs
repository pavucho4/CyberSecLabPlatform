using System.Collections.Generic;

namespace CyberSecLabPlatform.Models
{
    public class ScenarioStep
    {
        public int Id { get; set; }
        public int ScenarioId { get; set; }
        public Scenario Scenario { get; set; }

        public string StepText { get; set; }
        public bool IsFinal { get; set; }
        public int ScoreIfSuccess { get; set; }
        public int ScoreIfFail { get; set; }

        // Навигационное свойство - варианты ответов
        public ICollection<ScenarioStepOption> Options { get; set; } = new List<ScenarioStepOption>();
    }
}
