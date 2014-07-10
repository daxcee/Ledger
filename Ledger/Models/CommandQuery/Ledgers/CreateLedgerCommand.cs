using System.Data;
using Dapper;
using Ledger.Models.Entities;

namespace Ledger.Models.CommandQuery.Ledgers
{
    public class CreateLedgerCommand : ICommand
    {
        readonly LedgerEntity ledger;

        public CreateLedgerCommand(LedgerEntity ledger)
        {
            this.ledger = ledger;
        }
        public void Execute(IDbConnection db)
        {
            db.Execute("INSERT INTO ledgers (ledgerdesc, isactive) VALUES (@LedgerDesc, @IsActive)",
                new { ledger.LedgerDesc, ledger.IsActive });
        }
    }
}