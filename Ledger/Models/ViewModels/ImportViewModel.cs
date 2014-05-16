using System.Web.Mvc;

namespace Ledger.Models.ViewModels
{
    public class ImportViewModel
    {
        public string BaseUrl { get; set; }
        public string Token { get; set; }
        public SelectList LedgerList { get; set; }
        public SelectList AccountsList { get; set; }
    }
}