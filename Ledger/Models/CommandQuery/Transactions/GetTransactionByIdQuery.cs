using System.Data;
using System.Linq;
using Dapper;
using Ledger.Models.Entities;

namespace Ledger.Models.CommandQuery.Transactions
{
    public class GetTransactionByIdQuery : IQuery<Transaction>
    {
        readonly long id;

        public GetTransactionByIdQuery(long id)
        {
            this.id = id;
        }

        public Transaction Execute(IDbConnection db)
        {
            var sql = @"SELECT id, desc, amount, datedue, datepayed, datereconciled, account, ledger
                        FROM transactions
                        WHERE id = @id";
            return db.Query<Transaction>(sql, new { id }).FirstOrDefault();
        }
    }
}