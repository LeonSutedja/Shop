using Shop.Infrastructure.Customer;

namespace Shop.Shared.Models.Models
{
    /// <summary>
    /// NameViewModel purpose is to handle name viewing. It's main concern is displaying the name to the user according to the application needs.
    /// </summary>
    public class NameModel
    {
        public static NameModel Create(Customer customer) => new NameModel(customer.FirstName, customer.LastName);

        public string FirstName { get; }
        public string LastName { get; }
        private NameModel(string firstName, string lastName)
        {
            if (firstName != null) FirstName = firstName;
            if (lastName != null) LastName = lastName;
        }
        public override string ToString() => FirstName + " " + LastName;
    }
}