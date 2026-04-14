using System.Windows;
using ServiceMarketplace.ViewModels;

namespace ServiceMarketplace.Views;

public partial class AddBureauWindow : Window
{
    public AddBureauWindow()
    {
        InitializeComponent();

        // Listen for the ViewModel telling us it's time to close.
        Loaded += (_, _) =>
        {
            if (DataContext is AddBureauViewModel vm)
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
