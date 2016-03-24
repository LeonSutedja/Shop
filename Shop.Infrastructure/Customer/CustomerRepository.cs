using System;
using Shop.Infrastructure.Repository;

namespace Shop.Infrastructure.Customer
{
    public class CustomerRepository : GenericRepository<Customer>
    {
        public CustomerRepository()
        {
            var customer = Customer.Create("Sherlock", "Holmes", DateTime.Parse("11/11/1988"), "McKelvie Court", "1", "9", "Glen Waverley", "3150");
            Add(customer);
        }
    }
}
