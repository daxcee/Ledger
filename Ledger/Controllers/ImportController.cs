using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Ledger.Models;
using Ledger.Models.CommandQuery.Accounts;
using Ledger.Models.CommandQuery.Ledgers;
using Ledger.Models.ViewModels;

namespace Ledger.Controllers
{
    public class ImportController : Controller
    {
        readonly IDatabase _db;

        public ImportController(IDatabase db)
        {
            _db = db;
        }

        public ViewResult Index()
        {
            var model = new ImportViewModel();
            model.BaseUrl = ConfigurationManager.AppSettings["PublicServiceBaseUrl"];
            model.Token = ConfigurationManager.AppSettings["PublicServiceToken"];
            model.LedgerList = new SelectList(_db.Execute(new GetAllLedgersQuery()).Where(l => l.IsActive), "Ledger", "LedgerDesc");
            model.AccountsList = new SelectList(_db.Execute(new GetAllAccountsQuery()), "Id", "Desc");
            return View(model);
        }
    }
}