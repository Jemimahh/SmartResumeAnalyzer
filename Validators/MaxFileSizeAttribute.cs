using System.ComponentModel.DataAnnotations;

namespace SmartResumeAnalyzer.Validators
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxBytes;

        public MaxFileSizeAttribute(int maxMB)
        {
            _maxBytes = maxMB * 1024 * 1024;
            ErrorMessage = $"File size cannot exceed {maxMB} MB.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file && file.Length > _maxBytes)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
