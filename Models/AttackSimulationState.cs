namespace CyberSecLabPlatform.Models
{
    public class AttackSimulationState
    {
        public int Id { get; set; }
        public int ScenarioId { get; set; }
        public string CurrentStep { get; set; }
        public string HistoryJson { get; set; }
        public string ResultJson { get; set; }
        public string UserId { get; set; }
        public bool IsCompleted { get; set; }

        // Добавляем навигационное свойство для удобства Include()
        public Scenario Scenario { get; set; }
    }
}