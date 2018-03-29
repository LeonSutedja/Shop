using Shop.Infrastructure.Repository;
using System;

namespace Shop.Infrastructure.Customer
{
    public class Customer : AbstractId
    {
        public static Customer Create(string firstName, string lastName, DateTime dateOfBirth,
            string street, string streetNumber, string unit, string suburb, string postcode)
        {
            var customerHomeAddress = new Address(street, streetNumber, unit, suburb, postcode);
            var newCustomer = new Customer(firstName, lastName, dateOfBirth, customerHomeAddress);
            return newCustomer;
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public Address HomeAddress { get; private set; }

        public Customer(string firstName, string lastName, DateTime dob, Address homeAddress)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dob;
            HomeAddress = homeAddress;
        }

        public void SetName(string newFirstName, string newLastName)
        {
            FirstName = newFirstName;
            LastName = newLastName;
        }

        public void SetDateOfBirth(DateTime dateOfBirth) => DateOfBirth = dateOfBirth;

        public void SetAddress(Address newAddress) => HomeAddress = newAddress;
    }
}