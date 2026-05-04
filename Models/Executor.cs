using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using ServiceMarketplace.Models.ValidationAttributes;

namespace ServiceMarketplace.Models;

public class Executor : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string? property = null)
    {
        PropertyChanged?.Invoke(this, new(property));
    }

    [Required(ErrorMessage = "Executor should have a name")]
    [Length(1, 32, ErrorMessage = "Name length must be in range from 1 to 32")]
    [RegularExpression(@"\p{L}+([ -]\p{L}+)*")]
    public required string FirstName { get; set { field = value; OnPropertyChanged(); } }

    [Required(ErrorMessage = "Executor should have a last name")]
    [Length(1, 32, ErrorMessage = "Last name length must be in range from 1 to 32")]
    [RegularExpression(@"\p{L}+([ -]\p{L}+)*")]
    public required string LastName { get; set { field = value; OnPropertyChanged(); } }

    [Required(ErrorMessage = "Executor's age must be known")]
    [MinAge(18)]
    public DateOnly Birthday { get; set { field = value; OnPropertyChanged(); } }

    public override string ToString() => $"{FirstName} {LastName}";
}
