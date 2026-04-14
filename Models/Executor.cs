using System.ComponentModel.DataAnnotations;
using ServiceMarketplace.Models.ValidationAttributes;

namespace ServiceMarketplace.Models;

public record Executor(
    [property: Required(ErrorMessage = "Executor should have a name")]
    [property: Length(1, 32, ErrorMessage = "Name length must be in range from 1 to 32")]
    [property: RegularExpression(@"\p{L}+([ -]\p{L}+)*")]
    string FirstName,

    [property: Required(ErrorMessage = "Executor should have a last name")]
    [property: Length(1, 32, ErrorMessage = "Last name length must be in range from 1 to 32")]
    [property: RegularExpression(@"\p{L}+([ -]\p{L}+)*")]
    string LastName,

    [property: Required(ErrorMessage = "Executor's age must be known")]
    [property: MinAge(18)]
    DateOnly Birthday
)
{
    public override string ToString() => $"{FirstName} {LastName}";
}
