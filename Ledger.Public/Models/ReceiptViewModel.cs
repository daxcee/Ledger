using System;
using System.ComponentModel.DataAnnotations;

namespace Ledger.Public.Models
{
    public class ReceiptViewModel
    {
        [Required(ErrorMessage = "Required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Required")]
        public decimal? Amount { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name="Date")]
        public DateTime DatePayed { get; set; }

        [NonSerialized] public int NumTransactions;
        public Guid Id { get; set; }
    }
}