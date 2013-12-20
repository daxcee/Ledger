using System.Web.Mvc;
using Ledger.Models;
using Ledger.Models.ViewModels;

namespace Ledger.Controllers
{
    public class HomeController : Controller
    {
        readonly Repository _repo;

        public HomeController()
        {
            _repo = new Repository();
        }

        public ViewResult Index()
        {
            return View();
        }

        public ViewResult Unreconciled(int? id)
        {
            var model = new UnreconciledViewModel();
            model.Transactions = _repo.GetUnreconciled(id);
            model.CurrentBalance = _repo.GetCurrentBalance(id);
            model.ActualBalance = _repo.GetActualBalance(id);
            model.LedgerList = new SelectList(_repo.GetAllLedgers(), "Ledger", "LedgerDesc");
            model.AccountsList = new SelectList(_repo.GetAllAccounts(), "Id", "Desc");
            return View(model);
        } 
        
        public ViewResult BillsDue(int? ledger)
        {
            return View();
        }
    }
}