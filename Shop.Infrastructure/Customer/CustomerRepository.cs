using System;
using Shop.Infrastructure.Repository;

namespace Shop.Infrastructure.Customer
{
    public class CustomerRepository : GenericRepository<Customer>
    {
        public CustomerRepository()
        {
            var customer = Customer.Create("Sherlock", "Holmes", DateTime.Parse("11/11/1988"), "Baker", "221", "Blahsuburb", "2000");
            Add(customer);
        }
    }
}
