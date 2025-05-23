using System;
using System.ComponentModel.DataAnnotations;

namespace CyberSecLabPlatform.Models
{
    public class Incident
    {
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }  // Тип атаки: SQL-инъекция, DDoS и т.п.

        public string Description { get; set; }

        [Display(Name = "Уровень угрозы")]
        public string Severity { get; set; }  // Low, Medium, High, Critical

        [Display(Name = "Время обнаружения")]
        public DateTimeOffset DetectedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}