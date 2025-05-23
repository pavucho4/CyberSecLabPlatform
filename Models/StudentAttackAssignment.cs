namespace CyberSecLabPlatform.Models
{
    public class StudentAttackAssignment
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        public int ScenarioId { get; set; }
        public Scenario Scenario { get; set; }

        public bool Completed { get; set; }
        public int Score { get; set; }  // Например, баллы за выполнение
    }
}
