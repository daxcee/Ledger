using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using Ledger.Models.Entities;

namespace Ledger.Models.Repositories
{
    public class AccountRepository
    {
        readonly IDbConnection _connection;

        public AccountRepository()
        {
            var connectionString = @"Data Source=" + ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _connection = new SQLiteConnection(connectionString);
        }

        public List<Account> GetAllAccounts()
        {
            return _connection.Query<Account>("SELECT id, desc, category, comment FROM accounts").ToList();
        }

        public void CreateAccount(Account acct)
        {
            _connection.Execute("INSERT INTO accounts (desc, category, comment) VALUES (@Desc, @Category, @Comment)"
                                , new {acct.Desc, acct.Category, acct.Comment});
        }

        public Account GetAccount(long id)
        {
            return _connection.Query<Account>("SELECT id, desc, category, comment FROM accounts WHERE id = @id", new {id}).FirstOrDefault();
        }

        public void UpdateAccount(Account account)
        {
            _connection.Execute(@"UPDATE accounts SET
                                    desc = @Desc,
                                    category = @Category,
                                    comment = @Comment
                                  WHERE id = @Id", new {account.Id, account.Desc, account.Category, account.Comment});
        }
    }
}