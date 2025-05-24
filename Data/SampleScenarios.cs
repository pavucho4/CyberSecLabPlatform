using CyberSecLabPlatform.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecLabPlatform.Data
{
    public static class SampleScenarios
    {
        public static void SeedSampleScenarios(this ModelBuilder modelBuilder)
        {
            // Sample Attack Types
            modelBuilder.Entity<AttackType>().HasData(
                new AttackType { Id = 1, Name = "SQL Injection", Description = "Атака, направленная на внедрение SQL-кода", Difficulty = "Средний" },
                new AttackType { Id = 2, Name = "Cross-Site Scripting (XSS)", Description = "Атака, направленная на внедрение вредоносного скрипта", Difficulty = "Средний" },
                new AttackType { Id = 3, Name = "Phishing", Description = "Социальная инженерия для получения конфиденциальной информации", Difficulty = "Легкий" },
                new AttackType { Id = 4, Name = "DDoS", Description = "Распределенная атака отказа в обслуживании", Difficulty = "Сложный" }
            );

            // Sample Scenarios
            modelBuilder.Entity<Scenario>().HasData(
                new Scenario
                {
                    Id = 1,
                    Title = "Обнаружение SQL Injection",
                    Description = "Сценарий для обучения обнаружению и предотвращению SQL Injection атак",
                    AttackTypeId = 1,
                    MaxScore = 100,
                    IsActive = true,
                    Complexity = "Средний",
                    Instructions = "В этом сценарии вы будете анализировать логи веб-сервера для выявления попыток SQL Injection атак. Следуйте инструкциям на каждом шаге."
                },
                new Scenario
                {
                    Id = 2,
                    Title = "Защита от XSS атак",
                    Description = "Сценарий для обучения методам защиты от Cross-Site Scripting атак",
                    AttackTypeId = 2,
                    MaxScore = 80,
                    IsActive = true,
                    Complexity = "Средний",
                    Instructions = "Вам предстоит проанализировать код веб-приложения и найти уязвимости XSS. Затем вы должны предложить методы их устранения."
                },
                new Scenario
                {
                    Id = 3,
                    Title = "Распознавание фишинговых писем",
                    Description = "Сценарий для обучения распознаванию фишинговых писем и сайтов",
                    AttackTypeId = 3,
                    MaxScore = 60,
                    IsActive = true,
                    Complexity = "Легкий",
                    Instructions = "В этом сценарии вы будете анализировать различные электронные письма и определять, являются ли они фишинговыми. Обратите внимание на характерные признаки фишинга."
                },
                new Scenario
                {
                    Id = 4,
                    Title = "Анализ и предотвращение DDoS атак",
                    Description = "Сценарий для обучения методам анализа и предотвращения DDoS атак",
                    AttackTypeId = 4,
                    MaxScore = 120,
                    IsActive = true,
                    Complexity = "Сложный",
                    Instructions = "Вы будете анализировать сетевой трафик во время DDoS атаки и разрабатывать стратегию защиты. Используйте предоставленные инструменты для анализа и фильтрации трафика."
                }
            );

            // Sample Scenario Steps for SQL Injection scenario
            modelBuilder.Entity<ScenarioStep>().HasData(
                new ScenarioStep
                {
                    Id = 1,
                    ScenarioId = 1,
                    StepText = "Проанализируйте логи веб-сервера и определите, содержат ли они признаки SQL Injection атаки.",
                    IsFinal = false,
                    ScoreIfSuccess = 10,
                    ScoreIfFail = 0
                },
                new ScenarioStep
                {
                    Id = 2,
                    ScenarioId = 1,
                    StepText = "Определите, какой тип SQL Injection был использован в атаке.",
                    IsFinal = false,
                    ScoreIfSuccess = 15,
                    ScoreIfFail = 5
                },
                new ScenarioStep
                {
                    Id = 3,
                    ScenarioId = 1,
                    StepText = "Предложите метод защиты от обнаруженной уязвимости.",
                    IsFinal = true,
                    ScoreIfSuccess = 20,
                    ScoreIfFail = 0
                }
            );

            // Sample Scenario Steps for XSS scenario
            modelBuilder.Entity<ScenarioStep>().HasData(
                new ScenarioStep
                {
                    Id = 4,
                    ScenarioId = 2,
                    StepText = "Найдите в коде веб-приложения места, уязвимые для XSS атак.",
                    IsFinal = false,
                    ScoreIfSuccess = 10,
                    ScoreIfFail = 0
                },
                new ScenarioStep
                {
                    Id = 5,
                    ScenarioId = 2,
                    StepText = "Определите тип XSS уязвимости (Stored, Reflected, DOM-based).",
                    IsFinal = false,
                    ScoreIfSuccess = 15,
                    ScoreIfFail = 5
                },
                new ScenarioStep
                {
                    Id = 6,
                    ScenarioId = 2,
                    StepText = "Исправьте найденные уязвимости, используя правильные методы защиты.",
                    IsFinal = true,
                    ScoreIfSuccess = 20,
                    ScoreIfFail = 5
                }
            );

            // Sample Scenario Steps for Phishing scenario
            modelBuilder.Entity<ScenarioStep>().HasData(
                new ScenarioStep
                {
                    Id = 7,
                    ScenarioId = 3,
                    StepText = "Проанализируйте предоставленное электронное письмо и определите, является ли оно фишинговым.",
                    IsFinal = false,
                    ScoreIfSuccess = 10,
                    ScoreIfFail = 0
                },
                new ScenarioStep
                {
                    Id = 8,
                    ScenarioId = 3,
                    StepText = "Определите, какие признаки указывают на то, что письмо является фишинговым.",
                    IsFinal = false,
                    ScoreIfSuccess = 15,
                    ScoreIfFail = 0
                },
                new ScenarioStep
                {
                    Id = 9,
                    ScenarioId = 3,
                    StepText = "Предложите рекомендации для пользователей по защите от фишинговых атак.",
                    IsFinal = true,
                    ScoreIfSuccess = 15,
                    ScoreIfFail = 0
                }
            );

            // Sample Scenario Steps for DDoS scenario
            modelBuilder.Entity<ScenarioStep>().HasData(
                new ScenarioStep
                {
                    Id = 10,
                    ScenarioId = 4,
                    StepText = "Проанализируйте сетевой трафик и определите признаки DDoS атаки.",
                    IsFinal = false,
                    ScoreIfSuccess = 15,
                    ScoreIfFail = 0
                },
                new ScenarioStep
                {
                    Id = 11,
                    ScenarioId = 4,
                    StepText = "Определите тип DDoS атаки (SYN flood, HTTP flood, Amplification и т.д.).",
                    IsFinal = false,
                    ScoreIfSuccess = 20,
                    ScoreIfFail = 5
                },
                new ScenarioStep
                {
                    Id = 12,
                    ScenarioId = 4,
                    StepText = "Разработайте стратегию защиты от обнаруженной DDoS атаки.",
                    IsFinal = true,
                    ScoreIfSuccess = 25,
                    ScoreIfFail = 5
                }
            );

            // Sample Scenario Step Options for SQL Injection steps
            modelBuilder.Entity<ScenarioStepOption>().HasData(
                // Options for Step 1
                new ScenarioStepOption
                {
                    Id = 1,
                    ScenarioStepId = 1,
                    OptionText = "Да, логи содержат признаки SQL Injection атаки",
                    IsCorrect = true
                },
                new ScenarioStepOption
                {
                    Id = 2,
                    ScenarioStepId = 1,
                    OptionText = "Нет, логи не содержат признаков SQL Injection атаки",
                    IsCorrect = false
                },
                // Options for Step 2
                new ScenarioStepOption
                {
                    Id = 3,
                    ScenarioStepId = 2,
                    OptionText = "Union-based SQL Injection",
                    IsCorrect = true
                },
                new ScenarioStepOption
                {
                    Id = 4,
                    ScenarioStepId = 2,
                    OptionText = "Error-based SQL Injection",
                    IsCorrect = false
                },
                new ScenarioStepOption
                {
                    Id = 5,
                    ScenarioStepId = 2,
                    OptionText = "Blind SQL Injection",
                    IsCorrect = false
                },
                // Options for Step 3
                new ScenarioStepOption
                {
                    Id = 6,
                    ScenarioStepId = 3,
                    OptionText = "Использовать параметризованные запросы",
                    IsCorrect = true
                },
                new ScenarioStepOption
                {
                    Id = 7,
                    ScenarioStepId = 3,
                    OptionText = "Отключить доступ к базе данных",
                    IsCorrect = false
                },
                new ScenarioStepOption
                {
                    Id = 8,
                    ScenarioStepId = 3,
                    OptionText = "Использовать простую фильтрацию символов",
                    IsCorrect = false
                }
            );

            // Sample Scenario Step Options for XSS steps
            modelBuilder.Entity<ScenarioStepOption>().HasData(
                // Options for Step 4
                new ScenarioStepOption
                {
                    Id = 9,
                    ScenarioStepId = 4,
                    OptionText = "В форме комментариев, где пользовательский ввод отображается без обработки",
                    IsCorrect = true
                },
                new ScenarioStepOption
                {
                    Id = 10,
                    ScenarioStepId = 4,
                    OptionText = "В системе аутентификации",
                    IsCorrect = false
                },
                // Options for Step 5
                new ScenarioStepOption
                {
                    Id = 11,
                    ScenarioStepId = 5,
                    OptionText = "Stored XSS",
                    IsCorrect = true
                },
                new ScenarioStepOption
                {
                    Id = 12,
                    ScenarioStepId = 5,
                    OptionText = "Reflected XSS",
                    IsCorrect = false
                },
                new ScenarioStepOption
                {
                    Id = 13,
                    ScenarioStepId = 5,
                    OptionText = "DOM-based XSS",
                    IsCorrect = false
                },
                // Options for Step 6
                new ScenarioStepOption
                {
                    Id = 14,
                    ScenarioStepId = 6,
                    OptionText = "Использовать HTML-экранирование и Content Security Policy",
                    IsCorrect = true
                },
                new ScenarioStepOption
                {
                    Id = 15,
                    ScenarioStepId = 6,
                    OptionText = "Отключить JavaScript в браузере",
                    IsCorrect = false
                },
                new ScenarioStepOption
                {
                    Id = 16,
                    ScenarioStepId = 6,
                    OptionText = "Запретить пользователям оставлять комментарии",
                    IsCorrect = false
                }
            );

            // Sample Scenario Step Options for Phishing steps
            modelBuilder.Entity<ScenarioStepOption>().HasData(
                // Options for Step 7
                new ScenarioStepOption
                {
                    Id = 17,
                    ScenarioStepId = 7,
                    OptionText = "Да, это фишинговое письмо",
                    IsCorrect = true
                },
                new ScenarioStepOption
                {
                    Id = 18,
                    ScenarioStepId = 7,
                    OptionText = "Нет, это легитимное письмо",
                    IsCorrect = false
                },
                // Options for Step 8
                new ScenarioStepOption
                {
                    Id = 19,
                    ScenarioStepId = 8,
                    OptionText = "Поддельный адрес отправителя и подозрительные ссылки",
                    IsCorrect = true
                },
                new ScenarioStepOption
                {
                    Id = 20,
                    ScenarioStepId = 8,
                    OptionText = "Наличие логотипа компании",
                    IsCorrect = false
                },
                // Options for Step 9
                new ScenarioStepOption
                {
                    Id = 21,
                    ScenarioStepId = 9,
                    OptionText = "Проверять URL перед переходом по ссылкам и не вводить личные данные на подозрительных сайтах",
                    IsCorrect = true
                },
                new ScenarioStepOption
                {
                    Id = 22,
                    ScenarioStepId = 9,
                    OptionText = "Никогда не открывать электронные письма",
                    IsCorrect = false
                }
            );

            // Sample Scenario Step Options for DDoS steps
            modelBuilder.Entity<ScenarioStepOption>().HasData(
                // Options for Step 10
                new ScenarioStepOption
                {
                    Id = 23,
                    ScenarioStepId = 10,
                    OptionText = "Аномально высокий объем трафика с множества IP-адресов",
                    IsCorrect = true
                },
                new ScenarioStepOption
                {
                    Id = 24,
                    ScenarioStepId = 10,
                    OptionText = "Нормальный объем трафика с обычным распределением",
                    IsCorrect = false
                },
                // Options for Step 11
                new ScenarioStepOption
                {
                    Id = 25,
                    ScenarioStepId = 11,
                    OptionText = "SYN flood атака",
                    IsCorrect = true
                },
                new ScenarioStepOption
                {
                    Id = 26,
                    ScenarioStepId = 11,
                    OptionText = "DNS amplification атака",
                    IsCorrect = false
                },
                new ScenarioStepOption
                {
                    Id = 27,
                    ScenarioStepId = 11,
                    OptionText = "HTTP flood атака",
                    IsCorrect = false
                },
                // Options for Step 12
                new ScenarioStepOption
                {
                    Id = 28,
                    ScenarioStepId = 12,
                    OptionText = "Использовать фильтрацию трафика и CDN с защитой от DDoS",
                    IsCorrect = true
                },
                new ScenarioStepOption
                {
                    Id = 29,
                    ScenarioStepId = 12,
                    OptionText = "Отключить сервер до окончания атаки",
                    IsCorrect = false
                },
                new ScenarioStepOption
                {
                    Id = 30,
                    ScenarioStepId = 12,
                    OptionText = "Увеличить количество серверов без дополнительной защиты",
                    IsCorrect = false
                }
            );
        }
    }
}
