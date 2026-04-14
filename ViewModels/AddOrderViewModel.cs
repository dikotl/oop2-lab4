using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using ServiceMarketplace.Models;

namespace ServiceMarketplace.ViewModels;

public class AddOrderViewModel : BaseViewModel
{
    public static ServiceKind[] AvailableServices => Enum.GetValues<ServiceKind>();
    public IList<Executor> AvailableExecutors { get; }
    public IList<string> AvailableAddresses { get; }

    [Required]
    public Executor? SelectedExecutor
    {
        get;
        set { field = value; ValidateProperty(value); OnPropertyChanged(); }
    }

    public ServiceKind SelectedService
    {
        get;
        set { field = value; ValidateProperty(value); OnPropertyChanged(); }
    }

    [Required]
    public string CustomerAddress
    {
        get;
        set { field = value; ValidateProperty(value); OnPropertyChanged(); }
    }

    [Range(1, int.MaxValue, ErrorMessage = "Minimum cost is 1")]
    public int Cost
    {
        get;
        set { field = value; ValidateProperty(value); OnPropertyChanged(); }
    }

    public Order? CreatedOrder { get; private set; }
    public Action<bool>? CloseAction { get; set; }
    public ICommand CreateCommand { get; }
    public ICommand CancelCommand { get; }

    public AddOrderViewModel(IList<Executor> executors, IList<string> addresses)
    {
        SelectedExecutor = null;
        SelectedService = ServiceKind.Cleaning;
        CustomerAddress = "";
        Cost = 100;

        AvailableExecutors = executors;
        AvailableAddresses = addresses;

        CreateCommand = new RelayCommand(Create, _ => !HasErrors);
        CancelCommand = new RelayCommand(Cancel);
    }

    private void Create(object? obj)
    {
        CreatedOrder = new Order(
            SelectedExecutor!,
            SelectedService,
            CustomerAddress,
            DateTime.Now,
            OrderStatus.Created,
            Cost
        );
        CloseAction?.Invoke(true);
    }

    private void Cancel(object? obj)
    {
        CloseAction?.Invoke(false);
    }
}
