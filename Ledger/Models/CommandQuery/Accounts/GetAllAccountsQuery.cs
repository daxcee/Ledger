using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Ledger.Models.Entities;

namespace Ledger.Models.CommandQuery.Accounts
{
    public class GetAllAccountsQuery : IQuery<List<Account>>
    {
        public List<Account> Execute(IDbConnection db)
        {
            return db.Query<Account>("SELECT id, desc, category, comment FROM accounts").ToList();
        }
    }
}