using CyberSecLabPlatform.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CyberSecLabPlatform.Data
{
    public static class DbSeeder
    {
        public static void SeedData(IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            // Заполнение типов атак, если их еще нет
            if (!context.AttackTypes.Any())
            {
                context.AttackTypes.AddRange(
                    new AttackType 
                    { 
                        Name = "Фишинг", 
                        Description = "Атака через электронную почту для кражи учетных данных.",
                        Difficulty = "Средняя"
                    },
                    new AttackType 
                    { 
                        Name = "SQL-инъекция", 
                        Description = "Внедрение SQL-запросов через поля ввода.",
                        Difficulty = "Высокая"
                    },
                    new AttackType 
                    { 
                        Name = "XSS (Межсайтовый скриптинг)", 
                        Description = "Атака для внедрения скриптов на страницу.",
                        Difficulty = "Средняя"
                    }
                );
                context.SaveChanges();
            }

            // Заполнение сценариев, если их еще нет
            if (!context.Scenarios.Any())
            {
                // Получаем id типов атак для связи
                var phishingAttackType = context.AttackTypes.FirstOrDefault(a => a.Name == "Фишинг");
                var sqlInjectionAttackType = context.AttackTypes.FirstOrDefault(a => a.Name == "SQL-инъекция");
                var xssAttackType = context.AttackTypes.FirstOrDefault(a => a.Name.StartsWith("XSS"));

                context.Scenarios.AddRange(
                    new Scenario 
                    { 
                        Title = "Симуляция фишингового письма", 
                        AttackTypeId = phishingAttackType?.Id ?? 0,
                        IsActive = true,
                        MaxScore = 100,
                        Description = "Описание сценария для фишингового письма"
                    },
                    new Scenario 
                    { 
                        Title = "Обход входа через SQL-инъекцию", 
                        AttackTypeId = sqlInjectionAttackType?.Id ?? 0,
                        IsActive = true,
                        MaxScore = 120,
                        Description = "Описание сценария для SQL-инъекции"
                    },
                    new Scenario 
                    { 
                        Title = "Предотвращение внедрения скриптов", 
                        AttackTypeId = xssAttackType?.Id ?? 0,
                        IsActive = true,
                        MaxScore = 90,
                        Description = "Описание сценария для XSS атаки"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
