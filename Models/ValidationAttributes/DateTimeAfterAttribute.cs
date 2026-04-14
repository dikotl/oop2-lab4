using System.ComponentModel.DataAnnotations;

namespace ServiceMarketplace.Models.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class DateTimeAfterAttribute(string earliestValid)
    : ValidationAttribute($"Date must be after {earliestValid} and before today's date ({DateTime.Now})")
{
    private readonly DateTime _earliestValid = DateTime.Parse(earliestValid);

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Fix for [Required] attribute.
        if (value is not null)
        {
            if (value is not DateTime date)
            {
                return new ValidationResult("The value must be a valid DateTime");
            }
            if (date < _earliestValid || date > DateTime.Now)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
        }

        return ValidationResult.Success;
    }
}
