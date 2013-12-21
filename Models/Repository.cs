using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;

namespace Ledger.Models
{
    public class Repository
    {
        readonly string _connectionString;

        public Repository()
        {
            _connectionString = @"Data Source=" + ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public List<Transaction> GetUnreconciled(int ledger)
        {
            var sql = @"SELECT id, desc, amount, datedue, datepayed, datereconciled, account, ledger
                        FROM transactions
                        WHERE datereconciled IS null
                        AND datepayed IS NOT null
                        AND ledger = @ledger";
            var list = new List<Transaction>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var sqlCommand = new SQLiteCommand(sql, conn))
                {
                    sqlCommand.Parameters.AddWithValue("ledger", ledger);
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(Transaction.Map(reader));
                    }
                }
                conn.Close();
            }
            return list;
        }

        public decimal GetCurrentBalance(int ledger)
        {
            var sql = "SELECT SUM(amount) FROM transactions WHERE datereconciled is not null AND ledger = @ledger";
            decimal returnValue;
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var sqlCommand = new SQLiteCommand(sql, conn))
                {
                    sqlCommand.Parameters.AddWithValue("ledger", ledger);
                    returnValue = (decimal) Math.Round((double) sqlCommand.ExecuteScalar(), 2);
                }
                conn.Close();
            }
            return returnValue;
        }

        public decimal GetActualBalance(int ledger)
        {
            var sql = "SELECT SUM(amount) FROM transactions WHERE datepayed is not null AND ledger = @ledger";
            decimal returnValue;
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var sqlCommand = new SQLiteCommand(sql, conn))
                {
                    sqlCommand.Parameters.AddWithValue("ledger", ledger);
                    returnValue = (decimal) Math.Round((double) sqlCommand.ExecuteScalar(), 2);
                }
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
                using (var sqlCommand = new SQLiteCommand(sql, conn))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(LedgerEntity.Map(reader));
                    }
                }
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
                using (var sqlCommand = new SQLiteCommand(sql, conn))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(Account.Map(reader));
                    }
                }
                conn.Close();
            }
            return list;
        }

        public void CreateTransaction(Transaction transaction)
        {
            var sql = @"INSERT INTO transactions (desc, amount, datedue, datepayed, datereconciled, account, ledger) VALUES (
                        @Desc, @Amount, @DateDue, @DatePayed, @DateReconciled, @Account, @Ledger)";
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var sqlCommand = new SQLiteCommand(sql, conn))
                {
                    sqlCommand.Parameters.AddWithValue("Desc", transaction.Desc);
                    sqlCommand.Parameters.AddWithValue("Amount", transaction.Amount);
                    sqlCommand.Parameters.AddWithValue("DateDue", transaction.DateDue);
                    sqlCommand.Parameters.AddWithValue("DatePayed", transaction.DatePayed);
                    sqlCommand.Parameters.AddWithValue("DateReconciled", transaction.DateReconciled);
                    sqlCommand.Parameters.AddWithValue("Account", transaction.Account);
                    sqlCommand.Parameters.AddWithValue("Ledger", transaction.Ledger);
                    sqlCommand.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public void MarkTransactionReconciled(int id, DateTime reconcileDate)
        {
            var sql = @"UPDATE transactions SET
                        DateReconciled = @DateReconciled
                        WHERE id = @id";
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var sqlCommand = new SQLiteCommand(sql, conn))
                {
                    sqlCommand.Parameters.AddWithValue("DateReconciled", reconcileDate);
                    sqlCommand.Parameters.AddWithValue("id", id);
                    sqlCommand.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public void DeleteTransaction(int id)
        {
            var sql = @"DELETE FROM transactions WHERE id = @id";
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var sqlCommand = new SQLiteCommand(sql, conn))
                {
                    sqlCommand.Parameters.AddWithValue("id", id);
                    sqlCommand.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public Transaction GetTransaction(long id)
        {
            var sql = @"SELECT id, desc, amount, datedue, datepayed, datereconciled, account, ledger
                        FROM transactions
                        WHERE id = @id";
            Transaction t;
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var sqlCommand = new SQLiteCommand(sql, conn))
                {
                    sqlCommand.Parameters.AddWithValue("id", id);
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        if (!reader.HasRows)
                            throw new Exception("something went wrong");
                        reader.Read();
                        t = Transaction.Map(reader);
                    }
                }
                conn.Close();
            }
            return t;
        }

        public void UpdateTransaction(Transaction transaction)
        {
            var sql = @"UPDATE transactions SET
                            [desc] = @Desc,
                            amount = @Amount,
                            datedue = @DateDue,
                            datepayed = @DatePayed,
                            datereconciled = @DateReconciled,
                            account = @Account,
                            ledger = @Ledger
                        WHERE id = @Id";
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var sqlCommand = new SQLiteCommand(sql, conn))
                {
                    sqlCommand.Parameters.AddWithValue("Id", transaction.Id);
                    sqlCommand.Parameters.AddWithValue("Desc", transaction.Desc);
                    sqlCommand.Parameters.AddWithValue("Amount", transaction.Amount);
                    sqlCommand.Parameters.AddWithValue("DateDue", transaction.DateDue);
                    sqlCommand.Parameters.AddWithValue("DatePayed", transaction.DatePayed);
                    sqlCommand.Parameters.AddWithValue("DateReconciled", transaction.DateReconciled);
                    sqlCommand.Parameters.AddWithValue("Account", transaction.Account);
                    sqlCommand.Parameters.AddWithValue("Ledger", transaction.Ledger);
                    sqlCommand.ExecuteNonQuery();
                }
                conn.Close();
            }
        }
    }
}