namespace Shop.Shared.Models.CommandHandler
{
    // T is the input model
    // T1 is the expected output for the input
    // For every command in this application, there must be a userId
    public interface ICommandHandler<TInput, TOutput>
    {
        TOutput Handle(TInput model, int userId);
    }

    public class CommandHandlerLoggerDecorator<TInput, TEntity> : ICommandHandler<TInput, TEntity>
    {
        private readonly ICommandHandler<TInput, TEntity> _handler;

        public CommandHandlerLoggerDecorator(ICommandHandler<TInput, TEntity> handler)
        {
            _handler = handler;
        }

        public TEntity Handle(TInput model, int userId)
        {
            // Log the command here
            var result = _handler.Handle(model, userId);
            // Log the result here
            return result;
        }
    }
}