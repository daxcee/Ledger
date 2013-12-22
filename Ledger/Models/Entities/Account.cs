namespace Ledger.Models.Entities
{
    public class Account
    {
        public long Id { get; set; }
        public string Desc { get; set; }
        public string Category { get; set; }
        public string Comment { get; set; }
    }
}