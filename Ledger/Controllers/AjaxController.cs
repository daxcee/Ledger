using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Ledger.Models;
using Ledger.Models.CommandQuery.Accounts;
using Ledger.Models.CommandQuery.Ledgers;
using Ledger.Models.CommandQuery.Transactions;
using Ledger.Models.Entities;
using Ledger.Models.ViewModels;

namespace Ledger.Controllers
{
    public class AjaxController : Controller
    {
        readonly IDatabase _db;

        public AjaxController(IDatabase db)
        {
            _db = db;
        }

        [HttpPost]
        public ActionResult CreateTransaction(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _db.Execute(new CreateTransactionCommand(transaction));
                return new HttpStatusCodeResult(HttpStatusCode.Created, "it worked");
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");
        }
        
        [HttpPost]
        public ActionResult CreateLedger(LedgerEntity ledger)
        {
            if (ModelState.IsValid)
            {
                _db.Execute(new CreateLedgerCommand(ledger));
                return new HttpStatusCodeResult(HttpStatusCode.Created, "it worked");
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");
        }

        [HttpPost]
        public ActionResult CreateAccount(Account acct)
        {
            if (ModelState.IsValid)
            {
                _db.Execute(new CreateAccountCommand(acct));
                return new HttpStatusCodeResult(HttpStatusCode.Created, "it worked");
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");
        }

        [HttpPost]
        public ActionResult MarkTransactionReconciled(int id, DateTime reconcileDate)
        {
            if (id == 0 || reconcileDate == DateTime.MinValue || reconcileDate == DateTime.MaxValue)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");
            _db.Execute(new MarkTransactionReconciledCommand(id, reconcileDate));
            return new HttpStatusCodeResult(HttpStatusCode.Created, "it worked");
        }

        [HttpPost]
        public ActionResult MarkTransactionBillPaid(int id, DateTime paidDate)
        {
            if (id == 0 || paidDate == DateTime.MinValue || paidDate == DateTime.MaxValue)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");
            _db.Execute(new MarkBillPaidCommand(id, paidDate));
            return new HttpStatusCodeResult(HttpStatusCode.Created, "it worked");
        }

        [HttpGet]
        public JsonResult GetCurrentBalance(int ledger)
        {
            return Json(new { CurrentBalance = _db.Execute(new GetCurrentBalanceForLedgerQuery(ledger)) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteTransaction(int id)
        {
            if (id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");
            _db.Execute(new DeleteTransactionCommand(id));
            return new HttpStatusCodeResult(HttpStatusCode.Created, "it worked");
        }

        [HttpGet]
        public PartialViewResult GetEditRow(int id)
        {
            var model = new TransactionEditViewModel();
            model.Transaction = _db.Execute(new GetTransactionByIdQuery(id));
            
            // list all active ledgers AND the transactions current ledger (in case it's not active)
            var ledgers = _db.Execute(new GetAllLedgersQuery()).Where(l => l.IsActive || l.Ledger == model.Transaction.Ledger);

            model.LedgerList = new SelectList(ledgers, "Ledger", "LedgerDesc", model.Transaction.Ledger);
            model.AccountsList = new SelectList(_db.Execute(new GetAllAccountsQuery()), "Id", "Desc", model.Transaction.Account);
            return PartialView(model);
        } 
        
        [HttpGet]
        public PartialViewResult GetLedgerEditRow(int id)
        {
            var ledger = _db.Execute(new GetLedgerByIdQuery(id));
            return PartialView(ledger);
        }
        
        [HttpGet]
        public PartialViewResult GetAccountEditRow(int id)
        {
            var acct = _db.Execute(new GetAccountByIdQuery(id));
            return PartialView(acct);
        }

        [HttpGet]
        public PartialViewResult GetRow(int id)
        {
            var model = new UnreconciledViewModel();

            var t = _db.Execute(new GetTransactionByIdQuery(id));
            model.Transactions = new List<Transaction> { t };
            model.LedgerList = new SelectList(_db.Execute(new GetAllLedgersQuery()), "Ledger", "LedgerDesc", id);
            model.AccountsList = new SelectList(_db.Execute(new GetAllAccountsQuery()), "Id", "Desc");
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
            var ledger = _db.Execute(new GetLedgerByIdQuery(id));
            return PartialView(ledger);
        }

        [HttpGet]
        public PartialViewResult GetAccountRow(int id)
        {
            var acct = _db.Execute(new GetAccountByIdQuery(id));
            return PartialView(acct);
        }

        [HttpPost]
        public ActionResult UpdateTransaction(Transaction transaction)
        {
            if (!ModelState.IsValid || transaction.Id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");

            _db.Execute(new UpdateTransactionCommand(transaction));

            var t = _db.Execute(new GetTransactionByIdQuery(transaction.Id));
            var model = new UnreconciledViewModel();
            model.Transactions = new List<Transaction> { t };
            model.LedgerList = new SelectList(_db.Execute(new GetAllLedgersQuery()), "Ledger", "LedgerDesc", t.Id);
            model.AccountsList = new SelectList(_db.Execute(new GetAllAccountsQuery()), "Id", "Desc");
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

            _db.Execute(new UpdateLedgerCommand(ledgerIn));

            var model = _db.Execute(new GetLedgerByIdQuery(ledgerIn.Ledger));
            return PartialView("GetLedgerRow", model);
        }
        
        [HttpPost]
        public ActionResult UpdateAccount(Account account)
        {
            if (!ModelState.IsValid || account.Id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");

            _db.Execute(new UpdateAccountCommand(account));

            var acct = _db.Execute(new GetAccountByIdQuery(account.Id));
            return PartialView("GetAccountRow", acct);
        }
    }
}