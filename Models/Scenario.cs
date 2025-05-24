#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CyberSecLabPlatform.Models
{
    public class Scenario
    {
        public int Id { get; set; }

        [Required]  // Title обычно обязателен
        public string Title { get; set; }

        [Required]  // Обязательно, внешний ключ
        public int AttackTypeId { get; set; }

        public AttackType? AttackType { get; set; }  // Навигационное свойство — НЕ ставим [Required]

        public string? Description { get; set; }
        public int MaxScore { get; set; }
        public bool IsActive { get; set; }

        [Required]  // Обязательно
        public string Complexity { get; set; }

        public string? Instructions { get; set; }

        public virtual ICollection<ScenarioStep> Steps { get; set; } = new List<ScenarioStep>();
    }
}
