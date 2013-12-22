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
            return _connection.Query<LedgerEntity>("SELECT ledger, ledgerdesc FROM ledgers ORDER BY ledgerdesc").ToList();
        }
    }
}