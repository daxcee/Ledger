using System.Data;

namespace Ledger.Models
{
    public interface ICommand
    {
        void Execute(IDbConnection db);
    }
}