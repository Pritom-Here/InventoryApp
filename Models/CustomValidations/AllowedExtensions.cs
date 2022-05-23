using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Models.CustomValidations
{
    public class AllowedExtensions : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensions(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var fileList = value as IList<IFormFile>;

            foreach (var file in fileList)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();

                if (!_extensions.Contains(extension.ToLower()) )
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            var extensionsAsString = String.Join(", ", _extensions);

            return $"File extension is invalid! Allowed extensions are " + extensionsAsString;
        }
    }
}
