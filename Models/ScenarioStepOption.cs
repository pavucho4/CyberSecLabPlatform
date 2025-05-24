using System.ComponentModel.DataAnnotations;

namespace CyberSecLabPlatform.Models
{
    public class ScenarioStepOption
    {
        public int Id { get; set; }

        [Required]  // Внешний ключ обязателен
        public int ScenarioStepId { get; set; }

        public ScenarioStep? ScenarioStep { get; set; }  // НЕ ставим [Required]

        [Required]
        public string OptionText { get; set; }

        public bool IsCorrect { get; set; }

        public int? NextStepId { get; set; }  // Nullable, так как может быть последним шагом

        public ScenarioStep? NextStep { get; set; }  // НЕ ставим [Required]
    }
}
