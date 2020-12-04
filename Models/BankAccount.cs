using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MVCFinApp.Data;

namespace MVCFinApp.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public int HouseHoldId { get; set; }
        public HouseHold HouseHold { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string FAUserId { get; set; }//string instead of int because it will be a Guid
        public FAUser FAUser { get; set; }

        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Name { get; set; }
        [EnumDataType(typeof(AccountType))]
        public AccountType Type { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal StartingBalance { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal CurrentBalance { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
    }
}
