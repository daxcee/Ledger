using Ledger.Models.CommandQuery;

namespace Ledger.Models
{
    public interface IDatabase
    {
        T Query<T>(IQuery<T> query);
        void Execute(ICommand command);
    }
}