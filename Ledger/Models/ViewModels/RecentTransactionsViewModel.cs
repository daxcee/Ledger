using System.Collections.Generic;
using System.Web.Mvc;
using Ledger.Models.Entities;

namespace Ledger.Models.ViewModels
{
    public class RecentTransactionsViewModel
    {
        public List<Transaction> Transactions { get; set; }
        public SelectList LedgerList { get; set; }
        public SelectList AccountsList { get; set; }
        public string SearchTerm { get; set; }
    }
}