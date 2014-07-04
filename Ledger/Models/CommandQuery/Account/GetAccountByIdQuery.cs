using System.Data;
using System.Linq;
using Dapper;

namespace Ledger.Models.CommandQuery.Account
{
    public class GetAccountByIdQuery : IQuery<Entities.Account>
    {
        readonly long id;

        public GetAccountByIdQuery(long id)
        {
            this.id = id;
        }

        public Entities.Account Execute(IDbConnection db)
        {
            return db.Query<Entities.Account>("SELECT id, desc, category, comment FROM accounts WHERE id = @id", new { id }).FirstOrDefault();
        }
    }
}