using ServiceMarketplace.Models;

namespace ServiceMarketplace.ViewModels;

public class EditExecutorViewModel : BaseViewModel
{
    private readonly Executor _originalExecutor;

    public EditExecutorData NewExecutor
    {
        get;
        set { field = value; OnPropertyChanged(); }
    }

    public Action<bool>? CloseAction { get; set; }
    public RelayCommand SaveCommand { get; }
    public RelayCommand CancelCommand { get; }

    public EditExecutorViewModel(Executor originalExecutor)
    {
        _originalExecutor = originalExecutor;
        NewExecutor = new()
        {
            FirstName = originalExecutor.FirstName,
            LastName = originalExecutor.LastName,
        };

        SaveCommand = new RelayCommand(Save, _ => !NewExecutor.HasErrors);
        CancelCommand = new RelayCommand(Cancel);
    }

    private void Save(object? _)
    {
        _originalExecutor.FirstName = NewExecutor.FirstName!;
        _originalExecutor.LastName = NewExecutor.LastName;
        CloseAction?.Invoke(true);
    }

    private void Cancel(object? _)
    {
        CloseAction?.Invoke(false);
    }
}
