using System.Windows;
using ServiceMarketplace.ViewModels;

namespace ServiceMarketplace.Views;

public partial class EditExecutorWindow : Window
{
    public EditExecutorWindow()
    {
        InitializeComponent();

        // Listen for the ViewModel telling us it's time to close.
        Loaded += (_, _) =>
        {
            if (DataContext is EditExecutorViewModel vm)
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
