using System.Configuration;
using System.Web.Mvc;
using Ledger.Models.ViewModels;

namespace Ledger.Controllers
{
    public class ImportController : Controller
    {
        public ViewResult Index()
        {
            var model = new ImportViewModel();
            model.BaseUrl = ConfigurationManager.AppSettings["PublicServiceBaseUrl"];
            model.Token = ConfigurationManager.AppSettings["PublicServiceToken"];
            return View(model);
        }
    }
}