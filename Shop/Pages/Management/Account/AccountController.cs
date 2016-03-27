using System.Web.Mvc;
using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Repository;
using Shop.Shared.Controllers;
using System;

namespace Shop.Pages.Management.Account
{
    public class AccountController : BaseController
    {
        public AccountController(IRepository<Customer> customerRepository) : base(customerRepository)
        {
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
        public ActionResult Index(AccountDetailsEditViewModel viewModel)
        {
            //Hack for product repository. Rather than persisting to DB, we persist into session.
            var customer = _customerRepository.Get(CurrentUser.Id);
            customer.SetName(viewModel.FirstName, viewModel.LastName);
            customer.SetDateOfBirth(DateTime.Parse(viewModel.Dob));
            return View(new AccountIndexViewModel(CurrentUser));
        }
    }
}