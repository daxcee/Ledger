using System.Web.Mvc;
using Ledger.Models.Repositories;
using Ledger.Models.ViewModels;

namespace Ledger.Controllers
{
    public class LedgerController : Controller
    {
        readonly LedgerRepository _ledgerRepo;

        public LedgerController()
        {
            _ledgerRepo = new LedgerRepository();
        }

        public ViewResult Index()
        {
            var model = new LedgersViewModel();
            model.Ledgers = _ledgerRepo.GetAllLedgers();
            return View(model);
        }
    }
}