using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Ledger.Models.Repositories;
using Ledger.Models.ViewModels;

namespace Ledger.Controllers
{
    public class ImportController : Controller
    {
        readonly TransactionRepository _transRepo;

        public ImportController()
        {
            _transRepo = new TransactionRepository();
        }

        public ViewResult Index()
        {
            var model = new ImportViewModel();
            model.BaseUrl = ConfigurationManager.AppSettings["PublicServiceBaseUrl"];
            model.Token = ConfigurationManager.AppSettings["PublicServiceToken"];
            model.LedgerList = new SelectList(_transRepo.GetAllLedgers().Where(l => l.IsActive), "Ledger", "LedgerDesc");
            model.AccountsList = new SelectList(_transRepo.GetAllAccounts(), "Id", "Desc");
            return View(model);
        }
    }
}