﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCFinApp.Models
{
    public class HouseHold
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Greeting { get; set; }

        public DateTime Established { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; } = new HashSet<Attachment>();
        public virtual ICollection<Invitation> Invitations { get; set; } = new HashSet<Invitation>();
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();
        public virtual ICollection<BankAccount> BankAccounts { get; set; } = new HashSet<BankAccount>();
        public virtual ICollection<FAUser> FAUsers { get; set; } = new HashSet<FAUser>();
    }
}
