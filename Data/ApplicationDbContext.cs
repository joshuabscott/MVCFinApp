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
        public DbSet<Attachment> Attachment { get; set; }
        public DbSet<BankAccount> BankAccount { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<CategoryItem> CategoryItem { get; set; }
        public DbSet<HouseHold> HouseHold { get; set; }
        public DbSet<Invitation> Invitation { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<History> History { get; set; }
    }
}
