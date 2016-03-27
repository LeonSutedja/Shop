using System.Web.Mvc;
using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Repository;
using Shop.Shared.Controllers;
using System;

namespace Shop.Pages.Management.Account
{
    public class AccountController : BaseController
    {
        private readonly ICustomerService _customerService;
        public AccountController(IRepository<Customer> customerRepository, ICustomerService customerService) : base(customerRepository)
        {
            _customerService = customerService;
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
            _customerService.ChangeCustomerDetails(CurrentUser.Id, viewModel.FirstName, viewModel.LastName, DateTime.Parse(viewModel.Dob));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateAccountAddress(string street, string streetNumber, string unit, string suburb, string postcode)
        {
            var newAddress = new Address(street, streetNumber, unit, suburb, postcode);
            _customerService.ChangeCustomerAddress(CurrentUser.Id, newAddress);
            return RedirectToAction("Index");
        }
    }
}