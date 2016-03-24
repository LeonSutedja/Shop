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
        void RemoveProduct(IProduct productOrdered, int qtyOrdered);
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
            ProductsOrdered = new List<IOrderItem>() { new OrderItem(productOrdered, qtyOrdered) };
            CustomerWhoOrdered = customerWhoOrdered;
            TotalQuantityOrdered += qtyOrdered;
        }

        public void AddProduct(IProduct productOrdered, int qtyOrdered)
        {
            IOrderItem currentProductOrdered;
            if (_tryGetProductOrdered(productOrdered.Id, out currentProductOrdered))
            {
                currentProductOrdered.AddQuantity(qtyOrdered);
                TotalQuantityOrdered += qtyOrdered;
                return;
            }

            var newOrderItem = new OrderItem(productOrdered, qtyOrdered);
            ProductsOrdered.Add(newOrderItem);
            TotalQuantityOrdered += qtyOrdered;
        }

        public void RemoveProduct(IProduct productOrdered, int qtyOrdered)
        {
            IOrderItem currentProductOrdered;
            if (!_tryGetProductOrdered(productOrdered.Id, out currentProductOrdered)) return;
            currentProductOrdered.RemoveQuantity(qtyOrdered);

            if (TotalQuantityOrdered > 0) TotalQuantityOrdered -= qtyOrdered;
        }

        public IOrderItem GetOrderItemForProductOrdered(int productId)
        {
            IOrderItem currentProductOrdered;
            _tryGetProductOrdered(productId, out currentProductOrdered);
            return currentProductOrdered;
        }

        private bool _tryGetProductOrdered(int productId, out IOrderItem orderItem)
        {
            orderItem = ProductsOrdered.FirstOrDefault(pd => pd.ProductOrdered.Id == productId);
            return (orderItem != null);
        }
    }
}
