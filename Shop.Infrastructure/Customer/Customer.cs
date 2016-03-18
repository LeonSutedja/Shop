using System;
using Shop.Infrastructure.Repository;

namespace Shop.Infrastructure.Customer
{
    public class Customer : AbstractId
    {
        public static Customer Create(string firstName, string lastName, DateTime dateOfBirth, string street, 
            string number, string suburb, string postcode)
        {
            var customerHomeAddress = new Address(street, number, suburb, postcode);
            var newCustomer = new Customer(firstName, lastName, dateOfBirth, customerHomeAddress);
            return newCustomer;
        }
        public string FirstName { get; }
        public string LastName { get; }
        public DateTime DateOfBirth { get; }
        public Address HomeAddress { get; }

        public Customer(string firstName, string lastName, DateTime dob, Address homeAddress)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dob;
            HomeAddress = homeAddress;
        }
    }
}
