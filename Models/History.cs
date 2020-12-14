using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCFinApp.Models
{
    public class History
    {
        public int Id { get; set; }
        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }
        public decimal Balance { get; set; }
        public DateTime Date { get; set; }
    }
}
