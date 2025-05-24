#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CyberSecLabPlatform.Models
{
    public class ScenarioStep
    {
        public int Id { get; set; }

        [Required]  // Обязательный внешний ключ
        public int ScenarioId { get; set; }

        public Scenario? Scenario { get; set; }  // НЕ ставим [Required]

        [Required]  // Обычно обязательно
        public string StepText { get; set; }

        public bool IsFinal { get; set; }

        public int ScoreIfSuccess { get; set; }
        public int ScoreIfFail { get; set; }

        public ICollection<ScenarioStepOption> Options { get; set; } = new List<ScenarioStepOption>();
    }
}
