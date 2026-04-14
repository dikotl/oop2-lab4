using System.Windows;
using ServiceMarketplace.ViewModels;

namespace ServiceMarketplace.Views;

public partial class AddExecutorWindow : Window
{
    public AddExecutorWindow()
    {
        InitializeComponent();

        // Listen for the ViewModel telling us it's time to close.
        Loaded += (_, _) =>
        {
            if (DataContext is AddExecutorViewModel vm)
            {
                vm.CloseAction = (result) =>
                {
                    this.DialogResult = result;
                    this.Close();
                };
            }
        };
    }
}
