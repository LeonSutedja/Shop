using Shop.Infrastructure.Repository;
using System;

namespace Shop.Infrastructure.Product
{
    public interface IProduct : IId
    {
        string Name { get; }
        string Description { get; }

        void AddStock(int qty);

        void RemoveStock(int qty);
    }

    public class Product : AbstractId, IProduct
    {
        public string Name { get; private set; }
        public int StockQuantity { get; private set; }
        public string Description { get; private set; }

        public Product(string name, int stockQuantity = 0, string description = "")
        {
            if (stockQuantity < 0) throw new Exception("Stock cannot be less than 0");
            Name = name;
            StockQuantity = stockQuantity;
            Description = description;
        }

        public void AddStock(int qty) => StockQuantity += qty;

        public void RemoveStock(int qty)
        {
            if (qty > StockQuantity) throw new Exception("Not enough stock");
            StockQuantity -= qty;
        }

        public void SetDescription(string updatedDescription) => Description = updatedDescription;

        public void SetName(string newName) => Name = newName;
    }
}