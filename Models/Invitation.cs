using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCFinApp.Models
{
    public class Invitation
    {
        public int Id { get; set; }
        public int HouseHoldId { get; set; }
        public HouseHold HouseHold { get; set; }

        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }

        public bool Accepted { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string EmailTo { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Subject { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Body { get; set; }
        //THis is new, first time using in a MVC project---------------------------------------------------------------------
        public Guid Code { get; set; }
    }
}
