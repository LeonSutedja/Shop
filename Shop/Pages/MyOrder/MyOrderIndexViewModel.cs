using System.Collections.Generic;

namespace Shop.Pages.MyOrder
{
    public class MyOrderIndexViewModel
    {
        public IEnumerable<Shop.Order.Order> ProcessedOrders { get; }
        public Shop.Order.Order CurrentOrders { get; }
        public MyOrderIndexViewModel(IEnumerable<Shop.Order.Order> processedOrders, Shop.Order.Order currentOrders)
        {
            ProcessedOrders = processedOrders;
            CurrentOrders = currentOrders;
        }
    }
}