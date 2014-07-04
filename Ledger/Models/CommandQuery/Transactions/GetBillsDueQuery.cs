using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Ledger.Models.Entities;

namespace Ledger.Models.CommandQuery.Transactions
{
    public class GetBillsDueQuery : IQuery<List<Transaction>>
    {
        public List<Transaction> Execute(IDbConnection db)
        {
            var sql = @"SELECT id, desc, amount, datedue, datepayed, datereconciled, account, ledger
                        FROM transactions
                        WHERE datedue IS NOT null
                        AND datepayed IS null
                        ORDER BY datedue ASC";
            return db.Query<Transaction>(sql).ToList();
        }
    }
}