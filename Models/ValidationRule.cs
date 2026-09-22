namespace FormatValidator.Models
{
    /// <summary>
    /// Правило валидации
    /// </summary>
    public class ValidationRule
    {
        public string Name { get; set; }
        public string Pattern { get; set; }
        public string Description { get; set; }
        public string Example { get; set; }
    }
}