namespace ServiceMarketplace.ViewModels;

public class ConfirmationViewModel : BaseViewModel
{
    public string Text { get; }

    public Action<bool>? CloseAction { get; set; }
    public Command YesCommand { get; }

    public ConfirmationViewModel(string text, Action ifConfirmed)
    {
        Text = text;
        YesCommand = new(_ => { ifConfirmed(); CloseAction?.Invoke(false); });
    }
}
