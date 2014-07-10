using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Ledger.Models.Entities;

namespace Ledger.Models.CommandQuery.Ledgers
{
    public class GetAllLedgersQuery : IQuery<List<LedgerEntity>>
    {
        public List<LedgerEntity> Execute(IDbConnection db)
        {
            return db.Query<LedgerEntity>("SELECT ledger, ledgerdesc, isactive FROM ledgers ORDER BY ledgerdesc").ToList();
        }
    }
}