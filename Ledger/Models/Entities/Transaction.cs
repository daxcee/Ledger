using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SQLite;
using FluentValidation;

namespace Ledger.Models.Entities
{
    public class Transaction
    {
        public long Id { get; set; }
        [Display(Name = "Description")]
        public string Desc { get; set; }
        public decimal Amount { get; set; }
        [Display(Name = "Date Due")]
        [DisplayFormat(ApplyFormatInEditMode =  true, DataFormatString = "{0:M/d/yyyy}")]
        public DateTime? DateDue { get; set; }
        [Display(Name = "Date Paid")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:M/d/yyyy}")]
        public DateTime? DatePayed { get; set; }
        [Display(Name = "Date Reconciled")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:M/d/yyyy}")]
        public DateTime? DateReconciled { get; set; }
        public long Account { get; set; }
        public long Ledger { get; set; }

        public bool IsABillDue()
        {
            return DateDue.HasValue && !DatePayed.HasValue;
        }

        public bool IsUnreconciled()
        {
            return !DateReconciled.HasValue;
        }
    }

    public class TransactionValidation : AbstractValidator<Transaction>
    {
        public TransactionValidation()
        {
            RuleFor(x => x.Desc).NotEmpty();
            RuleFor(x => x.Amount).NotEmpty().NotEqual(0);
            RuleFor(x => x.Id).Must(HaveAtLeastOneDate);
            RuleFor(x => x.Account).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Ledger).NotEmpty().GreaterThan(0);
        }

        bool HaveAtLeastOneDate(Transaction t, long ignore)
        {
            return t.DateDue != null || t.DatePayed != null;
        }
    }
}