namespace ServiceMarketplace.Models;

public record ServiceBureau(string Name, IList<Order> Orders, IList<Executor> Staff)
{
    public ServiceBureau(string name) : this(name, [], []) { }

    public string ToShortString()
    {
        // Calculating the total cost of all completed orders
        int totalCost = Orders.Sum(order => order.Cost);
        return $"Service Bureau: {Name} | Total Revenue: {totalCost}";
    }

    public override string ToString()
    {
        string fullInfo = $"Bureau: {Name}\nTotal Orders Count: {Orders.Count}\n";
        foreach (var order in Orders)
        {
            fullInfo += order.ToString() + "\n";
        }
        return fullInfo;
    }
}
