﻿using System.Linq;
using System.Web.Mvc;
using Ledger.Models.Repositories;
using Ledger.Models.ViewModels;

namespace Ledger.Controllers
{
    public class HomeController : Controller
    {
        readonly TransactionRepository _transRepo;
        readonly LedgerRepository _ledgerRepo;

        public HomeController()
        {
            _transRepo = new TransactionRepository();
            _ledgerRepo = new LedgerRepository();
        }

        public ViewResult Index(string q)
        {
            var model = new RecentTransactionsViewModel();
            if (string.IsNullOrEmpty(q))
                model.Transactions = _transRepo.GetRecentReconciledTransations(100);
            else
                model.Transactions = _transRepo.GetTransactionsSearch(q);
            model.SearchTerm = q;
            model.LedgerList = new SelectList(_transRepo.GetAllLedgers(), "Ledger", "LedgerDesc");
            model.AccountsList = new SelectList(_transRepo.GetAllAccounts(), "Id", "Desc");
            return View(model);
        }

        [ChildActionOnly]
        public PartialViewResult Nav()
        {
            var model = new NavViewModel();
            model.Ledgers = _transRepo.GetAllLedgers().Where(l => l.IsActive).ToList();
            var action = ControllerContext.ParentActionViewContext.RouteData.Values["action"]  as string ?? "Index";
            var controller = ControllerContext.ParentActionViewContext.RouteData.Values["controller"]  as string ?? "Home";
            if (action == "Unreconciled")
            {
                var id = (string)RouteData.Values["id"];
                model.SelectedNav = model.Ledgers.Single(l => l.Ledger.ToString() == id).LedgerDesc;
            }
            else if (controller == "Ledger" || controller == "Account" || controller == "Import")
                model.SelectedNav = "Admin";
            else
                model.SelectedNav = action;

            return PartialView(model);
        }

        public ViewResult Unreconciled(int id)
        {
            var model = new UnreconciledViewModel();
            model.Transactions = _transRepo.GetUnreconciled(id);
            model.CurrentBalance = _transRepo.GetCurrentBalance(id);
            model.ActualBalance = _transRepo.GetActualBalance(id);
            model.LedgerList = new SelectList(_transRepo.GetAllLedgers(), "Ledger", "LedgerDesc", id);
            model.AccountsList = new SelectList(_transRepo.GetAllAccounts(), "Id", "Desc");
            model.Ledger = id;
            return View(model);
        } 
        
        public ViewResult BillsDue()
        {
            var model = new UnreconciledViewModel();
            model.Transactions = _transRepo.GetBillsDue();
            model.LedgerList = new SelectList(_transRepo.GetAllLedgers().Where(l => l.IsActive), "Ledger", "LedgerDesc");
            model.AccountsList = new SelectList(_transRepo.GetAllAccounts(), "Id", "Desc");
            return View(model);
        }
    }
}