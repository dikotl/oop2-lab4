using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ServiceMarketplace.ViewModels;

public partial class BaseViewModel : INotifyDataErrorInfo
{
    protected readonly Dictionary<string, List<string>> ValidationErrors = [];

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public bool HasErrors => ValidationErrors.Count != 0;

#pragma warning disable CS8603 // Possible null reference return.
    IEnumerable INotifyDataErrorInfo.GetErrors(string? propertyName)
    {
        if (string.IsNullOrEmpty(propertyName) || !ValidationErrors.TryGetValue(propertyName, out var value))
        {
            return null;
        }
        return value;
    }
#pragma warning restore CS8603 // Possible null reference return.

    protected void ValidateProperty(object? value, [CallerMemberName] string propertyName = "")
    {
        // Clear existing errors for this property.
        ValidationErrors.Remove(propertyName);

        // Ask the Data Annotations engine to validate the value.
        var validationContext = new ValidationContext(this) { MemberName = propertyName };
        var validationResults = new List<ValidationResult>();

        // If invalid, store the errors and notify the UI.
        if (!Validator.TryValidateProperty(value, validationContext, validationResults))
        {
            ValidationErrors[propertyName] = validationResults
                .Where(r => r.ErrorMessage is not null)
                .Select(r => r.ErrorMessage!)
                .ToList();
        }

        // Tell WPF the error state for this property has changed
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

        // Useful to tell the UI to re-evaluate Commands (e.g. disabling the Save button)
        OnPropertyChanged(nameof(HasErrors));
    }
}
