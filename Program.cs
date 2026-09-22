using System;
using System.Collections.Generic;
using FormatValidator.Models;
using FormatValidator.Validators;

namespace FormatValidator
{
    class Program
    {
        private static NumberValidator _numberValidator = new NumberValidator();
        private static DateValidator _dateValidator = new DateValidator();
        private static TimeValidator _timeValidator = new TimeValidator();

        static void Main(string[] args)
        {
            Console.Title = "FormatValidator - Модуль проверки форматов данных";
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            DisplayWelcomeScreen();

            bool exitRequested = false;

            while (!exitRequested)
            {
                DisplayMainMenu();
                var choice = GetMenuChoice(0, 9);

                switch (choice)
                {
                    case 1:
                        ValidateNumberMenu();
                        break;
                    case 2:
                        ValidateDateMenu();
                        break;
                    case 3:
                        ValidateTimeMenu();
                        break;
                    case 4:
                        ValidateDateTimeMenu();
                        break;
                    case 5:
                        BatchValidationMenu();
                        break;
                    case 6:
                        ShowSupportedFormats();
                        break;
                    case 7:
                        TestScenarios();
                        break;
                    case 8:
                        SettingsMenu();
                        break;
                    case 9:
                        AboutProgram();
                        break;
                    case 0:
                        exitRequested = true;
                        break;
                }

                if (!exitRequested && choice != 0)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }

            Console.WriteLine("\nСпасибо за использование программы!");
            Console.WriteLine("Программа завершена.");
        }

        static void DisplayWelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine("МОДУЛЬ ПРОВЕРКИ ФОРМАТОВ ДАННЫХ");
            Console.WriteLine();
            Console.WriteLine("Добро пожаловать в программу проверки форматов данных!");
            Console.WriteLine("Программа позволяет проверять корректность ввода:");
            Console.WriteLine("  • чисел (целые, десятичные, научная нотация)");
            Console.WriteLine("  • дат (различные форматы)");
            Console.WriteLine("  • времени (24-часовой, 12-часовой формат)");
            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("ГЛАВНОЕ МЕНЮ");
            Console.WriteLine();
            Console.WriteLine("1. Проверить число");
            Console.WriteLine("2. Проверить дату");
            Console.WriteLine("3. Проверить время");
            Console.WriteLine("4. Проверить дату и время");
            Console.WriteLine("5. Пакетная проверка");
            Console.WriteLine("6. Показать поддерживаемые форматы");
            Console.WriteLine("7. Тестовые сценарии");
            Console.WriteLine("8. Настройки");
            Console.WriteLine("9. О программе");
            Console.WriteLine("0. Выход");
            Console.WriteLine();
        }

        static void ValidateNumberMenu()
        {
            Console.Clear();
            Console.WriteLine("ПРОВЕРКА ЧИСЕЛ");
            Console.WriteLine();
            Console.WriteLine("Выберите тип числа:");
            Console.WriteLine("1. Целое число");
            Console.WriteLine("2. Десятичное число");
            Console.WriteLine("3. Вернуться в главное меню");
            Console.WriteLine();

            var choice = GetMenuChoice(1, 3);

            if (choice == 3) return;

            Console.Write("\nВведите число для проверки: ");
            string input = Console.ReadLine();

            ValidationResult result;

            switch (choice)
            {
                case 1:
                    result = _numberValidator.ValidateInteger(input);
                    break;
                case 2:
                    result = _numberValidator.ValidateDecimal(input);
                    break;
                default:
                    return;
            }

            DisplayValidationResult(result);
        }

