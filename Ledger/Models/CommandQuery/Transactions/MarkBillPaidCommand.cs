using System;
using System.Data;
using Dapper;

namespace Ledger.Models.CommandQuery.Transactions
{
    public class MarkBillPaidCommand : ICommand
    {
        readonly int id;
        readonly DateTime paidDate;

        public MarkBillPaidCommand(int id, DateTime paidDate)
        {
            this.id = id;
            this.paidDate = paidDate;
        }

        public void Execute(IDbConnection db)
        {
            var sql = @"UPDATE transactions SET
                        DatePayed = @DatePayed
                        WHERE id = @id";
            db.Execute(sql, new { id, DatePayed = paidDate });
        }
    }
}