using System.Text.Json.Serialization;

namespace ServiceMarketplace.Models;

public record ServiceBureau(string Name, IList<Order> Orders, IList<Executor> Staff)
{
    [JsonConstructor]
    public ServiceBureau(string name) : this(name, [], []) { }
}