        static void ValidateDateMenu()
        {
            Console.Clear();
            Console.WriteLine("ПРОВЕРКА ДАТ");
            Console.WriteLine();
            Console.WriteLine("Выберите формат даты:");
            Console.WriteLine("1. Автоматическое определение");
            Console.WriteLine("2. dd.MM.yyyy (31.12.2023)");
            Console.WriteLine("3. yyyy-MM-dd (2023-12-31)");
            Console.WriteLine("4. MM/dd/yyyy (12/31/2023)");
            Console.WriteLine("5. Вернуться в главное меню");
            Console.WriteLine();

            var choice = GetMenuChoice(1, 5);

            if (choice == 5) return;

            string format = choice switch
            {
                1 => null,
                2 => "dd.MM.yyyy",
                3 => "yyyy-MM-dd",
                4 => "MM/dd/yyyy",
                _ => null
            };

            string formatName = format ?? "автоматическое определение";
            Console.Write($"\nВведите дату (формат: {formatName}): ");
            string input = Console.ReadLine();

            var result = _dateValidator.ValidateDate(input, format);
            DisplayValidationResult(result);
        }

        static void ValidateTimeMenu()
        {
            Console.Clear();
            Console.WriteLine("ПРОВЕРКА ВРЕМЕНИ");
            Console.WriteLine();
            Console.WriteLine("Выберите тип проверки:");
            Console.WriteLine("1. Время (HH:mm:ss)");
            Console.WriteLine("2. Временной интервал");
            Console.WriteLine("3. Вернуться в главное меню");
            Console.WriteLine();

            var choice = GetMenuChoice(1, 3);

            if (choice == 3) return;

            Console.Write("\nВведите значение: ");
            string input = Console.ReadLine();

            ValidationResult result;

            if (choice == 1)
            {
                result = _timeValidator.ValidateTime(input);
            }
            else
            {
                result = _timeValidator.ValidateTimeSpan(input);
            }

            DisplayValidationResult(result);
        }

        static void ValidateDateTimeMenu()
        {
            Console.Clear();
            Console.WriteLine("ПРОВЕРКА ДАТЫ И ВРЕМЕНИ");
            Console.WriteLine();
            Console.WriteLine("Введите дату и время (например: 31.12.2023 23:59:59): ");
            string input = Console.ReadLine();

            // Разделяем дату и время
            var parts = input.Split(' ');
            if (parts.Length < 2)
            {
                Console.WriteLine("\n[✗] Ошибка: введите и дату, и время через пробел");
                return;
            }

            string datePart = parts[0];
            string timePart = parts[1];

            Console.WriteLine("\nРезультаты проверки:");
            Console.WriteLine("");

            var dateResult = _dateValidator.ValidateDate(datePart);
            var timeResult = _timeValidator.ValidateTime(timePart);

            Console.WriteLine($"Дата: {(dateResult.IsValid ? "[]" : "[]")} {dateResult.Message}");
            Console.WriteLine($"Время: {(timeResult.IsValid ? "[]" : "[]")} {timeResult.Message}");

            if (dateResult.IsValid && timeResult.IsValid)
            {
                Console.WriteLine("\n[] Полная дата и время корректны!");
                if (dateResult.ParsedValue is DateTime date && timeResult.ParsedValue is DateTime time)
                {
                    DateTime combined = new DateTime(date.Year, date.Month, date.Day,
                                                     time.Hour, time.Minute, time.Second);
                    Console.WriteLine($"Объединенное значение: {combined:dd.MM.yyyy HH:mm:ss}");
                }
            }
        }

