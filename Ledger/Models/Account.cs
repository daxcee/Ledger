using System.Data.SQLite;

namespace Ledger.Models
{
    public class Account
    {
        public long Id { get; set; }
        public string Desc { get; set; }

        public static Account Map(SQLiteDataReader reader)
        {
            var a = new Account();
            a.Id = (long)reader["id"];
            a.Desc = (string)reader["desc"];
            return a;
        }
    }
}