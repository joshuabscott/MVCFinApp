using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCFinApp.Models
{
    public class CategoryItem  //budget item
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(10,2)")]
        public Decimal TargetAmount { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(10,2)")]
        public Decimal ActualAmount { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
    }
}
