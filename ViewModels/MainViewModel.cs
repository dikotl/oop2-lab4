using System.Collections.ObjectModel;
using System.Windows.Input;
using ServiceMarketplace.Models;
using ServiceMarketplace.Services;

namespace ServiceMarketplace.ViewModels;

public class MainViewModel : BaseViewModel
{
    // Dependencies.
    private readonly DialogService _dialogService = new();
    private static readonly ServiceBureau _dummyBureauForNewOnes = new("(new bureau)");

    // Fix for situation when after hitting "Create" in "New Bureau" window it reopens.
    // Dumb but works.
    private bool _creatingNewBureau = false;

    public static OrderStatus[] OrderStatuses => Enum.GetValues<OrderStatus>();

    public ObservableCollection<ServiceBureau> Bureaus
    {
        get;
        set { field = value; OnPropertyChanged(); }
    } = [_dummyBureauForNewOnes];

    public ServiceBureau? SelectedBureau
    {
        get;
        set
        {
            // Intercept the selection.
            if (value == _dummyBureauForNewOnes && !_creatingNewBureau)
            {
                _creatingNewBureau = true;
                // TODO: disallow duplicate bureau names.
                if (_dialogService.OpenCreateBureauDialog(Bureaus) is ServiceBureau newBureau)
                {
                    // Insert it right before the "... create new" dummy item.
                    Bureaus.Insert(Bureaus.Count - 1, newBureau);

                    // Select the newly created bureau.
                    field = newBureau;
                }
                _creatingNewBureau = false;
            }
            else
            {
                // Standard selection.
                field = value;
            }
            OnPropertyChanged();

            // De-select active order.
            SelectedOrder = null;
        }
    }

    public ObservableCollection<string> AddressHistory
    {
        get;
        set { field = value; OnPropertyChanged(); }
    } = ["New York, IDK street, 30"];

    public Order? SelectedOrder
    {
        get;
        set { field = value; OnPropertyChanged(); }
    }

    public bool IsViewingExecutors
    {
        get;
        set { field = value; OnPropertyChanged(); }
    }

    public ICommand AddOrderCommand { get; }
    public ICommand AddExecutorCommand { get; }
    public ICommand ViewDetailsCommand { get; }
    public ICommand ToggleViewCommand { get; }
    public ICommand BureauDetailsCommand { get; }

    public MainViewModel()
    {
        AddOrderCommand = new RelayCommand(ExecuteAddOrder, _ => SelectedBureau is not null);
        AddExecutorCommand = new RelayCommand(ExecuteAddExecutor, _ => SelectedBureau is not null);
        ViewDetailsCommand = new RelayCommand(ExecuteViewDetailsExecutor, _ => SelectedBureau is not null && SelectedOrder is not null);
        ToggleViewCommand = new RelayCommand(ExecuteToggleView, _ => SelectedBureau is not null);
        BureauDetailsCommand = new RelayCommand(ExecuteViewBureauDetails, _ => SelectedBureau is not null);
    }

    private void ExecuteAddOrder(object? parameter)
    {
        if (_dialogService.OpenAddOrderDialog(SelectedBureau!.Staff!, AddressHistory) is Order newOrder)
        {
            SelectedBureau!.Orders!.Add(newOrder);
        }
    }

    private void ExecuteAddExecutor(object? parameter)
    {
        if (_dialogService.OpenAddExecutorDialog() is Executor newExecutor)
        {
            SelectedBureau!.Staff!.Add(newExecutor);
        }
    }

    private void ExecuteViewDetailsExecutor(object? _)
    {
        _dialogService.OpenViewDetailsDialog(SelectedOrder!);
    }

    private void ExecuteToggleView(object? parameter)
    {
        IsViewingExecutors = !IsViewingExecutors;
    }

    private void ExecuteViewBureauDetails(object? _)
    {
        var bureau = SelectedBureau!;
        int staffCount = bureau.Staff?.Count ?? 0;
        int ordersCount = bureau.Orders?.Count ?? 0;
        decimal totalRevenue = bureau.Orders?.Sum(o => o.Cost) ?? 0;

        // TODO: move to DialogService.
        System.Windows.MessageBox.Show(
            $"Total Staff: {staffCount}\n" +
            $"Total Orders: {ordersCount}\n" +
            $"Total Revenue: ${totalRevenue}",
            $"'{bureau.Name}' Bureau Details",
            System.Windows.MessageBoxButton.OK
        );
    }
}
