namespace Shop.Infrastructure.Product
{
    public interface IProductService
    {
        bool UpdateProductDetails(int productId, string name, string description);
    }
}
