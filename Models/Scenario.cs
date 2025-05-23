using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CyberSecLabPlatform.Models
{
    public class Scenario
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AttackTypeId { get; set; }
        public AttackType AttackType { get; set; }

        // Новые поля
        public string Description { get; set; }
        public int MaxScore { get; set; }
        public bool IsActive { get; set; }

        // Новые добавленные поля для твоих вьюх
        public string Complexity { get; set; }           // Сложность сценария
        public string Instructions { get; set; }      // Инструкции к сценарию

        public ICollection<ScenarioStep> Steps { get; set; } = new List<ScenarioStep>();
    }
}
