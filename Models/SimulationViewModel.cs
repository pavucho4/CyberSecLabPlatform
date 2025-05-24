using System.Collections.Generic;

namespace CyberSecLabPlatform.Models
{
    public class AvailableAction
    {
        public int ActionId { get; set; }
        public string ActionName { get; set; }
    }

    public class SimulationViewModel
    {
        public AttackSimulationState Simulation { get; set; }
        public List<string> History { get; set; }
        public string Result { get; set; }
        public ScenarioStep CurrentStep { get; set; }
        public List<AvailableAction> AvailableActions { get; set; }
    }
}
