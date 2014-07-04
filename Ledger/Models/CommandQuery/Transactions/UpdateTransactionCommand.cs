using System.Data;
using Dapper;
using Ledger.Models.Entities;

namespace Ledger.Models.CommandQuery.Transactions
{
    public class UpdateTransactionCommand : ICommand
    {
        readonly Transaction transaction;

        public UpdateTransactionCommand(Transaction transaction)
        {
            this.transaction = transaction;
        }

        public void Execute(IDbConnection db)
        {
            var sql = @"UPDATE transactions SET
                            [desc] = @Desc,
                            amount = @Amount,
                            datedue = @DateDue,
                            datepayed = @DatePayed,
                            datereconciled = @DateReconciled,
                            account = @Account,
                            ledger = @Ledger
                        WHERE id = @Id";
            db.Execute(sql, transaction);
        }
    }
}