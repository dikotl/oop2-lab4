using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using ServiceMarketplace.Models;

namespace ServiceMarketplace.ViewModels;

public class AddBureauViewModel : BaseViewModel
{
    private readonly IList<ServiceBureau> ExistingBureaus;

    [Required(ErrorMessage = "Bureau name is required")]
    [MinLength(2, ErrorMessage = "Bureau name must be at least 2 characters")]
    public string BureauName
    {
        get => field;
        set { field = value; ValidateProperty(value); CheckUnique(); OnPropertyChanged(); }
    }

    public ServiceBureau? CreatedBureau { get; private set; }
    public Action<bool>? CloseAction { get; set; }
    public ICommand CreateCommand { get; }
    public ICommand CancelCommand { get; }

    public AddBureauViewModel(IList<ServiceBureau> existingBureaus)
    {
        ExistingBureaus = existingBureaus;
        BureauName = "Unnamed";
        CreateCommand = new Command(Create, _ => !HasErrors);
        CancelCommand = new Command(Cancel);
    }

    private void Create(object? _)
    {
        CreatedBureau = new ServiceBureau(BureauName.Trim())
        {
            Orders = new ObservableCollection<Order>(),
            Staff = new ObservableCollection<Executor>(),
        };
        CloseAction?.Invoke(true);
    }

    private void Cancel(object? _)
    {
        CloseAction?.Invoke(false);
    }

    // Must be a validation class but who cares.
    private void CheckUnique([CallerMemberName] string property = "")
    {
        string bureauName = BureauName.Trim();

        if (ExistingBureaus.Any(bureau => bureau.Name == bureauName))
        {
            var error = $"Bureau named '{bureauName}' already exist";

            if (ValidationErrors.TryGetValue(property, out var errors))
            {
                errors.Add(error);
            }
            else
            {
                ValidationErrors[property] = [error];
            }
        }
    }
}
