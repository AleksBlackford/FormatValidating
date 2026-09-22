using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using FormatValidator.Models;

namespace FormatValidator.Validators
{
    public class NumberValidator
    {
        private readonly CultureInfo _culture;

        public NumberValidator(CultureInfo culture = null)
        {
            _culture = culture ?? CultureInfo.InvariantCulture;
        }

        public ValidationResult ValidateInteger(string input)
        {
            var result = new ValidationResult
            {
                Input = input,
                FormatType = "Целое число"
            };

            if (string.IsNullOrWhiteSpace(input))
            {
                result.IsValid = false;
                result.Message = "Входная строка пуста";
                result.Errors.Add("Входные данные не могут быть пустыми");
                return result;
            }

            // Проверка через TryParse
            if (int.TryParse(input, NumberStyles.Integer, _culture, out int intValue))
            {
                result.IsValid = true;
                result.Message = $"Целое число распознано: {intValue}";
                result.ParsedValue = intValue;

                // Дополнительная информация
                result.AdditionalInfo.Add($"Двоичное представление: {Convert.ToString(intValue, 2)}");
                result.AdditionalInfo.Add($"Шестнадцатеричное: 0x{intValue:X}");
                result.AdditionalInfo.Add($"Абсолютное значение: {Math.Abs(intValue)}");
            }
            else if (long.TryParse(input, NumberStyles.Integer, _culture, out long longValue))
            {
                result.IsValid = true;
                result.Message = $"Длинное целое число: {longValue}";
                result.ParsedValue = longValue;
                result.AdditionalInfo.Add("Число выходит за пределы int, используйте long");
            }
            else
            {
                result.IsValid = false;
                result.Message = "Неверный формат целого числа";
                result.Errors.Add("Строка не соответствует формату целого числа");
                result.Errors.Add("Допустимые символы: цифры 0-9, знак минус в начале");
                result.Errors.Add("Примеры: 123, -456, 0");
            }

            return result;
        }

        public ValidationResult ValidateDecimal(string input)
        {
            var result = new ValidationResult
            {
                Input = input,
                FormatType = "Десятичное число"
            };

            // Удаляем разделители тысяч для упрощения проверки
            string normalizedInput = input.Replace(_culture.NumberFormat.NumberGroupSeparator, "");

            // Паттерн для десятичных чисел
            string decimalPattern = @"^-?\d+(?:[.,]\d+)?(?:[eE][+-]?\d+)?$";

            if (Regex.IsMatch(normalizedInput, decimalPattern))
            {
                if (decimal.TryParse(normalizedInput, NumberStyles.Any, _culture, out decimal decimalValue))
                {
                    result.IsValid = true;
                    result.Message = $"Десятичное число: {decimalValue}";
                    result.ParsedValue = decimalValue;

                    // Дополнительная информация
                    result.AdditionalInfo.Add($"Научная нотация: {decimalValue:E2}");
                    result.AdditionalInfo.Add($"Процент: {decimalValue:P2}");
                    result.AdditionalInfo.Add($"Валюта: {decimalValue:C}");
                }
                else
                {
                    result.IsValid = false;
                    result.Message = "Число вне допустимого диапазона";
                    result.Errors.Add("Значение слишком велико или слишком мало");
                }
            }
            else
            {
                result.IsValid = false;
                result.Message = "Неверный формат десятичного числа";
                result.Errors.Add("Допустимые форматы: 123.45, -78.9, 1.23e+10");
                result.Errors.Add($"Разделитель дробной части: '{_culture.NumberFormat.NumberDecimalSeparator}'");
            }

            return result;
        }

        public List<ValidationRule> GetSupportedFormats()
        {
            return new List<ValidationRule>
            {
                new ValidationRule
                {
                    Name = "Целое число",
                    Pattern = @"^-?\d+$",
                    Description = "Целое число со знаком или без",
                    Example = "123, -456, 0"
                },
                new ValidationRule
                {
                    Name = "Десятичное число",
                    Pattern = @"^-?\d+(?:[.,]\d+)?$",
                    Description = "Число с плавающей точкой",
                    Example = "123.45, -78.9"
                },
                new ValidationRule
                {
                    Name = "Научная нотация",
                    Pattern = @"^-?\d+(?:[.,]\d+)?[eE][+-]?\d+$",
                    Description = "Число в научной нотации",
                    Example = "1.23e+10, -4.56E-3"
                }
            };
        }
    }
}