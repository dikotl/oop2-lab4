using System.Collections.ObjectModel;
using ServiceMarketplace.Models;
using ServiceMarketplace.Services;

namespace ServiceMarketplace.ViewModels;

public class MainViewModel : BaseViewModel
{
    public static OrderStatus[] OrderStatuses => Enum.GetValues<OrderStatus>();

    // Dependencies.
    private readonly DialogService _dialogService = new();
    private readonly DataBaseService _dbService = new();
    private static readonly ServiceBureau _dummyBureauForNewOnes = new("(new bureau)");

    // Fix for situation when after hitting "Create" in "New Bureau" window it reopens.
    // Dumb but works.
    private bool _creatingNewBureau = false;

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
                if (_dialogService.OpenCreateBureauDialog(Bureaus) is ServiceBureau newBureau)
                {
                    // Insert it right before the "(new bureau)" dummy item.
                    Bureaus.Insert(Bureaus.Count - 1, newBureau);

                    // Select the newly created bureau.
                    field = newBureau;
                }
                else
                {
                    // If dialog was canceled, reset selection.
                    field = null;
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

    public RelayCommand AddOrderCommand { get; }
    public RelayCommand AddExecutorCommand { get; }
    public RelayCommand EditOrderCommand { get; }
    public RelayCommand EditExecutorCommand { get; }
    public RelayCommand ViewDetailsCommand { get; }
    public RelayCommand ToggleViewCommand { get; }
    public RelayCommand BureauDetailsCommand { get; }
    public RelayCommand SaveDataBaseCommand { get; }
    public RelayCommand LoadDataBaseCommand { get; }

    public MainViewModel()
    {
        AddOrderCommand = new(_ =>
        {
            if (_dialogService.OpenAddOrderDialog(SelectedBureau!.Staff!, AddressHistory) is Order newOrder)
            {
                SelectedBureau!.Orders!.Add(newOrder);
            }
        }, _ => SelectedBureau is not null);

        AddExecutorCommand = new(_ =>
        {
            if (_dialogService.OpenAddExecutorDialog() is Executor newExecutor)
            {
                SelectedBureau!.Staff!.Add(newExecutor);
            }
        }, _ => SelectedBureau is not null);

        EditOrderCommand = new(
            _ =>
            {
                _dialogService.OpenEditOrderDialog(SelectedOrder!, SelectedBureau!.Staff!, AddressHistory);
            },
            _ => SelectedBureau is not null && SelectedOrder is not null
        );

        EditExecutorCommand = new(
            _ => { },
            _ => SelectedBureau is not null && SelectedOrder is not null
        );

        ViewDetailsCommand = new(
            _ => _dialogService.OpenViewDetailsDialog(SelectedOrder!),
            _ => SelectedBureau is not null && SelectedOrder is not null
        );

        ToggleViewCommand = new(
            _ => IsViewingExecutors = !IsViewingExecutors,
            _ => SelectedBureau is not null
        );

        BureauDetailsCommand = new(_ =>
        {
            var bureau = SelectedBureau!;
            int staffCount = bureau.Staff?.Count ?? 0;
            int ordersCount = bureau.Orders?.Count ?? 0;
            int createdOrdersCount = bureau.Orders?.Count(o => o.Status == OrderStatus.Created) ?? 0;
            int inProgressOrdersCount = bureau.Orders?.Count(o => o.Status == OrderStatus.InProgress) ?? 0;
            int completedOrdersCount = bureau.Orders?.Count(o => o.Status == OrderStatus.Completed) ?? 0;
            decimal totalRevenue = bureau.Orders?.Sum(o => o.Cost) ?? 0;

            // TODO: move to DialogService.
            System.Windows.MessageBox.Show(
                $"Total Staff: {staffCount}\n" +
                $"Total Orders: {ordersCount}\n" +
                $" - Just created: {createdOrdersCount}\n" +
                $" - In progress: {inProgressOrdersCount}\n" +
                $" - Completed: {completedOrdersCount}\n" +
                $"Total Revenue: ${totalRevenue}",
                $"'{bureau.Name}' Bureau Details",
                System.Windows.MessageBoxButton.OK
            );
        }, _ => SelectedBureau is not null);

        SaveDataBaseCommand = new(_ =>
        {
            // Remove dummy bureau.
            var bureaus = Bureaus.ToList()[..^1];
            _dbService.Save(new(bureaus, AddressHistory), _dialogService);
        });

        LoadDataBaseCommand = new(_ =>
        {
            try
            {
                if (_dbService.Load(_dialogService) is DataBase db)
                {
                    Bureaus = [.. db.Bureaus, _dummyBureauForNewOnes];
                    AddressHistory = [.. db.AddressHistory];
                    OnPropertyChanged(nameof(Bureaus));
                }
            }
            catch (DataBaseException e)
            {
                _dialogService.ShowError(
                    "Database cannot be loaded properly, it might be corrupted.\n\n" + e.Message,
                    "Cannot load the database"
                );
            }
        });
    }
}
