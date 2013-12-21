using System.Web.Mvc;

namespace Ledger.Models.ViewModels
{
    public class TransactionEditViewModel
    {
        public Transaction Transaction { get; set; }
        public SelectList AccountsList { get; set; }
        public SelectList LedgerList { get; set; }
    }
}