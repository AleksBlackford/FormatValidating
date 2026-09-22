using System;
using System.Collections.Generic;
using System.Globalization;
using FormatValidator.Models;

namespace FormatValidator.Validators
{
    public class DateValidator
    {
        private readonly CultureInfo _culture;

        public DateValidator(CultureInfo culture = null)
        {
            _culture = culture ?? CultureInfo.CurrentCulture;
        }

        public ValidationResult ValidateDate(string input, string format = null)
        {
            var result = new ValidationResult
            {
                Input = input,
                FormatType = "Дата"
            };

            if (string.IsNullOrWhiteSpace(input))
            {
                result.IsValid = false;
                result.Message = "Входная строка пуста";
                result.Errors.Add("Введите дату для проверки");
                return result;
            }

            DateTime parsedDate;
            bool isValid = false;

            if (!string.IsNullOrEmpty(format))
            {
                // Проверка по конкретному формату
                isValid = DateTime.TryParseExact(input, format, _culture,
                    DateTimeStyles.None, out parsedDate);
            }
            else
            {
                // Автоматическое определение формата
                string[] formats = {
                    "dd.MM.yyyy", "d.M.yyyy",
                    "dd-MM-yyyy", "d-M-yyyy",
                    "dd/MM/yyyy", "d/M/yyyy",
                    "yyyy-MM-dd", "yyyy-M-d",
                    "MM/dd/yyyy", "M/d/yyyy"
                };

                isValid = DateTime.TryParseExact(input, formats, _culture,
                    DateTimeStyles.None, out parsedDate);
            }

            if (isValid)
            {
                result.IsValid = true;
                result.Message = $"Дата распознана: {parsedDate:dd.MM.yyyy}";
                result.ParsedValue = parsedDate;

                // Дополнительная информация
                result.AdditionalInfo.Add($"День недели: {_culture.DateTimeFormat.GetDayName(parsedDate.DayOfWeek)}");
                result.AdditionalInfo.Add($"День года: {parsedDate.DayOfYear}");
                result.AdditionalInfo.Add($"Квартал: {(parsedDate.Month - 1) / 3 + 1}");

                // Проверка на особые даты
                if (parsedDate.Day == 29 && parsedDate.Month == 2)
                {
                    result.AdditionalInfo.Add("Особенность: 29 февраля (високосный год)");
                }
            }
            else
            {
                result.IsValid = false;
                result.Message = "Неверный формат даты";
                result.Errors.Add("Поддерживаемые форматы:");
                result.Errors.Add("  - dd.MM.yyyy (31.12.2023)");
                result.Errors.Add("  - yyyy-MM-dd (2023-12-31)");
                result.Errors.Add("  - dd/MM/yyyy (31/12/2023)");
                result.Errors.Add("Проверьте корректность чисел (день 1-31, месяц 1-12)");
            }

            return result;
        }

        public bool IsLeapYear(int year)
        {
            return DateTime.IsLeapYear(year);
        }

        public int GetDaysInMonth(int year, int month)
        {
            return DateTime.DaysInMonth(year, month);
        }

        public List<ValidationRule> GetSupportedFormats()
        {
            return new List<ValidationRule>
            {
                new ValidationRule
                {
                    Name = "dd.MM.yyyy",
                    Pattern = @"^\d{1,2}\.\d{1,2}\.\d{4}$",
                    Description = "Европейский формат даты",
                    Example = "31.12.2023"
                },
                new ValidationRule
                {
                    Name = "yyyy-MM-dd",
                    Pattern = @"^\d{4}-\d{1,2}-\d{1,2}$",
                    Description = "Международный формат даты (ISO)",
                    Example = "2023-12-31"
                },
                new ValidationRule
                {
                    Name = "MM/dd/yyyy",
                    Pattern = @"^\d{1,2}/\d{1,2}/\d{4}$",
                    Description = "Американский формат даты",
                    Example = "12/31/2023"
                }
            };
        }
    }
}