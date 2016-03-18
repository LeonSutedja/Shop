using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Product;
using System.Collections.Generic;
using System.Linq;
using Shop.Infrastructure.Repository;

namespace Shop.Order
{  
    public interface IOrder
    {
        List<IOrderItem>  ProductsOrdered { get; }
        Customer CustomerWhoOrdered { get; }
        void AddProduct(IProduct product, int qtyOrdered);
    }

    public class Order : AbstractId, IOrder
    {
        public List<IOrderItem> ProductsOrdered { get; }
        public Customer CustomerWhoOrdered { get; }
        public int TotalQuantityOrdered { get; private set; }

        public Order(Customer customerWhoOrdered) {
            ProductsOrdered = new List<IOrderItem>();
            CustomerWhoOrdered = customerWhoOrdered;
            TotalQuantityOrdered = 0;
        }
        public Order(Customer customerWhoOrdered, IProduct productOrdered, int qtyOrdered) {
            ProductsOrdered = new List<IOrderItem>() {
                new OrderItem(productOrdered, qtyOrdered)
            };
            CustomerWhoOrdered = customerWhoOrdered;
            TotalQuantityOrdered += qtyOrdered;
        }
        public void AddProduct(IProduct productOrdered, int qtyOrdered)
        {
            if (ProductsOrdered.Any(pd => pd.ProductOrdered.Id == productOrdered.Id))
            {
                var currentProductOrdered = ProductsOrdered.First(pd => pd.ProductOrdered.Id == productOrdered.Id);
                currentProductOrdered.AddQuantity(qtyOrdered);
                TotalQuantityOrdered += qtyOrdered;
            }
            else
            {
                var newOrderItem = new OrderItem(productOrdered, qtyOrdered);
                ProductsOrdered.Add(newOrderItem);
                TotalQuantityOrdered += qtyOrdered;
            }
        }

        public IOrderItem GetOrderItemForProductOrdered(int productId)
        {
            if (!ProductsOrdered.Any(pd => pd.ProductOrdered.Id == productId)) return null;
            var currentProductOrdered = ProductsOrdered.First(pd => pd.ProductOrdered.Id == productId);
            return currentProductOrdered;
        }
    }
}
