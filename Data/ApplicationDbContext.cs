using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCFinApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVCFinApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<FAUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
