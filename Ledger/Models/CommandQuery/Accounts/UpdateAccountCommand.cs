using System.Data;
using Dapper;
using Ledger.Models.Entities;

namespace Ledger.Models.CommandQuery.Accounts
{
    public class UpdateAccountCommand : ICommand
    {
        private readonly Account account;

        public UpdateAccountCommand(Account account)
        {
            this.account = account;
        }

        public void Execute(IDbConnection db)
        {
            db.Execute(@"UPDATE accounts SET
                                    desc = @Desc,
                                    category = @Category,
                                    comment = @Comment
                                  WHERE id = @Id", new { account.Id, account.Desc, account.Category, account.Comment });
        }
    }
}