using System.ComponentModel.DataAnnotations;

namespace ServiceMarketplace.Models;

public enum OrderStatus
{
    Created,
    InProgress,
    Completed,
}

public record Order(
    Executor Executor,
    ServiceKind Service,
    string Address,
    DateTime CreationDate,
    OrderStatus Status,
    [property: Range(1, int.MaxValue)] int Cost
);
