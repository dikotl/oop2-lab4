using ServiceMarketplace.Models;

namespace ServiceMarketplace.ViewModels;

public class EditOrderViewModel : BaseViewModel
{
    private readonly Order _originalOrder;

    public static ServiceKind[] AvailableServices => Enum.GetValues<ServiceKind>();
    public IList<Executor> AvailableExecutors { get; }
    public IList<string> AvailableAddresses { get; }
    public EditOrderData NewOrder { get; }

    public Action<bool>? CloseAction { get; set; }
    public RelayCommand SaveCommand { get; }
    public RelayCommand CancelCommand { get; }

    public EditOrderViewModel(Order originalOrder, IList<Executor> executors, IList<string> addresses)
    {
        _originalOrder = originalOrder;
        AvailableExecutors = executors;
        AvailableAddresses = addresses;
        NewOrder = new()
        {
            Executor = originalOrder.Executor,
            Service = originalOrder.Service,
            CustomerAddress = originalOrder.Address,
            Cost = originalOrder.Cost
        };

        SaveCommand = new RelayCommand(Save, _ => !NewOrder.HasErrors);
        CancelCommand = new RelayCommand(Cancel);
    }

    private void Save(object? _)
    {
        _originalOrder.Executor = NewOrder.Executor!;
        _originalOrder.Service = NewOrder.Service;
        _originalOrder.Address = NewOrder.CustomerAddress;
        _originalOrder.Cost = NewOrder.Cost;
        CloseAction?.Invoke(true);
    }

    private void Cancel(object? _)
    {
        CloseAction?.Invoke(false);
    }
}
