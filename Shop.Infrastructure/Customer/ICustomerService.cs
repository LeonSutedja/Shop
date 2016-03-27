using System;

namespace Shop.Infrastructure.Customer
{
    public interface ICustomerService
    {
        bool ChangeCustomerDetails(int customerId, string firstName, string lastName, DateTime dob);
        bool ChangeCustomerAddress(int customerId, Address address);
    }
}
