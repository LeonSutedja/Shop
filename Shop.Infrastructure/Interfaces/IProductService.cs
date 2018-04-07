using Shop.Infrastructure.TableCreator;

namespace Shop.Infrastructure.Interfaces
{
    public interface IProductService
    {
        bool UpdateProductDetails(int productId, string name, string description);

        bool AddProduct(string name, string description);

        bool AddToStock(int productId, int quantity);

        TableOutput GetProducts(TableInput input);
    }
}