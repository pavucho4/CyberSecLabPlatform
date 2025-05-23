using System.Collections.Generic;

namespace CyberSecLabPlatform.Models
{
    public class SimulationViewModel
    {
        public AttackSimulationState Simulation { get; set; }
        public List<string> History { get; set; }
        public string Result { get; set; }
        public List<string> AvailableActions { get; set; }
    }
}
