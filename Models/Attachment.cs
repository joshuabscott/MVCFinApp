using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCFinApp.Models
{
    public class Attachment
    {
        public int Id { get; set; }
        public int HouseHoldId { get; set; }
        public HouseHold HouseHold { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string FileName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Description { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string ContentType { get; set; }

        public byte[] FileData { get; set; }
    }
}
