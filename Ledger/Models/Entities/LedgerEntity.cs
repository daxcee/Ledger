using System.Data.SQLite;

namespace Ledger.Models.Entities
{
    public class LedgerEntity
    {
        public long Ledger { get; set; }
        public string LedgerDesc { get; set; }

        public static LedgerEntity Map(SQLiteDataReader reader)
        {
            var l = new LedgerEntity();
            l.Ledger = (long)reader["ledger"];
            l.LedgerDesc = (string)reader["ledgerdesc"];
            return l;
        }
    }
}