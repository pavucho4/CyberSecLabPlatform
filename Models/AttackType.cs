using System.ComponentModel.DataAnnotations;

namespace CyberSecLabPlatform.Models
{
    public class AttackType
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Difficulty { get; set; }
    }
}
