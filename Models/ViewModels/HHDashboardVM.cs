using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCFinApp.Models.ViewModels
{
    public class HHDashboardVM
    {
        public IEnumerable<FAUser> Occupants { get; set; }
        public IEnumerable<BankAccount> Accounts { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}