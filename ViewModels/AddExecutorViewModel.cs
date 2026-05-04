using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.ValidationAttributes;

namespace ServiceMarketplace.ViewModels;

public class AddExecutorViewModel : BaseViewModel
{
    [Required(ErrorMessage = "Executor should have a name")]
    [Length(2, 32, ErrorMessage = "First name length must be in range from 2 to 32")]
    [RegularExpression(@"\p{L}+([ -]\p{L}+)*", ErrorMessage = "First name should contain only letters, spaces and '-' symbols")]
    public string FirstName
    {
        get;
        set { field = value; ValidateProperty(value); OnPropertyChanged(); }
    }

    [Required(ErrorMessage = "Executor should have a last name")]
    [Length(2, 32, ErrorMessage = "Last name length must be in range from 2 to 32")]
    [RegularExpression(@"\p{L}+([ -]\p{L}+)*", ErrorMessage = "Last name should contain only letters, spaces and '-' symbols")]
    public string LastName
    {
        get;
        set { field = value; ValidateProperty(value); OnPropertyChanged(); }
    }

    [Required(ErrorMessage = "Executor's age must be known")]
    [MinAge(18, ErrorMessage = "Executor must be at least 18 years old")]
    public DateTime DateOfBirth
    {
        get;
        set { field = value; ValidateProperty(value); OnPropertyChanged(); }
    }

    public Executor? CreatedExecutor { get; private set; }
    public Action<bool>? CloseAction { get; set; }
    public ICommand CreateCommand { get; }
    public ICommand CancelCommand { get; }

    public AddExecutorViewModel()
    {
        FirstName = "";
        LastName = "";
        DateOfBirth = DateTime.Now.AddYears(-30);

        CreateCommand = new RelayCommand(Create, _ => !HasErrors);
        CancelCommand = new RelayCommand(Cancel);
    }

    private void Create(object? obj)
    {
        CreatedExecutor = new Executor
        {
            FirstName = FirstName,
            LastName = LastName,
            Birthday = DateOnly.FromDateTime(DateOfBirth),
        };
        CloseAction?.Invoke(true);
    }

    private void Cancel(object? obj)
    {
        CloseAction?.Invoke(false);
    }
}
