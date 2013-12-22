using System.Collections.Generic;
using Ledger.Models.Entities;

namespace Ledger.Models.ViewModels
{
    public class IndexViewModel
    {
        public List<LedgerEntity> Ledgers { get; set; }
    }
}