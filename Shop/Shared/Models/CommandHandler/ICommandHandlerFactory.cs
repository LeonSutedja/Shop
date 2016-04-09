using System.Web.Mvc;

namespace Shop.Shared.Models.CommandHandler
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler<TInput, TOutput> GetCommandHandler<TInput, TOutput>();
    }

    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        public ICommandHandler<TInput, TOutput> GetCommandHandler<TInput, TOutput>()
            => (ICommandHandler<TInput, TOutput>)DependencyResolver.Current.GetService(typeof(ICommandHandler<TInput, TOutput>));
    }
}