using System.Data;

namespace Ledger.Models
{
    public class Database : IDatabase
    {
        readonly IDbConnection _db;

        public Database(IDbConnection db)
        {
            this._db = db;
        }

        public T Query<T>(IQuery<T> query)
        {
            return query.Execute(_db);
        }

        public void Execute(ICommand command)
        {
            command.Execute(_db);
        }
    }
}