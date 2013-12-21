using System;
using System.Data.SQLite;
using FluentValidation;

namespace Ledger.Models
{
    public class Transaction
    {
        public long Id { get; set; }
        public string Desc { get; set; }
        public decimal Amount { get; set; }
        public DateTime? DateDue { get; set; }
        public DateTime? DatePayed { get; set; }
        public DateTime? DateReconciled { get; set; }
        public long Account { get; set; }
        public long Ledger { get; set; }

        public static Transaction Map(SQLiteDataReader reader)
        {
            var t = new Transaction();
            t.Id = (long) reader["id"];
            t.Desc = (string) reader["Desc"];
            t.Amount = (decimal) reader["Amount"];
            if (reader["datedue"] != DBNull.Value) t.DateDue = (DateTime) reader["datedue"];
            if (reader["datepayed"] != DBNull.Value) t.DatePayed = (DateTime) reader["datepayed"];
            if (reader["datereconciled"] != DBNull.Value) t.DateReconciled = (DateTime) reader["datereconciled"];
            t.Account = (long)reader["account"];
            t.Ledger = (long)reader["ledger"];
            return t;
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