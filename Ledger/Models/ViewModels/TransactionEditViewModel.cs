using System.Web.Mvc;
using Ledger.Models.Entities;

namespace Ledger.Models.ViewModels
{
    public class TransactionEditViewModel
    {
        public Transaction Transaction { get; set; }
        public SelectList AccountsList { get; set; }
        public SelectList LedgerList { get; set; }
    }
}