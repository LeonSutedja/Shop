using Shop.Infrastructure.Customer;
using System;

namespace Shop.Infrastructure.Interfaces
{
    public interface ICustomerService
    {
        bool ChangeCustomerDetails(int customerId, string firstName, string lastName, DateTime dob);

        bool ChangeCustomerAddress(int customerId, Address address);
    }
}