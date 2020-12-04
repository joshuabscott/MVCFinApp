using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCFinApp.Data
{
    public enum TransactionType
    {
        Credit, //Withdrawal - decreases assets or pay out cash for bills, always increases liability. 
        Debit   //Deposit - increases asset or cash in expense accounts & decreases liability. Also known as Revenue
    }
}
