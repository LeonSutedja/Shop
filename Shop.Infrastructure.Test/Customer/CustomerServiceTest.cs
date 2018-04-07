using System;
using System.Linq;
using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Interfaces;
using Shop.Infrastructure.Repository;
using Shouldly;
using Unity;
using Xunit;

namespace Shop.Infrastructure.Test.Customer
{
    public class CustomerServiceTest
    {
        private UnityContainer _unityContainer { get; }

        private ICustomerService _customerService { get; }
        private IRepository<Infrastructure.Customer.Customer> _customerRepository { get; }

        public CustomerServiceTest()
        {
            _unityContainer = new UnityContainer();
            UnityConfig.RegisterTypes(_unityContainer);
            _customerService = _unityContainer.Resolve<ICustomerService>();
            _customerRepository = _unityContainer.Resolve<IRepository<Infrastructure.Customer.Customer>>();
        }

        [Fact]
        public void ChangeCustomerDetails_Should_ChangeDetails()
        {
            var firstCustomer = _customerRepository.All().First();
            var firstCustomerId = firstCustomer.Id;

            var firstNameAfterChange = "ChangeToFirstName";
            var lastNameAfterChange = "ChangeToLastName";
            var dobAfterChange = DateTime.Now.AddYears(-20);
            var success = _customerService.ChangeCustomerDetails(
                firstCustomerId,
                firstNameAfterChange,
                lastNameAfterChange,
                DateTime.Now.AddYears(-20));

            success.ShouldBeTrue();
            var firstCustomerAfterChange = _customerRepository.Get(firstCustomerId);
            firstCustomerAfterChange.FirstName.ShouldBe(firstNameAfterChange);
            firstCustomerAfterChange.LastName.ShouldBe(lastNameAfterChange);
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