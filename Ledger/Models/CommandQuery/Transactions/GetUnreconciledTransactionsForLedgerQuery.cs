using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Ledger.Models.Entities;

namespace Ledger.Models.CommandQuery.Transactions
{
    public class GetUnreconciledTransactionsForLedgerQuery : IQuery<List<Transaction>>
    {
        readonly int ledger;

        public GetUnreconciledTransactionsForLedgerQuery(int ledger)
        {
            this.ledger = ledger;
        }

        public List<Transaction> Execute(IDbConnection db)
        {
            var sql = @"SELECT id, desc, amount, datedue, datepayed, datereconciled, account, ledger
                        FROM transactions
                        WHERE datereconciled IS null
                        AND datepayed IS NOT null
                        AND ledger = @ledger
                        ORDER BY datepayed";
            return db.Query<Transaction>(sql, new { ledger }).ToList();
        }
    }
}