        static void BatchValidationMenu()
        {
            Console.Clear();
            Console.WriteLine("ПАКЕТНАЯ ПРОВЕРКА");
            Console.WriteLine();
            Console.WriteLine("Введите несколько значений через точку с запятой:");
            Console.WriteLine("Пример: 123; 31.12.2023; 23:59:59; 1.23e+10");
            Console.Write("\nВвод: ");

            string input = Console.ReadLine();
            var values = input.Split(';', StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine("РЕЗУЛЬТАТЫ ПРОВЕРКИ");

            int total = values.Length;
            int validCount = 0;

            foreach (var value in values)
            {
                string trimmedValue = value.Trim();
                Console.WriteLine($"\nПроверка: '{trimmedValue}'");

                // Автоматическое определение типа
                var intResult = _numberValidator.ValidateInteger(trimmedValue);
                if (intResult.IsValid)
                {
                    Console.WriteLine($"  Тип: Целое число - []");
                    validCount++;
                    continue;
                }

                var decimalResult = _numberValidator.ValidateDecimal(trimmedValue);
                if (decimalResult.IsValid)
                {
                    Console.WriteLine($"  Тип: Десятичное число - []");
                    validCount++;
                    continue;
                }

                var dateResult = _dateValidator.ValidateDate(trimmedValue);
                if (dateResult.IsValid)
                {
                    Console.WriteLine($"  Тип: Дата - []");
                    validCount++;
                    continue;
                }

                var timeResult = _timeValidator.ValidateTime(trimmedValue);
                if (timeResult.IsValid)
                {
                    Console.WriteLine($"  Тип: Время - []");
                    validCount++;
                    continue;
                }

                Console.WriteLine($"  Тип: Неизвестный формат - []");
            }

            Console.WriteLine("\n");
            Console.WriteLine($"СТАТИСТИКА: {validCount}/{total} значений корректны");
            if (validCount == total)
                Console.WriteLine("Все значения корректны! []");
            else if (validCount == 0)
                Console.WriteLine("Нет корректных значений []");
            else
                Console.WriteLine($"Успешно: {validCount}, Ошибок: {total - validCount}");
        }

        static void ShowSupportedFormats()
        {
            Console.Clear();
            Console.WriteLine("ПОДДЕРЖИВАЕМЫЕ ФОРМАТЫ");
            Console.WriteLine();

            Console.WriteLine("ЧИСЛА:");
            Console.WriteLine("");
            foreach (var format in _numberValidator.GetSupportedFormats())
            {
                Console.WriteLine($"• {format.Name}:");
                Console.WriteLine($"  Описание: {format.Description}");
                Console.WriteLine($"  Пример: {format.Example}");
                Console.WriteLine($"  Паттерн: {format.Pattern}");
                Console.WriteLine();
            }

            Console.WriteLine("\nДАТЫ:");
            Console.WriteLine("");
            foreach (var format in _dateValidator.GetSupportedFormats())
            {
                Console.WriteLine($"• {format.Name}:");
                Console.WriteLine($"  Описание: {format.Description}");
                Console.WriteLine($"  Пример: {format.Example}");
                Console.WriteLine($"  Паттерн: {format.Pattern}");
                Console.WriteLine();
            }

            Console.WriteLine("\nВРЕМЯ:");
            Console.WriteLine("");
            foreach (var format in _timeValidator.GetSupportedFormats())
            {
                Console.WriteLine($"• {format.Name}:");
                Console.WriteLine($"  Описание: {format.Description}");
                Console.WriteLine($"  Пример: {format.Example}");
                Console.WriteLine($"  Паттерн: {format.Pattern}");
                Console.WriteLine();
            }
        }

        static void TestScenarios()
        {
            Console.Clear();
            Console.WriteLine("ТЕСТОВЫЕ СЦЕНАРИИ");
            Console.WriteLine();

            Console.WriteLine("Тестовые данные загружаются...");

            // Тестовые данные
            var testCases = new List<(string type, string value, bool expected)>
            {
                ("Число", "123", true),
                ("Число", "abc", false),
                ("Число", "12.34", true),
                ("Число", "1.23e+10", true),
                ("Дата", "31.12.2023", true),
                ("Дата", "29.02.2023", false), // Невисокосный год
                ("Дата", "29.02.2024", true),  // Високосный год
                ("Время", "23:59:59", true),
                ("Время", "25:00:00", false),
                ("Время", "11:59:59 PM", true)
            };

            int passed = 0;
            int failed = 0;

            Console.WriteLine("\nРезультаты тестирования:");
            Console.WriteLine("");

            foreach (var test in testCases)
            {
                ValidationResult result;

                switch (test.type)
                {
                    case "Число":
                        result = _numberValidator.ValidateInteger(test.value);
                        if (!result.IsValid)
                            result = _numberValidator.ValidateDecimal(test.value);
                        break;
                    case "Дата":
                        result = _dateValidator.ValidateDate(test.value);
                        break;
                    case "Время":
                        result = _timeValidator.ValidateTime(test.value);
                        break;
                    default:
                        continue;
                }

                bool success = result.IsValid == test.expected;

                if (success)
                {
                    Console.WriteLine($"[] {test.type}: '{test.value}' - пройден");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[] {test.type}: '{test.value}' - не пройден");
                    Console.WriteLine($"    Ожидалось: {(test.expected ? "валидно" : "невалидно")}");
                    Console.WriteLine($"    Получено: {(result.IsValid ? "валидно" : "невалидно")}");
                    failed++;
                }
            }

            Console.WriteLine("\n");
            Console.WriteLine($"ИТОГО: Пройдено {passed}/{testCases.Count} тестов");
            if (failed == 0)
                Console.WriteLine("Все тесты пройдены успешно! []");
            else
                Console.WriteLine($"Ошибок: {failed}");
        }

        static void SettingsMenu()
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("НАСТРОЙКИ");
            Console.WriteLine("");
            Console.WriteLine();
            Console.WriteLine("Настройки программы:");
            Console.WriteLine("1. Язык/локаль (текущая: " + System.Globalization.CultureInfo.CurrentCulture.Name + ")");
            Console.WriteLine("2. Цветовая схема");
            Console.WriteLine("3. Сохранение истории проверок");
            Console.WriteLine("4. Сброс настроек");
            Console.WriteLine("5. Вернуться в главное меню");
            Console.WriteLine();

            var choice = GetMenuChoice(1, 5);

            if (choice == 5) return;

            Console.WriteLine("\nЭта функция находится в разработке.");
            Console.WriteLine("В будущих версиях будут доступны настройки.");
        }

