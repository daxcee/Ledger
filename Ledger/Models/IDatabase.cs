using Ledger.Models.CommandQuery;

namespace Ledger.Models
{
    public interface IDatabase
    {
        T Execute<T>(IQuery<T> query);
        void Execute(ICommand command);
    }
}