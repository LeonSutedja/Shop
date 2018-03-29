using Shop.Infrastructure.Interfaces;
using Shop.Shared.Models.CommandHandler;
using System;

namespace Shop.Pages.Management.Account
{
    public class AccountDetailsEditViewModel
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dob { get; set; }

        public AccountDetailsEditViewModel()
        {
        }
    }

    public class AccountDetailsEditViewModelHandler : ICommandHandler<AccountDetailsEditViewModel, bool>
    {
        private readonly ICustomerService _customerService;

        public AccountDetailsEditViewModelHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public bool Handle(AccountDetailsEditViewModel model, int userId)
            => _customerService.ChangeCustomerDetails(userId, model.FirstName,
                model.LastName, DateTime.Parse(model.Dob));
    }
}