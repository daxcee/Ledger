using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace Ledger.Models
{
    public class Repository
    {
        string _connectionString;

        public Repository()
        {
            _connectionString = @"Data Source=" + ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public List<Transaction> GetUnreconciled(int? ledger)
        {
            var sql = @"SELECT id, desc, amount, datedue, datepayed, datereconciled, account, ledger
                        FROM transactions
                        WHERE datereconciled IS null
                        AND datepayed IS NOT null";
            if(ledger.HasValue)
                sql += " AND ledger = @ledger";
            var list = new List<Transaction>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var sqlCommand = new SQLiteCommand(sql, conn);
                if(ledger.HasValue)
                    sqlCommand.Parameters.AddWithValue("ledger", ledger.Value);
                var reader = sqlCommand.ExecuteReader();
                while(reader.Read())
                    list.Add(Transaction.Map(reader));
                conn.Close();
            }
            return list;
        }

        public decimal GetCurrentBalance(int? ledger)
        {
            var sql = "SELECT SUM(amount) FROM transactions WHERE datereconciled is not null ";
            if (ledger.HasValue)
                sql += " AND ledger = @ledger";
            decimal returnValue;
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var sqlCommand = new SQLiteCommand(sql, conn);
                if (ledger.HasValue)
                    sqlCommand.Parameters.AddWithValue("ledger", ledger.Value);
                returnValue = (decimal)Math.Round((double)sqlCommand.ExecuteScalar(),2);
                conn.Close();
            }
            return returnValue;
        }

        public decimal GetActualBalance(int? ledger)
        {
            var sql = "SELECT SUM(amount) FROM transactions WHERE datepayed is not null ";
            if (ledger.HasValue)
                sql += " AND ledger = @ledger";
            decimal returnValue;
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var sqlCommand = new SQLiteCommand(sql, conn);
                if (ledger.HasValue)
                    sqlCommand.Parameters.AddWithValue("ledger", ledger.Value);
                returnValue = (decimal)Math.Round((double)sqlCommand.ExecuteScalar(), 2);
                conn.Close();
            }
            return returnValue;
        }

        public IEnumerable<LedgerEntity> GetAllLedgers()
        {
            var sql = @"SELECT ledger, ledgerdesc
                        FROM ledgers
                        ORDER BY ledgerdesc";
            var list = new List<LedgerEntity>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var sqlCommand = new SQLiteCommand(sql, conn);
                var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                    list.Add(LedgerEntity.Map(reader));
                conn.Close();
            }
            return list;
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            var sql = @"SELECT id, [desc]
                        FROM accounts
                        ORDER BY [desc]";
            var list = new List<Account>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var sqlCommand = new SQLiteCommand(sql, conn);
                var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                    list.Add(Account.Map(reader));
                conn.Close();
            }
            return list;
        }
    }

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

    public class Transaction
    {
        public long Id { get; set; }
        public string Desc { get; set; }
        public decimal Amount { get; set; }
        public DateTime? DateDue { get; set; }
        public DateTime? DatePayed { get; set; }
        public DateTime? DateReconciled { get; set; }
        public long Account { get; set; }
        public long Ledger { get; set; }

        public static Transaction Map(SQLiteDataReader reader)
        {
            var t = new Transaction();
            t.Id = (long) reader["id"];
            t.Desc = (string) reader["Desc"];
            t.Amount = (decimal) reader["Amount"];
            if (reader["datedue"] != DBNull.Value) t.DateDue = (DateTime) reader["datedue"];
            if (reader["datepayed"] != DBNull.Value) t.DateDue = (DateTime) reader["datepayed"];
            if (reader["datereconciled"] != DBNull.Value) t.DateDue = (DateTime) reader["datereconciled"];
            t.Account = (long)reader["account"];
            t.Ledger = (long)reader["ledger"];
            return t;
        }
    }
}