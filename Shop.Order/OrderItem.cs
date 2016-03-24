using Shop.Infrastructure.Product;

namespace Shop.Order
{
    public interface IOrderItem
    {
        IProduct ProductOrdered { get; }
        int QtyOrdered { get; }
        void AddQuantity(int qtyOrdered);
        void RemoveQuantity(int qtyOrdered);
    }

    public class OrderItem : IOrderItem
    {
        public IProduct ProductOrdered { get; }
        public int QtyOrdered { get; private set; }
        public OrderItem(IProduct productOrdered, int qtyOrdered = 1)
        {
            ProductOrdered = productOrdered;
            QtyOrdered = qtyOrdered;
        }

        public void AddQuantity(int qty) => QtyOrdered += qty;
        public void RemoveQuantity(int qty)
        {
            if (QtyOrdered >= qty) QtyOrdered -= qty;
        } 
    }
}
