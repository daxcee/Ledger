using System;
using System.Linq;
using System.Web.Mvc;
using Ledger.Models.Repositories;
using Ledger.Models.ViewModels;

namespace Ledger.Controllers
{
    public class ReportController : Controller
    {
        readonly TransactionRepository _transRepo;

        public ReportController()
        {
            _transRepo = new TransactionRepository();
        }

        public ViewResult Monthly()
        {
            var model = new MonthlyReportView();
            model.Month = DateTime.Now.AddMonths(-1).Month;
            model.Year = DateTime.Now.AddMonths(-1).Year;
            model.Ledgers = new SelectList(_transRepo.GetAllLedgers(), "Ledger", "LedgerDesc");
            return View(model);
        }

        [HttpPost]
        public ViewResult Monthly(MonthlyReportView args)
        {
            var results = new MonthlyReportView();
            var transactions = _transRepo.GetAllReconciled(args);

            results.Ledgers = new SelectList(_transRepo.GetAllLedgers(), "Ledger", "LedgerDesc");
            results.Accounts = _transRepo.GetAllAccounts().OrderBy(a => a.Desc).ThenBy(a => a.Category).ToList();
            results.Transactions = transactions.OrderBy(t => t.Account).ThenBy(t => t.DateReconciled).ToList();
            return View(results);
        }
    }
}