using Shop.Infrastructure.Repository;

namespace Shop.Infrastructure.Product
{
    public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository()
        {
            Add(new Product("Aveda", 20, "This is one really expensive brand."));
            Add(new Product("Clear", 11, "I used to use Clear previously."));
            Add(new Product("Pantene", 32, "I have used Pantene before, and it did not worked well for me."));
            Add(new Product("Head & Shoulder", 24, "I have no idea what to write in this description box."));
            Add(new Product("Redken", 11, "Some really amazing game or long description is just nuts."));
            Add(new Product("Biolage", 22, "I love Star Wars."));
            Add(new Product("Aveeno", 23, "Be careful with your eyes."));
            Add(new Product("Neutrogena", 12, "Use the conditioner or extra face wash."));
            Add(new Product("Sunsilk", 12, "Make your hair shines everyday."));
        }
    }
}