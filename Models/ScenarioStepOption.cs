namespace CyberSecLabPlatform.Models
{
    public class ScenarioStepOption
    {
        public int Id { get; set; }
        public int ScenarioStepId { get; set; }
        public ScenarioStep ScenarioStep { get; set; }

        public string OptionText { get; set; }
        public bool IsCorrect { get; set; }

        // Следующий шаг - nullable, т.к. может быть конец сценария
        public int? NextStepId { get; set; }
        public ScenarioStep NextStep { get; set; }
    }
}
