using Shop.Infrastructure.Customer;

namespace Shop.Shared.Models
{
    /// <summary>
    /// NameViewModel purpose is to handle name viewing. It's main concern is displaying the name to the user according to the application needs.
    /// </summary>
    public class NameModel
    {
        // Factory method to help creating NameModel from a customer.
        public static NameModel Create(Customer customer) => new NameModel(customer.FirstName, customer.LastName);

        public string FirstName { get; }
        public string LastName { get; }

        private NameModel(string firstName, string lastName)
        {
            if (!string.IsNullOrEmpty(firstName)) FirstName = firstName;
            if (!string.IsNullOrEmpty(lastName)) LastName = lastName;
        }

        public override string ToString() => string.Format("{0} {1}", FirstName, LastName);
    }
}