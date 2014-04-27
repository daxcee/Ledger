using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Ledger.Models.Entities
{
    public class LedgerEntity
    {
        [Display(Name = "ID")]
        public long Ledger { get; set; }
        [Display(Name = "Description")]
        public string LedgerDesc { get; set; }
        [Display(Name = "Active?")]
        public bool IsActive { get; set; }
    }

    public class LedgerEntityValidation : AbstractValidator<LedgerEntity>
    {
        public LedgerEntityValidation()
        {
            RuleFor(x => x.LedgerDesc).NotEmpty();
        }
    }
}