using System.Web.Mvc;
using Ledger.Models;
using Ledger.Models.CommandQuery.Accounts;
using Ledger.Models.ViewModels;

namespace Ledger.Controllers
{
    public class AccountController : Controller
    {
        readonly IDatabase _db;

        public AccountController(IDatabase db)
        {
            _db = db;
        }

        public ViewResult Index()
        {
            var model = new AccountsViewModel();
            model.Accounts = _db.Execute(new GetAllAccountsQuery());
            return View(model);
        }
    }
}