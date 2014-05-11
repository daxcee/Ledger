using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using Ledger.Models.Entities;

namespace Ledger.Models.Repositories
{
    public class TransactionRepository
    {
        readonly IDbConnection _connection;

        public TransactionRepository()
        {
            var connectionString = @"Data Source=" + ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _connection = new SQLiteConnection(connectionString);
        }

        public List<Transaction> GetUnreconciled(int ledger)
        {
            var sql = @"SELECT id, desc, amount, datedue, datepayed, datereconciled, account, ledger
                        FROM transactions
                        WHERE datereconciled IS null
                        AND datepayed IS NOT null
                        AND ledger = @ledger
                        ORDER BY datepayed";
            return _connection.Query<Transaction>(sql, new {ledger}).ToList();
        }

        public decimal GetCurrentBalance(int ledger)
        {
            var sql = "SELECT CAST(COALESCE(SUM(amount),0.0) AS FLOAT) FROM transactions WHERE datereconciled is not null AND ledger = @ledger";
            var sums = _connection.Query<double>(sql, new {ledger});
            var sumAmount = sums.FirstOrDefault();
            return (decimal)Math.Round(sumAmount, 2);
        }

        public decimal GetActualBalance(int ledger)
        {
            var sql = "SELECT CAST(COALESCE(SUM(amount),0.0) AS FLOAT) FROM transactions WHERE datepayed is not null AND ledger = @ledger";
            var sums = _connection.Query<double>(sql, new { ledger });
            var sumAmount = sums.FirstOrDefault();
            return (decimal)Math.Round(sumAmount, 2);
        }

        public IEnumerable<LedgerEntity> GetAllActiveLedgers()
        {
            var sql = @"SELECT ledger, ledgerdesc
                        FROM ledgers
                        WHERE isactive = 1
                        ORDER BY ledgerdesc";
            return _connection.Query<LedgerEntity>(sql);
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            var sql = @"SELECT id, [desc]
                        FROM accounts
                        ORDER BY [desc]";
            return _connection.Query<Account>(sql);
        }

        public void CreateTransaction(Transaction transaction)
        {
            var sql = @"INSERT INTO transactions (desc, amount, datedue, datepayed, datereconciled, account, ledger) VALUES (
                        @Desc, @Amount, @DateDue, @DatePayed, @DateReconciled, @Account, @Ledger)";
            _connection.Execute(sql, new {
                Desc = transaction.Desc,
                Amount = transaction.Amount,
                DateDue = transaction.DateDue,
                DatePayed = transaction.DatePayed,
                DateReconciled = transaction.DateReconciled,
                Account = transaction.Account,
                Ledger = transaction.Ledger
            });
        }

        public void MarkTransactionReconciled(int id, DateTime reconcileDate)
        {
            var sql = @"UPDATE transactions SET
                        DateReconciled = @DateReconciled
                        WHERE id = @id";
            _connection.Execute(sql, new { id, DateReconciled = reconcileDate });
        }

        public void DeleteTransaction(int id)
        {
            var sql = @"DELETE FROM transactions WHERE id = @id";
            _connection.Execute(sql, new {id});
        }

        public Transaction GetTransaction(long id)
        {
            var sql = @"SELECT id, desc, amount, datedue, datepayed, datereconciled, account, ledger
                        FROM transactions
                        WHERE id = @id";
            return _connection.Query<Transaction>(sql, new {id}).FirstOrDefault();
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
            _connection.Execute(sql, transaction);
        }

        public List<Transaction> GetBillsDue()
        {
            var sql = @"SELECT id, desc, amount, datedue, datepayed, datereconciled, account, ledger
                        FROM transactions
                        WHERE datedue IS NOT null
                        AND datepayed IS null
                        ORDER BY datedue ASC";
            return _connection.Query<Transaction>(sql).ToList();
        }

        public void MarkTransactionBillPaid(int id, DateTime paidDate)
        {
            var sql = @"UPDATE transactions SET
                        DatePayed = @DatePayed
                        WHERE id = @id";
            _connection.Execute(sql, new { id, DatePayed = paidDate });
        }

        public List<Transaction> GetRecentReconciledTransations(int numTransactions)
        {
            var sql = @"SELECT id, desc, amount, datedue, datepayed, datereconciled, account, ledger
                        FROM transactions
                        WHERE datereconciled IS NOT null
                        ORDER BY datereconciled DESC
                        LIMIT " + numTransactions;
            return _connection.Query<Transaction>(sql).ToList();
        }
    }
}