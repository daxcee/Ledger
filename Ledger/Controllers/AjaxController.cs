using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Ledger.Models.Entities;
using Ledger.Models.Repositories;
using Ledger.Models.ViewModels;

namespace Ledger.Controllers
{
    public class AjaxController : Controller
    {
        readonly TransactionRepository _transRepo;
        readonly LedgerRepository _ledgerRepo;
        readonly AccountRepository _acctRepo;

        public AjaxController()
        {
            _transRepo = new TransactionRepository();
            _ledgerRepo = new LedgerRepository();
            _acctRepo = new AccountRepository();
        }

        [HttpPost]
        public ActionResult CreateTransaction(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _transRepo.CreateTransaction(transaction);
                return new HttpStatusCodeResult(HttpStatusCode.Created, "it worked");
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");
        }
        
        [HttpPost]
        public ActionResult CreateLedger(LedgerEntity ledger)
        {
            if (ModelState.IsValid)
            {
                _ledgerRepo.CreateLedger(ledger);
                return new HttpStatusCodeResult(HttpStatusCode.Created, "it worked");
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");
        }

        [HttpPost]
        public ActionResult CreateAccount(Account acct)
        {
            if (ModelState.IsValid)
            {
                _acctRepo.CreateAccount(acct);
                return new HttpStatusCodeResult(HttpStatusCode.Created, "it worked");
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");
        }

        [HttpPost]
        public ActionResult MarkTransactionReconciled(int id, DateTime reconcileDate)
        {
            if (id == 0 || reconcileDate == DateTime.MinValue || reconcileDate == DateTime.MaxValue)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");
            _transRepo.MarkTransactionReconciled(id, reconcileDate);
            return new HttpStatusCodeResult(HttpStatusCode.Created, "it worked");
        }

        [HttpPost]
        public ActionResult MarkTransactionBillPaid(int id, DateTime paidDate)
        {
            if (id == 0 || paidDate == DateTime.MinValue || paidDate == DateTime.MaxValue)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");
            _transRepo.MarkTransactionBillPaid(id, paidDate);
            return new HttpStatusCodeResult(HttpStatusCode.Created, "it worked");
        }

        [HttpGet]
        public JsonResult GetCurrentBalance(int ledger)
        {
            return Json(new { CurrentBalance = _transRepo.GetCurrentBalance(ledger) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteTransaction(int id)
        {
            if (id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");
            _transRepo.DeleteTransaction(id);
            return new HttpStatusCodeResult(HttpStatusCode.Created, "it worked");
        }

        [HttpGet]
        public PartialViewResult GetEditRow(int id)
        {
            var model = new TransactionEditViewModel();
            model.Transaction = _transRepo.GetTransaction(id);
            model.LedgerList = new SelectList(_transRepo.GetAllActiveLedgers(), "Ledger", "LedgerDesc", model.Transaction.Ledger);
            model.AccountsList = new SelectList(_transRepo.GetAllAccounts(), "Id", "Desc", model.Transaction.Account);
            return PartialView(model);
        } 
        
        [HttpGet]
        public PartialViewResult GetLedgerEditRow(int id)
        {
            var model = _ledgerRepo.GetLedger(id);
            return PartialView(model);
        }
        
        [HttpGet]
        public PartialViewResult GetAccountEditRow(int id)
        {
            var model = _acctRepo.GetAccount(id);
            return PartialView(model);
        }

        [HttpGet]
        public PartialViewResult GetRow(int id)
        {
            var model = new UnreconciledViewModel();

            var t = _transRepo.GetTransaction(id);
            model.Transactions = new List<Transaction> { t };
            model.LedgerList = new SelectList(_transRepo.GetAllActiveLedgers(), "Ledger", "LedgerDesc", id);
            model.AccountsList = new SelectList(_transRepo.GetAllAccounts(), "Id", "Desc");
            model.Ledger = id;

            if (t.IsABillDue())
                return PartialView("GetRowBillsDue", model);
            if(t.IsUnreconciled())
                return PartialView("GetRowUnreconciled", model);
            return PartialView("GetRowReconciled", model);
        }
        
        [HttpGet]
        public PartialViewResult GetLedgerRow(int id)
        {
            var model = _ledgerRepo.GetLedger(id);
            return PartialView(model);
        }

        [HttpGet]
        public PartialViewResult GetAccountRow(int id)
        {
            var model = _acctRepo.GetAccount(id);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult UpdateTransaction(Transaction transaction)
        {
            if (!ModelState.IsValid || transaction.Id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");

            _transRepo.UpdateTransaction(transaction);

            var t = _transRepo.GetTransaction(transaction.Id);
            var model = new UnreconciledViewModel();
            model.Transactions = new List<Transaction> { t };
            model.LedgerList = new SelectList(_transRepo.GetAllActiveLedgers(), "Ledger", "LedgerDesc", t.Id);
            model.AccountsList = new SelectList(_transRepo.GetAllAccounts(), "Id", "Desc");
            model.Ledger = t.Id;

            if (t.IsABillDue())
                return PartialView("GetRowBillsDue", model);
            if(t.IsUnreconciled())
                return PartialView("GetRowUnreconciled", model);
            return PartialView("GetRowReconciled", model);
        }

        [HttpPost]
        public ActionResult UpdateLedger(int ledger, string ledgerDesc, bool isActive)
        {
            var ledgerIn = new LedgerEntity {Ledger = ledger, LedgerDesc = ledgerDesc, IsActive = isActive};
            if (!ModelState.IsValid || ledgerIn.Ledger <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");

            _ledgerRepo.UpdateLedger(ledgerIn);

            var model = _ledgerRepo.GetLedger(ledgerIn.Ledger);
            return PartialView("GetLedgerRow", model);
        }
        
        [HttpPost]
        public ActionResult UpdateAccount(Account account)
        {
            if (!ModelState.IsValid || account.Id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");

            _acctRepo.UpdateAccount(account);

            var model = _acctRepo.GetAccount(account.Id);
            return PartialView("GetAccountRow", model);
        }
    }
}