using Autofac;
using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Interfaces;
using Shop.Infrastructure.Repository;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace Shop.Infrastructure.Test.Customer
{
    public class CustomerServiceTest
    {
        private ICustomerService _customerService { get; }
        private IRepository<Infrastructure.Customer.Customer> _customerRepository { get; }

        public CustomerServiceTest()
        {
            var autofacConfig = new AutofacConfig();
            var container = autofacConfig.InitiateAutofacContainerBuilder();
            _customerService = container.Resolve<ICustomerService>();
            _customerRepository = container.Resolve<IRepository<Infrastructure.Customer.Customer>>();
        }

        [Theory]
        [InlineData("ChangeToFirstName", "ChangeToLastName")]
        [InlineData("ABC1234567890", "ABC1234567890")]
        [InlineData("1234567890", "1234567890")]
        [InlineData("~!@#$%^&*()_+-=[]\\{}|';/.,<>?:\"", "~!@#$%^&*()_+-=[]\\{}|';/.,<>?:\"")]
        public void ChangeCustomerDetails_Should_ChangeDetails(string firstNameToChange, string lastNameToChange)
        {
            var firstCustomer = _customerRepository.All().First();
            var firstCustomerId = firstCustomer.Id;

            var dobAfterChange = DateTime.Now.AddYears(-20);
            var success = _customerService.ChangeCustomerDetails(
                firstCustomerId,
                firstNameToChange,
                lastNameToChange,
                DateTime.Now.AddYears(-20));

            success.ShouldBeTrue();
            var firstCustomerAfterChange = _customerRepository.Get(firstCustomerId);
            firstCustomerAfterChange.FirstName.ShouldBe(firstNameToChange);
            firstCustomerAfterChange.LastName.ShouldBe(lastNameToChange);
            firstCustomerAfterChange.DateOfBirth.ShouldBe(dobAfterChange);
        }

        [Fact]
        public void ChangeCustomerAddress_Should_ChangeAddress()
        {
            var firstCustomerId = _customerRepository.All().First().Id;
            var newAddress = new Address("Duncan", "1", "", "Murumbeena", "3153");

            var success = _customerService.ChangeCustomerAddress(firstCustomerId, newAddress);

            success.ShouldBeTrue();
            var firstCustomerAfterChange = _customerRepository.Get(firstCustomerId);
            firstCustomerAfterChange.HomeAddress.Street.ShouldBe(newAddress.Street);
            firstCustomerAfterChange.HomeAddress.Postcode.ShouldBe(newAddress.Postcode);
            firstCustomerAfterChange.HomeAddress.Suburb.ShouldBe(newAddress.Suburb);
            firstCustomerAfterChange.HomeAddress.Unit.ShouldBe(newAddress.Unit);
        }
    }
}