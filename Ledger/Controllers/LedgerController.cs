using System.Web.Mvc;
using Ledger.Models;
using Ledger.Models.CommandQuery.Ledgers;
using Ledger.Models.ViewModels;

namespace Ledger.Controllers
{
    public class LedgerController : Controller
    {
        readonly IDatabase _db;

        public LedgerController(IDatabase db)
        {
            _db = db;
        }

        public ViewResult Index()
        {
            var model = new LedgersViewModel();
            model.Ledgers = _db.Execute(new GetAllLedgersQuery());
            return View(model);
        }
    }
}