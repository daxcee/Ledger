using System.Data;

namespace Ledger.Models
{
    public interface IQuery<T>
    {
        T Execute(IDbConnection db);
    }
}