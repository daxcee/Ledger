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

        public ViewResult Index()
        {
            var model = new IndexViewModel();
            model.Ledgers = _ledgerRepo.GetAllLedgers();
            return View(model);
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
            model.LedgerList = new SelectList(_transRepo.GetAllLedgers(), "Ledger", "LedgerDesc");
            model.AccountsList = new SelectList(_transRepo.GetAllAccounts(), "Id", "Desc");
            return View(model);
        }
    }
}