using Shop.Infrastructure.Customer;
using Shop.Infrastructure.Repository;
using System.Web.Mvc;

namespace Shop.Shared.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly Customer CurrentUser;
        protected readonly IRepository<Customer> _customerRepository;

        protected BaseController(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
            CurrentUser = customerRepository.Get(1); /// Hack set for auto login
            ViewBag.CurrentUser = CurrentUser;
        }
    }
}