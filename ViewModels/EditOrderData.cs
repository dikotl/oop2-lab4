using System.ComponentModel.DataAnnotations;
using ServiceMarketplace.Models;

namespace ServiceMarketplace.ViewModels;

public class EditOrderData : BaseViewModel
{
    [Required]
    public Executor? Executor
    {
        get;
        set { field = value; ValidateProperty(value); OnPropertyChanged(); }
    }

    public ServiceKind Service
    {
        get;
        set { field = value; ValidateProperty(value); OnPropertyChanged(); }
    }

    [Required]
    public string CustomerAddress
    {
        get;
        set { field = value; ValidateProperty(value); OnPropertyChanged(); }
    } = "";

    [Range(1, int.MaxValue, ErrorMessage = "Minimum cost is 1")]
    public int Cost
    {
        get;
        set { field = value; ValidateProperty(value); OnPropertyChanged(); }
    }
}
