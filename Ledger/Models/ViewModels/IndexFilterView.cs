using System;

namespace Ledger.Models.ViewModels
{
    public class IndexFilterView
    {
        public string Query { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Limit { get; set; }
    }
}