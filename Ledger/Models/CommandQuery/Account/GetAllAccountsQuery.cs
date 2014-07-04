using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;

namespace Ledger.Models.CommandQuery.Account
{
    public class GetAllAccountsQuery : IQuery<List<Entities.Account>>
    {
        public List<Entities.Account> Execute(IDbConnection db)
        {
            return db.Query<Entities.Account>("SELECT id, desc, category, comment FROM accounts").ToList();
        }
    }
}