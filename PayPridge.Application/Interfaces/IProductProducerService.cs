namespace PayPridge.Application.Interfaces
{
    public interface IProductProducerService
    {
        Task FetchAndPublishProducts();
    }
}