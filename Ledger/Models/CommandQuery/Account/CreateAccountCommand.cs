using System.Data;
using Dapper;

namespace Ledger.Models.CommandQuery.Account
{
    public class CreateAccountCommand : ICommand
    {
        readonly Entities.Account acct;

        public CreateAccountCommand(Entities.Account acct)
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