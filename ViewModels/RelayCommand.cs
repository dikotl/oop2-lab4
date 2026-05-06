using System.Windows.Input;

namespace ServiceMarketplace.ViewModels;

// A reusable command class that routes actions from the View to the ViewModel.
public record Command(Action<object?> Action, Predicate<object?>? Predicate = null) : ICommand
{
    bool ICommand.CanExecute(object? parameter) => Predicate is null || Predicate(parameter);
    void ICommand.Execute(object? parameter) => Action(parameter);

    // Tells WPF to re-evaluate the CanExecute method when UI interactions happen.
    //
    // I hate global state.
    // I hate global state.
    // I hate global state.
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}
