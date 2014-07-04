using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Ledger.Models.Entities;

namespace Ledger.Models.ViewModels
{
    public class MonthlyReportView
    {
        [Required]
        public int? Month { get; set; }
        [Required]
        public int? Year { get; set; }
        [Required]
        public long? Ledger { get; set; }




        public List<Transaction> Transactions { get; set; }

        public SelectList Months
        {
            get
            {
                var dict = new Dictionary<int, string>();
                for(int i=1;i<=12;i++)
                    dict.Add(i, new DateTime(1980, i, 1).ToString("MMMM"));
                return new SelectList(dict, "Key", "Value");
            }
        }

        public SelectList Years
        {
            get
            {
                var dict = new Dictionary<int, int>();
                for (int i = DateTime.Now.Year; i >= (DateTime.Now.Year-20); i--)
                    dict.Add(i, i);
                return new SelectList(dict, "Key", "Value");
            }
        }

        public List<Account> Accounts { get; set; }
        public SelectList Ledgers { get; set; }
    }
}