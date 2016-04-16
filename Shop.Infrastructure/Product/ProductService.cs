using Shop.Infrastructure.Repository;
using System;

namespace Shop.Infrastructure.Product
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public bool UpdateProductDetails(int productId, string name, string description)
        {
            Product product;
            if (!tryGetProduct(productId, out product)) return false;
            product = _productRepository.Get(productId);
            product.SetName(name);
            product.SetDescription(description);
            return true;
        }

        private bool tryGetProduct(int productId, out Product productResult)
        {
            productResult = null;
            var productToGet = _productRepository.Get(productId);
            if (productToGet == null) return false;
            productResult = productToGet;
            return true;
        }
    }
}
