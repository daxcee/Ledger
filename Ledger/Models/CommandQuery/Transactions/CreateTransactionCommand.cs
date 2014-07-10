using System.Data;
using Dapper;
using Ledger.Models.Entities;

namespace Ledger.Models.CommandQuery.Transactions
{
    public class CreateTransactionCommand : ICommand
    {
        readonly Transaction transaction;

        public CreateTransactionCommand(Transaction transaction)
        {
            this.transaction = transaction;
        }

        public void Execute(IDbConnection db)
        {
            var sql = @"INSERT INTO transactions (desc, amount, datedue, datepayed, datereconciled, account, ledger) VALUES (
                        @Desc, @Amount, @DateDue, @DatePayed, @DateReconciled, @Account, @Ledger)";
            db.Execute(sql, new
            {
                Desc = transaction.Desc,
                Amount = transaction.Amount,
                DateDue = transaction.DateDue,
                DatePayed = transaction.DatePayed,
                DateReconciled = transaction.DateReconciled,
                Account = transaction.Account,
                Ledger = transaction.Ledger
            });
        }
    }
}