using Shop.Infrastructure.Customer;
using Shop.Shared.Models.Models;
using Shop.Shared.Models.ViewModels;

namespace Shop.Pages.Management.Account
{
    public class AccountIndexViewModel
    {
        public NameModel CustomerName { get; }
        public DateTimeModel CustomerDateOfBirth { get; }
        public AddressViewModel CustomerHomeAddress { get; }

        public AccountIndexViewModel(Customer customer)
        {
            CustomerName = NameModel.Create(customer);
            CustomerDateOfBirth = new DateTimeModel(customer.DateOfBirth);
            CustomerHomeAddress = new AddressViewModel(customer.HomeAddress);
        }
    }
}