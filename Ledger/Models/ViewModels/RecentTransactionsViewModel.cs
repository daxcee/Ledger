using System.Collections.Generic;
using System.Web.Mvc;
using Ledger.Models.CommandQuery.Accounts;
using Ledger.Models.CommandQuery.Ledgers;
using Ledger.Models.CommandQuery.Transactions;
using Ledger.Models.Entities;

namespace Ledger.Models.ViewModels
{
    public class RecentTransactionsViewModel
    {
        public RecentTransactionsViewModel(IndexFilterView view, IDatabase db)
        {
            Transactions = db.Query(new GetRecentReconciledTransactionsQuery(view));

            SearchTerm = view.Query;
            if (view.StartDate.HasValue)
                StartDateInput = view.StartDate.Value.ToShortDateString();
            if (view.EndDate.HasValue)
                EndDateInput = view.EndDate.Value.ToShortDateString();

            LedgerList = new SelectList(db.Query(new GetAllLedgersQuery()), "Ledger", "LedgerDesc");
            AccountsList = new SelectList(db.Query(new GetAllAccountsQuery()), "Id", "Desc");
        }

        public List<Transaction> Transactions { get; private set; }

        public SelectList LedgerList { get; private set; }
        public SelectList AccountsList { get; private set; }

        public string SearchTerm { get; private set; }
        public string StartDateInput { get; private set; }
        public string EndDateInput { get; private set; }
    }
}