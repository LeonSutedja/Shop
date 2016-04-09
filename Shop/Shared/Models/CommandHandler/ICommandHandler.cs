namespace Shop.Shared.Models.CommandHandler
{
    // T is the input model
    // T1 is the expected output for the input
    // For every command in this application, there must be a userId
    public interface ICommandHandler<TInput, TOutput>
    {
        TOutput Handle(TInput model, int userId);
    }
}