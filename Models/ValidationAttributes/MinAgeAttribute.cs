using System.ComponentModel.DataAnnotations;

namespace ServiceMarketplace.Models.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class MinAgeAttribute(int age) : ValidationAttribute($"You must be at least {age} years old")
{
    private readonly int _minAge = age;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Fix for [Required] attribute.
        if (value is not null)
        {
            DateOnly? birthDate = value switch
            {
                DateOnly date => date,
                DateTime date => DateOnly.FromDateTime(date),
                _ => null,
            };

            if (birthDate is null)
            {
                return new ValidationResult("The value must be a DateOnly or DateTime instance");
            }

            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - birthDate.Value.Year;

            // If today's date minus the years lived is still earlier than their birth date,
            // they haven't celebrated their birthday yet this year.
            if (birthDate.Value > today.AddYears(-age))
            {
                --age;
            }

            if (age < _minAge)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
        }

        return ValidationResult.Success;
    }
}