        static void AboutProgram()
        {
            Console.Clear();
            Console.WriteLine("О ПРОГРАММЕ");
            Console.WriteLine();
            Console.WriteLine("FormatValidator v1.0");
            Console.WriteLine("Модуль проверки форматов данных");
            Console.WriteLine();
            Console.WriteLine("Разработано в рамках учебной практики");
            Console.WriteLine("по программированию на C#");
            Console.WriteLine();
            Console.WriteLine("Функциональные возможности:");
            Console.WriteLine("• Валидация чисел, дат, времени");
            Console.WriteLine("• Поддержка различных форматов");
            Console.WriteLine("• Пакетная обработка данных");
            Console.WriteLine("• Расширяемая архитектура");
            Console.WriteLine();
            Console.WriteLine("Используемые технологии:");
            Console.WriteLine("• C# 10.0, .NET 6.0");
            Console.WriteLine("• ООП, регулярные выражения");
            Console.WriteLine("• Модульное тестирование");
            Console.WriteLine();
            Console.WriteLine("2026 Учебный проект");
        }

        static void DisplayValidationResult(ValidationResult result)
        {
            Console.WriteLine();
            Console.WriteLine("");
            Console.WriteLine("РЕЗУЛЬТАТ ПРОВЕРКИ");
            Console.WriteLine("");
            Console.WriteLine();

            Console.ForegroundColor = result.IsValid ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(result.ToString());
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("Детали:");
            Console.WriteLine($"Тип формата: {result.FormatType}");
            Console.WriteLine($"Входные данные: '{result.Input}'");

            if (result.ParsedValue != null)
            {
                Console.WriteLine($"Распознанное значение: {result.ParsedValue}");
            }

            if (result.AdditionalInfo.Count > 0)
            {
                Console.WriteLine("\nДополнительная информация:");
                foreach (var info in result.AdditionalInfo)
                {
                    Console.WriteLine($"  • {info}");
                }
            }

            if (result.Errors.Count > 0)
            {
                Console.WriteLine("\nОшибки:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"  • {error}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("");
        }

        static int GetMenuChoice(int min, int max)
        {
            while (true)
            {
                Console.Write($"Выберите пункт [{min}-{max}]: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int choice) && choice >= min && choice <= max)
                {
                    return choice;
                }

                Console.WriteLine($"Ошибка: введите число от {min} до {max}");
                Console.Beep();
            }
        }
    }
}