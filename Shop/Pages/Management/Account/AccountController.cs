using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Interfaces;
using Shop.Infrastructure.Repository;
using Shop.Shared.Controllers;
using Shop.Shared.Models.CommandHandler;
using System.Web.Mvc;

namespace Shop.Pages.Management.Account
{
    public class AccountController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly ICommandHandlerFactory _commandHandlerFactory;

        public AccountController(IRepository<Customer> customerRepository,
            ICustomerService customerService, ICommandHandlerFactory commandHandlerFactory) : base(customerRepository)
        {
            _customerService = customerService;
            _commandHandlerFactory = commandHandlerFactory;
        }

        /// <summary>
        /// Landing Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //Hack for product repository. Rather than persisting to DB, we persist into session.
            return View(new AccountIndexViewModel(CurrentUser));
        }

        [HttpPost]
        public ActionResult UpdateAccountDetails(AccountDetailsEditViewModel viewModel)
        {
            var isSuccess = _commandHandlerFactory.GetCommandHandler<AccountDetailsEditViewModel, bool>().Handle(viewModel, CurrentUser.Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateAccountAddress(AccountAddressEditViewModel model)
        {
            var isSuccess = _commandHandlerFactory.GetCommandHandler<AccountAddressEditViewModel, bool>().Handle(model, CurrentUser.Id);
            return RedirectToAction("Index");
        }
    }
}