using System.Web.Mvc;
using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Repository;
using Shop.Shared.Controllers;

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
    }
}