using System.Collections.Generic;
namespace CyberSecLabPlatform.Models
{

    public class ScenarioResult
    {
        public int Id { get; set; }
        public int StudentAttackAssignmentId { get; set; }
        public StudentAttackAssignment Assignment { get; set; }
        public int Score { get; set; }
        public bool IsPassed { get; set; }
        public DateTime CompletedAt { get; set; }
        public string DetailedLogJson { get; set; } // Для анализа хода прохождения
    }
}