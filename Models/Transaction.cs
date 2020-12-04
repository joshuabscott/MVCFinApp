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
    public class Transaction
    {
        public int Id { get; set; }

        public int? CategoryItemId { get; set; }
        public CategoryItem CategoryItem { get; set; }

        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }

        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string FAUserId { get; set; }
        public FAUser FAUser { get; set; }

        public DateTime Created { get; set; }
        public TransactionType Type { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Memo { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        public bool IsDeleted { get; set; }
    }
}