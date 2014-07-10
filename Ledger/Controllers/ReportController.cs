using System;
using System.Linq;
using System.Web.Mvc;
using Ledger.Models;
using Ledger.Models.CommandQuery.Accounts;
using Ledger.Models.CommandQuery.Ledgers;
using Ledger.Models.CommandQuery.Transactions;
using Ledger.Models.ViewModels;

namespace Ledger.Controllers
{
    public class ReportController : Controller
    {
        readonly IDatabase _db;

        public ReportController(IDatabase db)
        {
            _db = db;
        }

        public ViewResult Monthly()
        {
            var model = new MonthlyReportView();
            model.Month = DateTime.Now.AddMonths(-1).Month;
            model.Year = DateTime.Now.AddMonths(-1).Year;
            model.Ledgers = new SelectList(_db.Query(new GetAllLedgersQuery()), "Ledger", "LedgerDesc");
            return View(model);
        }

        [HttpPost]
        public ViewResult Monthly(MonthlyReportView filter)
        {
            var results = new MonthlyReportView();
            var transactions = _db.Query(new GetAllReconciledByFilter(filter));

            results.Ledgers = new SelectList(_db.Query(new GetAllLedgersQuery()), "Ledger", "LedgerDesc");
            results.Accounts = _db.Query(new GetAllAccountsQuery()).OrderBy(a => a.Desc).ThenBy(a => a.Category).ToList();
            results.Transactions = transactions.OrderBy(t => t.Account).ThenBy(t => t.DateReconciled).ToList();
            return View(results);
        }
    }
}