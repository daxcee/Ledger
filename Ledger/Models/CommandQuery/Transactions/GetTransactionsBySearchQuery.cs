using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Ledger.Models.Entities;

namespace Ledger.Models.CommandQuery.Transactions
{
    public class GetTransactionsBySearchQuery : IQuery<List<Transaction>>
    {
        readonly string searchTerm;

        public GetTransactionsBySearchQuery(string searchTerm)
        {
            this.searchTerm = searchTerm;
        }

        public List<Transaction> Execute(IDbConnection db)
        {
            var sql = @"SELECT id, desc, amount, datedue, datepayed, datereconciled, account, ledger
                        FROM transactions
                        WHERE datereconciled IS NOT null
                        AND ((desc LIKE @searchTerm) OR (amount LIKE @searchTerm))
                        ORDER BY datereconciled DESC";
            return db.Query<Transaction>(sql, new { searchTerm = "%" + searchTerm + "%" }).ToList();
        }
    }
}