using Shop.Infrastructure.Product;
using System.Collections.Generic;

namespace Shop.Pages.Order
{
    public class OrderIndexViewModel
    {
        public IEnumerable<Product> ProductList { get; }

        public OrderIndexViewModel(IEnumerable<Product> productList)
        {
            ProductList = productList;
        }
    }
}