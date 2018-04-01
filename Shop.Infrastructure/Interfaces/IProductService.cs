using Shop.Infrastructure.TableCreator;

namespace Shop.Infrastructure.Interfaces
{
    public interface IProductService
    {
        bool UpdateProductDetails(int productId, string name, string description);
        TableOutput GetProducts();
    }
}