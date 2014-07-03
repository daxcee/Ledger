using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using Ledger.Models.Entities;

namespace Ledger.Models.Repositories
{
    public class LedgerRepository
    {
        readonly IDbConnection _connection;

        public LedgerRepository()
        {
            var connectionString = @"Data Source=" + ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _connection = new SQLiteConnection(connectionString);
        }

        public List<LedgerEntity> GetAllLedgers()
        {
            return _connection.Query<LedgerEntity>("SELECT ledger, ledgerdesc, isactive FROM ledgers ORDER BY ledgerdesc").ToList();
        }

        public void CreateLedger(LedgerEntity ledger)
        {
            _connection.Execute("INSERT INTO ledgers (ledgerdesc, isactive) VALUES (@LedgerDesc, @IsActive)",
                new {ledger.LedgerDesc, ledger.IsActive});
        }

        public LedgerEntity GetLedger(long id)
        {
            return _connection.Query<LedgerEntity>("SELECT ledger, ledgerdesc, isactive FROM ledgers WHERE ledger = @id", new { id }).FirstOrDefault();
        }

        public void UpdateLedger(LedgerEntity ledger)
        {
            _connection.Execute("UPDATE ledgers SET ledgerdesc = @LedgerDesc, isactive = @IsActive WHERE ledger = @Ledger",
                new { ledger.Ledger, ledger.LedgerDesc, ledger.IsActive });
        }
    }
}