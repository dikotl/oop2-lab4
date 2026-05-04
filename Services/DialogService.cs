using System.Windows;
using ServiceMarketplace.Models;
using ServiceMarketplace.ViewModels;
using ServiceMarketplace.Views;

namespace ServiceMarketplace.Services;

public class DialogService
{
    public Order? OpenAddOrderDialog(IList<Executor> executors, IList<string> addresses)
    {
        var vm = new AddOrderViewModel(executors, addresses);
        var dialog = new AddOrderWindow() { DataContext = vm };

        if (dialog.ShowDialog() is true)
        {
            return vm.CreatedOrder;
        }

        // User clicked `Cancel` button or just closed the window.
        return null;
    }

    public bool OpenEditOrderDialog(Order originalOrder, IList<Executor> executors, IList<string> addresses)
    {
        var vm = new EditOrderViewModel(originalOrder, executors, addresses);
        var dialog = new EditOrderWindow() { DataContext = vm };

        return dialog.ShowDialog() is true;
    }

    public Executor? OpenAddExecutorDialog()
    {
        var vm = new AddExecutorViewModel();
        var dialog = new AddExecutorWindow() { DataContext = vm };

        if (dialog.ShowDialog() is true)
        {
            return vm.CreatedExecutor;
        }

        // User clicked `Cancel` button or just closed the window.
        return null;
    }

    public ServiceBureau? OpenCreateBureauDialog(IList<ServiceBureau> bureaus)
    {
        var vm = new AddBureauViewModel(bureaus);
        var dialog = new AddBureauWindow() { DataContext = vm };

        if (dialog.ShowDialog() is true)
        {
            return vm.CreatedBureau;
        }

        // User clicked `Cancel` button or just closed the window.
        return null;
    }

    public void OpenViewDetailsDialog(Order order)
    {
        MessageBox.Show(
            $"Created at {order.CreationDate}\n" +
            $"Status: {order.Status}\n" +
            $"Service: {order.Service}\n" +
            $"Address: {order.Address}\n" +
            $"Executor: {order.Executor}\n" +
            $"Cost: ${order.Cost}",
            "Order Information"
        );
    }

    public void ShowError(string message, string caption)
    {
        MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
