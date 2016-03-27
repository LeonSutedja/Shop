using Shop.Infrastructure.Repository;
using System;

namespace Shop.Infrastructure.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;
        public CustomerService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public bool ChangeCustomerAddress(int customerId, Address newAddress)
        {
            Customer customer;
            if (!tryGetCustomer(customerId, out customer)) return false;
            customer = _customerRepository.Get(customerId);
            customer.SetAddress(newAddress);
            return true;
        }

        public bool ChangeCustomerDetails(int customerId, string firstName, string lastName, DateTime dob)
        {
            Customer customer;
            if (!tryGetCustomer(customerId, out customer)) return false;
            customer = _customerRepository.Get(customerId);
            customer.SetName(firstName, lastName);
            customer.SetDateOfBirth(dob);
            return true;
        }

        private bool tryGetCustomer(int customerId, out Customer customerResult)
        {
            customerResult = null;
            var customerToGet = _customerRepository.Get(customerId);
            if (customerToGet == null) return false;
            customerResult = customerToGet;
            return true;
        }
    }
}
