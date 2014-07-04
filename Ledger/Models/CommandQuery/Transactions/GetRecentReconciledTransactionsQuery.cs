using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Ledger.Models.Entities;

namespace Ledger.Models.CommandQuery.Transactions
{
    public class GetRecentReconciledTransactionsQuery : IQuery<List<Transaction>>
    {
        readonly int numTransactions;

        public GetRecentReconciledTransactionsQuery(int numTransactions)
        {
            this.numTransactions = numTransactions;
        }

        public List<Transaction> Execute(IDbConnection db)
        {
            var sql = @"SELECT id, desc, amount, datedue, datepayed, datereconciled, account, ledger
                        FROM transactions
                        WHERE datereconciled IS NOT null
                        ORDER BY datereconciled DESC
                        LIMIT " + numTransactions;
            return db.Query<Transaction>(sql).ToList();
        }
    }
}