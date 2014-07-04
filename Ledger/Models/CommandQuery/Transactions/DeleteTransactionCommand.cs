using System.Data;
using Dapper;

namespace Ledger.Models.CommandQuery.Transactions
{
    public class DeleteTransactionCommand : ICommand
    {
        readonly int id;

        public DeleteTransactionCommand(int id)
        {
            this.id = id;
        }

        public void Execute(IDbConnection db)
        {
            var sql = @"DELETE FROM transactions WHERE id = @id";
            db.Execute(sql, new { id });
        }
    }
}