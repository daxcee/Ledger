using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Ledger.Models.Entities;
using Ledger.Models.ViewModels;

namespace Ledger.Models.CommandQuery.Transactions
{
    public class GetAllReconciledByFilter : IQuery<List<Transaction>>
    {
        readonly MonthlyReportView view;

        public GetAllReconciledByFilter(MonthlyReportView view)
        {
            this.view = view;
        }

        public List<Transaction> Execute(IDbConnection db)
        {
            var sql = @"SELECT id, desc, amount, datedue, datepayed, datereconciled, account, ledger
                        FROM transactions
                        WHERE datereconciled IS NOT null
                        AND (strftime('%m', datereconciled)+0) = @Month
                        AND (strftime('%Y', datereconciled)+0) = @Year
                        AND ledger = @Ledger";
            return db.Query<Transaction>(sql, view).ToList();
        }
    }
}