using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace MVCFinApp.Models
{
    public class FAUser : IdentityUser
    {
        [Required]
        [StringLength(30)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        [NotMapped]
        public string FullName { get { return $"{FirstName} {LastName}"; } }

        public byte[] FileData { get; set; }
        public string FileName { get; set; }

        public int? HouseHoldId { get; set; }//string? no, change to int?
        public HouseHold HouseHold { get; set; }

        public virtual ICollection<BankAccount> BankAccounts { get; set; } = new HashSet<BankAccount>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
    }
}
