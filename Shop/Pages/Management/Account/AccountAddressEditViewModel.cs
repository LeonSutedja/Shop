using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Interfaces;
using Shop.Shared.Models.CommandHandler;

namespace Shop.Pages.Management.Account
{
    public class AccountAddressEditViewModel
    {
        public string street { get; set; }
        public string streetNumber { get; set; }
        public string unit { get; set; }
        public string suburb { get; set; }
        public string postcode { get; set; }

        public AccountAddressEditViewModel() { }
    }

    public class AccountAddressEditViewModelHandler : ICommandHandler<AccountAddressEditViewModel, bool>
    {
        private readonly ICustomerService _customerService;
        public AccountAddressEditViewModelHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public bool Handle(AccountAddressEditViewModel model, int userId)
        {
            var newAddress = new Address(model.street, model.streetNumber, model.unit, model.suburb, model.postcode);
            return _customerService.ChangeCustomerAddress(userId, newAddress);
        }
    }
}