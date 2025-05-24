using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CyberSecLabPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AttackTypes",
                columns: new[] { "Id", "Description", "Difficulty", "Name" },
                values: new object[,]
                {
                    { 1, "Атака, направленная на внедрение SQL-кода", "Средний", "SQL Injection" },
                    { 2, "Атака, направленная на внедрение вредоносного скрипта", "Средний", "Cross-Site Scripting (XSS)" },
                    { 3, "Социальная инженерия для получения конфиденциальной информации", "Легкий", "Phishing" },
                    { 4, "Распределенная атака отказа в обслуживании", "Сложный", "DDoS" }
                });

            migrationBuilder.InsertData(
                table: "Scenarios",
                columns: new[] { "Id", "AttackTypeId", "Complexity", "Description", "Instructions", "IsActive", "MaxScore", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Средний", "Сценарий для обучения обнаружению и предотвращению SQL Injection атак", "В этом сценарии вы будете анализировать логи веб-сервера для выявления попыток SQL Injection атак. Следуйте инструкциям на каждом шаге.", true, 100, "Обнаружение SQL Injection" },
                    { 2, 2, "Средний", "Сценарий для обучения методам защиты от Cross-Site Scripting атак", "Вам предстоит проанализировать код веб-приложения и найти уязвимости XSS. Затем вы должны предложить методы их устранения.", true, 80, "Защита от XSS атак" },
                    { 3, 3, "Легкий", "Сценарий для обучения распознаванию фишинговых писем и сайтов", "В этом сценарии вы будете анализировать различные электронные письма и определять, являются ли они фишинговыми. Обратите внимание на характерные признаки фишинга.", true, 60, "Распознавание фишинговых писем" },
                    { 4, 4, "Сложный", "Сценарий для обучения методам анализа и предотвращения DDoS атак", "Вы будете анализировать сетевой трафик во время DDoS атаки и разрабатывать стратегию защиты. Используйте предоставленные инструменты для анализа и фильтрации трафика.", true, 120, "Анализ и предотвращение DDoS атак" }
                });

            migrationBuilder.InsertData(
                table: "ScenarioSteps",
                columns: new[] { "Id", "IsFinal", "ScenarioId", "ScoreIfFail", "ScoreIfSuccess", "StepText" },
                values: new object[,]
                {
                    { 1, false, 1, 0, 10, "Проанализируйте логи веб-сервера и определите, содержат ли они признаки SQL Injection атаки." },
                    { 2, false, 1, 5, 15, "Определите, какой тип SQL Injection был использован в атаке." },
                    { 3, true, 1, 0, 20, "Предложите метод защиты от обнаруженной уязвимости." },
                    { 4, false, 2, 0, 10, "Найдите в коде веб-приложения места, уязвимые для XSS атак." },
                    { 5, false, 2, 5, 15, "Определите тип XSS уязвимости (Stored, Reflected, DOM-based)." },
                    { 6, true, 2, 5, 20, "Исправьте найденные уязвимости, используя правильные методы защиты." },
                    { 7, false, 3, 0, 10, "Проанализируйте предоставленное электронное письмо и определите, является ли оно фишинговым." },
                    { 8, false, 3, 0, 15, "Определите, какие признаки указывают на то, что письмо является фишинговым." },
                    { 9, true, 3, 0, 15, "Предложите рекомендации для пользователей по защите от фишинговых атак." },
                    { 10, false, 4, 0, 15, "Проанализируйте сетевой трафик и определите признаки DDoS атаки." },
                    { 11, false, 4, 5, 20, "Определите тип DDoS атаки (SYN flood, HTTP flood, Amplification и т.д.)." },
                    { 12, true, 4, 5, 25, "Разработайте стратегию защиты от обнаруженной DDoS атаки." }
                });

            migrationBuilder.InsertData(
                table: "ScenarioStepOptions",
                columns: new[] { "Id", "IsCorrect", "NextStepId", "OptionText", "ScenarioStepId" },
                values: new object[,]
                {
                    { 1, true, null, "Да, логи содержат признаки SQL Injection атаки", 1 },
                    { 2, false, null, "Нет, логи не содержат признаков SQL Injection атаки", 1 },
                    { 3, true, null, "Union-based SQL Injection", 2 },
                    { 4, false, null, "Error-based SQL Injection", 2 },
                    { 5, false, null, "Blind SQL Injection", 2 },
                    { 6, true, null, "Использовать параметризованные запросы", 3 },
                    { 7, false, null, "Отключить доступ к базе данных", 3 },
                    { 8, false, null, "Использовать простую фильтрацию символов", 3 },
                    { 9, true, null, "В форме комментариев, где пользовательский ввод отображается без обработки", 4 },
                    { 10, false, null, "В системе аутентификации", 4 },
                    { 11, true, null, "Stored XSS", 5 },
                    { 12, false, null, "Reflected XSS", 5 },
                    { 13, false, null, "DOM-based XSS", 5 },
                    { 14, true, null, "Использовать HTML-экранирование и Content Security Policy", 6 },
                    { 15, false, null, "Отключить JavaScript в браузере", 6 },
                    { 16, false, null, "Запретить пользователям оставлять комментарии", 6 },
                    { 17, true, null, "Да, это фишинговое письмо", 7 },
                    { 18, false, null, "Нет, это легитимное письмо", 7 },
                    { 19, true, null, "Поддельный адрес отправителя и подозрительные ссылки", 8 },
                    { 20, false, null, "Наличие логотипа компании", 8 },
                    { 21, true, null, "Проверять URL перед переходом по ссылкам и не вводить личные данные на подозрительных сайтах", 9 },
                    { 22, false, null, "Никогда не открывать электронные письма", 9 },
                    { 23, true, null, "Аномально высокий объем трафика с множества IP-адресов", 10 },
                    { 24, false, null, "Нормальный объем трафика с обычным распределением", 10 },
                    { 25, true, null, "SYN flood атака", 11 },
                    { 26, false, null, "DNS amplification атака", 11 },
                    { 27, false, null, "HTTP flood атака", 11 },
                    { 28, true, null, "Использовать фильтрацию трафика и CDN с защитой от DDoS", 12 },
                    { 29, false, null, "Отключить сервер до окончания атаки", 12 },
                    { 30, false, null, "Увеличить количество серверов без дополнительной защиты", 12 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "ScenarioStepOptions",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AttackTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AttackTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AttackTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AttackTypes",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
