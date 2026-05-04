using System.ComponentModel.DataAnnotations;

namespace ServiceMarketplace.ViewModels;

public class EditExecutorData : BaseViewModel
{
    [Required(ErrorMessage = "Executor should have a name")]
    [Length(2, 32, ErrorMessage = "First name length must be in range from 2 to 32")]
    [RegularExpression(@"\p{L}+([ -]\p{L}+)*", ErrorMessage = "First name should contain only letters, spaces and '-' symbols")]
    public required string FirstName
    {
        get;
        set { field = value; ValidateProperty(value); OnPropertyChanged(); }
    }

    [Required(ErrorMessage = "Executor should have a last name")]
    [Length(2, 32, ErrorMessage = "Last name length must be in range from 2 to 32")]
    [RegularExpression(@"\p{L}+([ -]\p{L}+)*", ErrorMessage = "Last name should contain only letters, spaces and '-' symbols")]
    public required string LastName
    {
        get;
        set { field = value; ValidateProperty(value); OnPropertyChanged(); }
    }
}
