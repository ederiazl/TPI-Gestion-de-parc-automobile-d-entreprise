using System.ComponentModel.DataAnnotations;

namespace GestionParcAuto.Classes
{
    public class AllowExtensionsAttribute : ValidationAttribute
    {
        private string _extensions = "png";

        public AllowExtensionsAttribute(string? extensions)
        {
            this._extensions = extensions ?? this._extensions;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            var extension = Path.GetExtension(file.FileName).Trim('.');
            if (file != null)
            {
                if (!_extensions.Split(',').Contains(extension.ToLower()))
                {
                    return new ValidationResult(this.ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
