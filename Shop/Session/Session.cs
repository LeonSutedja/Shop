using System.Web;
using Shop.Infrastructure.Customer;
using Shop.Session.SessionEntity;

namespace Shop.Session
{
    internal abstract class Session<T> : ISession<T>
    {
        protected abstract string _suffixSessionName { get; }
        protected abstract T _newEntity();

        private string _getSessionName(Customer currentUser) => currentUser.Id + _suffixSessionName;
        protected HttpContextBase _context { get; }
        protected Customer _currentUser { get; }
        protected Session(HttpContextBase context, Customer currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public T Get()
        {
            var sessionName = _getSessionName(_currentUser);
            if (_context.Session[sessionName] == null) Reset();
            return (T)_context.Session[sessionName];
        }

        public virtual void Reset() => Set(_newEntity());

        public virtual void Set(T newEntity)
        {
            var sessionName = _getSessionName(_currentUser);
            if (_context.Session[sessionName] != null)
                _context.Session.Remove(sessionName);
            _context.Session.Add(sessionName, newEntity);
        }
    } 

    /// <summary>
    /// This application should only use this to access the sessions.
    /// </summary>
    internal static class SessionFacade
    {
        public static ISession<Order.Order> CurrentCustomerOrder(HttpContextBase context, Customer currentCustomer) 
            => new CurrentCustomerOrder(context, currentCustomer);
    }
}