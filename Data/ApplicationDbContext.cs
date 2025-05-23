using CyberSecLabPlatform.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CyberSecLabPlatform.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Incident> Incidents { get; set; }
        public DbSet<AttackType> AttackTypes { get; set; }
        public DbSet<Scenario> Scenarios { get; set; }
        public DbSet<ScenarioStep> ScenarioSteps { get; set; }
        public DbSet<ScenarioStepOption> ScenarioStepOptions { get; set; }
        public DbSet<ScenarioResult> ScenarioResults { get; set; }

        public DbSet<StudentAttackAssignment> StudentAttackAssignments { get; set; }
        public DbSet<AttackSimulationState> AttackSimulationStates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Связь ScenarioStep -> ScenarioStepOption (1 - много)
            builder.Entity<ScenarioStep>()
                .HasMany(s => s.Options)
                .WithOne(o => o.ScenarioStep)
                .HasForeignKey(o => o.ScenarioStepId)
                .OnDelete(DeleteBehavior.Cascade);

            // Рекурсивная связь NextStep в ScenarioStepOption
            builder.Entity<ScenarioStepOption>()
                .HasOne(o => o.NextStep)
                .WithMany()
                .HasForeignKey(o => o.NextStepId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
