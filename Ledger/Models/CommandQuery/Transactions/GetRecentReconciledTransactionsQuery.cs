using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Ledger.Models.Entities;
using Ledger.Models.ViewModels;

namespace Ledger.Models.CommandQuery.Transactions
{
    public class GetRecentReconciledTransactionsQuery : IQuery<List<Transaction>>
    {
        readonly IndexFilterView filter;

        public GetRecentReconciledTransactionsQuery(IndexFilterView filter)
        {
            this.filter = filter;
        }

        public List<Transaction> Execute(IDbConnection db)
        {
            var sql = @"SELECT id, desc, amount, datedue, datepayed, datereconciled, account, ledger
                        FROM transactions
                        WHERE datereconciled IS NOT null
                        AND ((desc LIKE @searchTerm) OR (amount LIKE @searchTerm))
                        AND (@startDate IS NULL OR datereconciled >= @startDate)
                        AND (@endDate IS NULL OR datereconciled <= @endDate)
                        AND (@ledger IS NULL OR ledger = @ledger)
                        ORDER BY datereconciled DESC
                        LIMIT 100";

            return db.Query<Transaction>(sql, new
            {
                searchTerm = "%" + filter.Query + "%",
                startDate = filter.StartDate,
                endDate = filter.EndDate,
                ledger = filter.Ledger
            }).ToList();
        }
    }
}