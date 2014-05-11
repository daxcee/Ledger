using System;
using System.Net;
using System.Web.Mvc;
using Ledger.Public.Models;

namespace Ledger.Public.Controllers
{
    [AuthFilter]
    public class HomeController : Controller
    {
        readonly IReceiptRepository _receiptRepository;

        public HomeController()
        {
            _receiptRepository = new ReceiptRepository();
        }

        public ViewResult Index()
        {
            var defaultReceipt = new ReceiptViewModel();
            defaultReceipt.DatePayed = DateTime.Now.Date;
            defaultReceipt.NumTransactions = _receiptRepository.GetCountTransactionsWaiting();
            return View(defaultReceipt);
        }

        [HttpPost]
        public ActionResult Index(ReceiptViewModel model)
        {
            if (ModelState.IsValid)
            {
                _receiptRepository.Save(model);
                var token = Request.QueryString["token"] ?? "";
                return RedirectToAction("Index", new { token});
            }
            model.NumTransactions = _receiptRepository.GetCountTransactionsWaiting();
            return View(model);
        }

        public JsonResult GetAllReceipts()
        {
            var idList = _receiptRepository.GetAllReceipts();
            return Json(idList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteReceipt(Guid id)
        {
            if(id == Guid.Empty)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No id specified");

            try
            {
                _receiptRepository.DeleteReceipt(id);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}