using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SmartResumeAnalyzer.Validators
{
    public class AllowedFileExtensionsAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string[] _extensions;

        public AllowedFileExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
            ErrorMessage = $"Invalid file type. Allowed extensions are: {string.Join(", ", _extensions)}";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!_extensions.Contains(extension))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
        public void AddValidation(ClientModelValidationContext context)
        {
            var extensions = string.Join(",", _extensions);
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-fileextension", ErrorMessage);
            context.Attributes.Add("data-val-fileextension-extensions", extensions);
        }
    }
}
