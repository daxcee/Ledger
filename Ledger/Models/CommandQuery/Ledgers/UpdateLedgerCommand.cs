using System.Data;
using Dapper;
using Ledger.Models.Entities;

namespace Ledger.Models.CommandQuery.Ledgers
{
    public class UpdateLedgerCommand : ICommand
    {
        readonly LedgerEntity ledger;

        public UpdateLedgerCommand(LedgerEntity ledger)
        {
            this.ledger = ledger;
        }

        public void Execute(IDbConnection db)
        {
            db.Execute("UPDATE ledgers SET ledgerdesc = @LedgerDesc, isactive = @IsActive WHERE ledger = @Ledger",
                new { ledger.Ledger, ledger.LedgerDesc, ledger.IsActive });
        }
    }
}