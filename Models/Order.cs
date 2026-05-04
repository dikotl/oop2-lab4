using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ServiceMarketplace.Models;

public enum OrderStatus
{
    Created,
    InProgress,
    Completed,
}

public class Order : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string? property = null)
    {
        PropertyChanged?.Invoke(this, new(property));
    }

    public required Executor Executor { get; set { field = value; OnPropertyChanged(); } }
    public ServiceKind Service { get; set { field = value; OnPropertyChanged(); } }
    public required string Address { get; set { field = value; OnPropertyChanged(); } }
    public DateTime CreationDate { get; set { field = value; OnPropertyChanged(); } }
    public OrderStatus Status { get; set { field = value; OnPropertyChanged(); } }
    [Range(1, int.MaxValue)]
    public int Cost { get; set { field = value; OnPropertyChanged(); } }
}
