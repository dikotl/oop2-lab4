using System.Windows;
using ServiceMarketplace.ViewModels;

namespace ServiceMarketplace.Views;

public partial class ConfirmationWindow : Window
{
    public ConfirmationWindow()
    {
        InitializeComponent();

        // Listen for the ViewModel telling us it's time to close.
        Loaded += (_, _) =>
        {
            if (DataContext is ConfirmationViewModel vm)
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
