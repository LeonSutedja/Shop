using Shop.Infrastructure.Customer;
using System.Web.Mvc;
using Shop.Infrastructure.Repository;

namespace Shop.Shared
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