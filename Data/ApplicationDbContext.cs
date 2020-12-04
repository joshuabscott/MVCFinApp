using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCFinApp.Models;

namespace MVCFinApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<FAUser> //this needs to be here to fix error, mapping over
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<MVCFinApp.Models.Attachment> Attachment { get; set; }
        public DbSet<MVCFinApp.Models.BankAccount> BankAccount { get; set; }
        public DbSet<MVCFinApp.Models.Category> Category { get; set; }
        public DbSet<MVCFinApp.Models.CategoryItem> CategoryItem { get; set; }
        public DbSet<MVCFinApp.Models.HouseHold> HouseHold { get; set; }
        public DbSet<MVCFinApp.Models.Invitation> Invitation { get; set; }
        public DbSet<MVCFinApp.Models.Notification> Notification { get; set; }
        public DbSet<MVCFinApp.Models.Transaction> Transaction { get; set; }
    }
}
