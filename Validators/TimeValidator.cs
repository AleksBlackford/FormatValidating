using System;
using System.Collections.Generic;
using System.Globalization;
using FormatValidator.Models;

namespace FormatValidator.Validators
{
    public class TimeValidator
    {
        private readonly CultureInfo _culture;

        public TimeValidator(CultureInfo culture = null)
        {
            _culture = culture ?? CultureInfo.CurrentCulture;
        }

        public ValidationResult ValidateTime(string input)
        {
            var result = new ValidationResult
            {
                Input = input,
                FormatType = "Время"
            };

            if (string.IsNullOrWhiteSpace(input))
            {
                result.IsValid = false;
                result.Message = "Входная строка пуста";
                result.Errors.Add("Введите время для проверки");
                return result;
            }

            string[] timeFormats = {
                "HH:mm:ss", "H:m:s",
                "HH:mm", "H:m",
                "hh:mm:ss tt", "h:m:s tt",
                "hh:mm tt", "h:m tt"
            };

            bool isValid = DateTime.TryParseExact(input, timeFormats, _culture,
                DateTimeStyles.NoCurrentDateDefault, out DateTime parsedTime);

            if (isValid)
            {
                result.IsValid = true;
                result.Message = $"Время распознано: {parsedTime:HH:mm:ss}";
                result.ParsedValue = parsedTime;

                // Дополнительная информация
                result.AdditionalInfo.Add($"12-часовой формат: {parsedTime:hh:mm:ss tt}");
                result.AdditionalInfo.Add($"Общее количество секунд: {parsedTime.Hour * 3600 + parsedTime.Minute * 60 + parsedTime.Second}");

                // Проверка на особое время
                if (parsedTime.Hour == 0 && parsedTime.Minute == 0 && parsedTime.Second == 0)
                {
                    result.AdditionalInfo.Add("Особенность: Полночь (00:00:00)");
                }
                else if (parsedTime.Hour == 12 && parsedTime.Minute == 0 && parsedTime.Second == 0)
                {
                    result.AdditionalInfo.Add("Особенность: Полдень (12:00:00)");
                }
            }
            else
            {
                result.IsValid = false;
                result.Message = "Неверный формат времени";
                result.Errors.Add("Поддерживаемые форматы:");
                result.Errors.Add("  - 24-часовой: HH:mm:ss (23:59:59)");
                result.Errors.Add("  - 12-часовой: hh:mm:ss tt (11:59:59 PM)");
                result.Errors.Add("Проверьте диапазоны: часы 0-23, минуты 0-59, секунды 0-59");
            }

            return result;
        }

        public ValidationResult ValidateTimeSpan(string input)
        {
            var result = new ValidationResult
            {
                Input = input,
                FormatType = "Временной интервал"
            };

            if (string.IsNullOrWhiteSpace(input))
            {
                result.IsValid = false;
                result.Message = "Входная строка пуста";
                result.Errors.Add("Введите временной интервал для проверки");
                return result;
            }

            if (TimeSpan.TryParse(input, out TimeSpan timeSpan))
            {
                result.IsValid = true;
                result.Message = $"Интервал: {timeSpan}";
                result.ParsedValue = timeSpan;
                result.AdditionalInfo.Add($"Всего дней: {timeSpan.TotalDays:F2}");
                result.AdditionalInfo.Add($"Всего часов: {timeSpan.TotalHours:F2}");
                result.AdditionalInfo.Add($"Всего минут: {timeSpan.TotalMinutes:F2}");
            }
            else
            {
                result.IsValid = false;
                result.Message = "Неверный формат временного интервала";
                result.Errors.Add("Формат: [д.]чч:мм:сс[.дробная_часть]");
                result.Errors.Add("Пример: 1.12:30:45 (1 день, 12 часов, 30 минут, 45 секунд)");
            }

            return result;
        }

        public List<ValidationRule> GetSupportedFormats()
        {
            return new List<ValidationRule>
            {
                new ValidationRule
                {
                    Name = "HH:mm:ss",
                    Pattern = @"^([01]?\d|2[0-3]):([0-5]?\d):([0-5]?\d)$",
                    Description = "24-часовой формат времени",
                    Example = "23:59:59"
                },
                new ValidationRule
                {
                    Name = "hh:mm:ss tt",
                    Pattern = @"^(0?[1-9]|1[0-2]):([0-5]?\d):([0-5]?\d) (AM|PM)$",
                    Description = "12-часовой формат времени",
                    Example = "11:59:59 PM"
                },
                new ValidationRule
                {
                    Name = "TimeSpan",
                    Pattern = @"^(\d+\.)?\d{1,2}:\d{2}(:\d{2}(\.\d+)?)?$",
                    Description = "Временной интервал",
                    Example = "1.12:30:45.500"
                }
            };
        }
    }
}