using System.Web.Mvc;
using Ledger.Models.Repositories;
using Ledger.Models.ViewModels;

namespace Ledger.Controllers
{
    public class AccountController : Controller
    {
        readonly AccountRepository _acctRepo;

        public AccountController()
        {
            _acctRepo = new AccountRepository();
        }

        public ViewResult Index()
        {
            var model = new AccountsViewModel();
            model.Accounts = _acctRepo.GetAllAccounts();
            return View(model);
        }
    }
}