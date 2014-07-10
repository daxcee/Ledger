using System.Data;
using System.Linq;
using Dapper;
using Ledger.Models.Entities;

namespace Ledger.Models.CommandQuery.Ledgers
{
    public class GetLedgerByIdQuery : IQuery<LedgerEntity>
    {
        long id;

        public GetLedgerByIdQuery(long id)
        {
            this.id = id;
        }
        public LedgerEntity Execute(IDbConnection db)
        {
            return db.Query<LedgerEntity>("SELECT ledger, ledgerdesc, isactive FROM ledgers WHERE ledger = @id", new { id }).FirstOrDefault();
        }
    }
}