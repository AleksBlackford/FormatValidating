using System;
using System.Collections.Generic;

namespace FormatValidator.Models
{
    /// <summary>
    /// Результат валидации
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string Input { get; set; }
        public string FormatType { get; set; }
        public string Message { get; set; }
        public object ParsedValue { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> AdditionalInfo { get; set; } = new List<string>();

        public override string ToString()
        {
            return IsValid
                ? $"[] {Input} - {Message}"
                : $"[] {Input} - {Message}";
        }
    }
}