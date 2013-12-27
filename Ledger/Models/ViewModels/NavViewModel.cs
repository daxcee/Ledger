using System.Collections.Generic;
using Ledger.Models.Entities;

namespace Ledger.Models.ViewModels
{
    public class NavViewModel
    {
        public List<LedgerEntity> Ledgers { get; set; }
        public string SelectedNav { get; set; }
    }
}