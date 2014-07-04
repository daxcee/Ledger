using System;
using System.Data;
using Dapper;

namespace Ledger.Models.CommandQuery.Transactions
{
    public class MarkTransactionReconciledCommand : ICommand
    {
        readonly int id;
        readonly DateTime reconcileDate;

        public MarkTransactionReconciledCommand(int id, DateTime reconcileDate)
        {
            this.id = id;
            this.reconcileDate = reconcileDate;
        }

        public void Execute(IDbConnection db)
        {
            var sql = @"UPDATE transactions SET
                        DateReconciled = @DateReconciled
                        WHERE id = @id";
            db.Execute(sql, new { id, DateReconciled = reconcileDate });
        }
    }
}