using System.Linq;
using System.Web.Mvc;
using Ledger.Models;
using Ledger.Models.CommandQuery.Accounts;
using Ledger.Models.CommandQuery.Ledgers;
using Ledger.Models.CommandQuery.Transactions;
using Ledger.Models.ViewModels;

namespace Ledger.Controllers
{
    public class HomeController : Controller
    {
        readonly IDatabase _db;

        public HomeController(IDatabase db)
        {
            _db = db;
        }

        public ViewResult Index(string q)
        {
            var model = new RecentTransactionsViewModel();
            if (string.IsNullOrEmpty(q))
                model.Transactions = _db.Query(new GetRecentReconciledTransactionsQuery(100));
            else
                model.Transactions = _db.Query(new GetTransactionsBySearchQuery(q));
            model.SearchTerm = q;
            model.LedgerList = new SelectList(_db.Query(new GetAllLedgersQuery()), "Ledger", "LedgerDesc");
            model.AccountsList = new SelectList(_db.Query(new GetAllAccountsQuery()), "Id", "Desc");
            return View(model);
        }

        [ChildActionOnly]
        public PartialViewResult Nav()
        {
            var model = new NavViewModel();
            model.Ledgers = _db.Query(new GetAllLedgersQuery()).Where(l => l.IsActive).ToList();
            var action = ControllerContext.ParentActionViewContext.RouteData.Values["action"]  as string ?? "Index";
            var controller = ControllerContext.ParentActionViewContext.RouteData.Values["controller"]  as string ?? "Home";
            if (action == "Unreconciled")
            {
                var id = (string)RouteData.Values["id"];
                model.SelectedNav = model.Ledgers.Single(l => l.Ledger.ToString() == id).LedgerDesc;
            }
            else if (controller == "Ledger" || controller == "Account" || controller == "Import")
                model.SelectedNav = "Admin";
            else
                model.SelectedNav = action;

            return PartialView(model);
        }

        public ViewResult Unreconciled(int id)
        {
            var model = new UnreconciledViewModel();
            model.Transactions = _db.Query(new GetUnreconciledTransactionsForLedgerQuery(id)).OrderBy(t => t.Amount).ToList();
            model.CurrentBalance = _db.Query(new GetCurrentBalanceForLedgerQuery(id));
            model.ActualBalance = _db.Query(new GetActualBalanceForLedgerQuery(id));
            model.LedgerList = new SelectList(_db.Query(new GetAllLedgersQuery()), "Ledger", "LedgerDesc", id);
            model.AccountsList = new SelectList(_db.Query(new GetAllAccountsQuery()), "Id", "Desc");
            model.Ledger = id;
            return View(model);
        } 
        
        public ViewResult BillsDue()
        {
            var model = new UnreconciledViewModel();
            model.Transactions = _db.Query(new GetBillsDueQuery());
            model.LedgerList = new SelectList(_db.Query(new GetAllLedgersQuery()).Where(l => l.IsActive), "Ledger", "LedgerDesc");
            model.AccountsList = new SelectList(_db.Query(new GetAllAccountsQuery()), "Id", "Desc");
            return View(model);
        }
    }
}