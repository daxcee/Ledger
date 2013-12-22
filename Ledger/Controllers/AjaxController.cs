﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Ledger.Models;
using Ledger.Models.Entities;
using Ledger.Models.Repositories;
using Ledger.Models.ViewModels;

namespace Ledger.Controllers
{
    public class AjaxController : Controller
    {
        readonly TransactionRepository _repo;

        public AjaxController()
        {
            _repo = new TransactionRepository();
        }

        [HttpPost]
        public ActionResult CreateTransaction(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _repo.CreateTransaction(transaction);
                return new HttpStatusCodeResult(HttpStatusCode.Created, "it worked");
            }
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");
        }

        [HttpPost]
        public ActionResult MarkTransactionReconciled(int id, DateTime reconcileDate)
        {
            if (id == 0 || reconcileDate == DateTime.MinValue || reconcileDate == DateTime.MaxValue)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");
            _repo.MarkTransactionReconciled(id, reconcileDate);
            return new HttpStatusCodeResult(HttpStatusCode.Created, "it worked");
        }

        [HttpPost]
        public ActionResult MarkTransactionBillPaid(int id, DateTime paidDate)
        {
            if (id == 0 || paidDate == DateTime.MinValue || paidDate == DateTime.MaxValue)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");
            _repo.MarkTransactionBillPaid(id, paidDate);
            return new HttpStatusCodeResult(HttpStatusCode.Created, "it worked");
        }

        [HttpGet]
        public JsonResult GetCurrentBalance(int ledger)
        {
            return Json(new { CurrentBalance = _repo.GetCurrentBalance(ledger) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteTransaction(int id)
        {
            if (id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");
            _repo.DeleteTransaction(id);
            return new HttpStatusCodeResult(HttpStatusCode.Created, "it worked");
        }

        [HttpGet]
        public PartialViewResult GetEditRow(int id)
        {
            var model = new TransactionEditViewModel();
            model.Transaction = _repo.GetTransaction(id);
            model.LedgerList = new SelectList(_repo.GetAllLedgers(), "Ledger", "LedgerDesc", model.Transaction.Ledger);
            model.AccountsList = new SelectList(_repo.GetAllAccounts(), "Id", "Desc", model.Transaction.Account);
            return PartialView(model);
        }

        [HttpGet]
        public PartialViewResult GetRow(int id)
        {
            var model = new UnreconciledViewModel();

            var t = _repo.GetTransaction(id);
            model.Transactions = new List<Transaction> { t };
            model.LedgerList = new SelectList(_repo.GetAllLedgers(), "Ledger", "LedgerDesc", id);
            model.AccountsList = new SelectList(_repo.GetAllAccounts(), "Id", "Desc");
            model.Ledger = id;

            if (t.IsABillDue())
                return PartialView("GetRowBillsDue", model);
            return PartialView("GetRowUnreconciled", model);
        }

        [HttpPost]
        public ActionResult UpdateTransaction(Transaction transaction)
        {
            if (!ModelState.IsValid || transaction.Id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Errors");

            _repo.UpdateTransaction(transaction);

            var t = _repo.GetTransaction(transaction.Id);
            var model = new UnreconciledViewModel();
            model.Transactions = new List<Transaction> { t };
            model.LedgerList = new SelectList(_repo.GetAllLedgers(), "Ledger", "LedgerDesc", t.Id);
            model.AccountsList = new SelectList(_repo.GetAllAccounts(), "Id", "Desc");
            model.Ledger = t.Id;

            if (t.IsABillDue())
                return PartialView("GetRowBillsDue", model);
            return PartialView("GetRowUnreconciled", model);
        }
    }
}