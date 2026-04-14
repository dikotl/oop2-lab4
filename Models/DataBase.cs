namespace ServiceMarketplace.Models;

public record DataBase(
    IList<ServiceBureau> ServiceCenters,
    IList<Customer> Clients,
    IList<Executor> Executors
);
