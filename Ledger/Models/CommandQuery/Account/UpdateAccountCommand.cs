using System.Data;
using Dapper;

namespace Ledger.Models.CommandQuery.Account
{
    public class UpdateAccountCommand : ICommand
    {
        private readonly Entities.Account account;

        public UpdateAccountCommand(Entities.Account account)
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