using System.Data;
using System.Linq;
using Dapper;
using Ledger.Models.Entities;

namespace Ledger.Models.CommandQuery.Accounts
{
    public class GetAccountByIdQuery : IQuery<Account>
    {
        readonly long id;

        public GetAccountByIdQuery(long id)
        {
            this.id = id;
        }

        public Account Execute(IDbConnection db)
        {
            return db.Query<Account>("SELECT id, desc, category, comment FROM accounts WHERE id = @id", new { id }).FirstOrDefault();
        }
    }
}