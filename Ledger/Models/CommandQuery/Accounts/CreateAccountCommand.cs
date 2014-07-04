using System.Data;
using Dapper;
using Ledger.Models.Entities;

namespace Ledger.Models.CommandQuery.Accounts
{
    public class CreateAccountCommand : ICommand
    {
        readonly Account acct;

        public CreateAccountCommand(Account acct)
        {
            this.acct = acct;
        }
        public void Execute(IDbConnection db)
        {
            db.Execute("INSERT INTO accounts (desc, category, comment) VALUES (@Desc, @Category, @Comment)"
                                , new { acct.Desc, acct.Category, acct.Comment });
        }
    }
}