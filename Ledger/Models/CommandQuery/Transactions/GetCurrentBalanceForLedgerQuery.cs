using System;
using System.Data;
using System.Linq;
using Dapper;

namespace Ledger.Models.CommandQuery.Transactions
{
    public class GetCurrentBalanceForLedgerQuery : IQuery<decimal>
    {
        readonly int ledger;

        public GetCurrentBalanceForLedgerQuery(int ledger)
        {
            this.ledger = ledger;
        }

        public decimal Execute(IDbConnection db)
        {
            var sql = "SELECT CAST(COALESCE(SUM(amount),0.0) AS FLOAT) FROM transactions WHERE datereconciled is not null AND ledger = @ledger";
            var sums = db.Query<double>(sql, new { ledger });
            var sumAmount = sums.FirstOrDefault();
            return (decimal)Math.Round(sumAmount, 2);
        }
    }
}