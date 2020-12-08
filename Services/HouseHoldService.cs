using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCFinApp.Data;
using MVCFinApp.Data.Enums;
using MVCFinApp.Models;

namespace MVCFinApp.Services
{
    public class HouseHoldService : IHouseHoldService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<FAUser> _userManager;

        public HouseHoldService(ApplicationDbContext context, UserManager<FAUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<FAUser>> ListHouseHoldMembersAsync(int? houseHoldId)
        {
            var houseHold = await _context.HouseHold.Include(hh => hh.FAUsers).FirstOrDefaultAsync(hh => hh.Id == houseHoldId);
            var members = new List<FAUser>();
            foreach (var member in houseHold.FAUsers)
            {
                var role = (await _userManager.GetRolesAsync(member))[0];
                if (role == Roles.Member.ToString())
                {
                    members.Add(member);
                }
            }
            return members;
        }

        public async Task<string> GetRoleAsync(FAUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles[0];
        }

        public List<Transaction> ListTransactions(HouseHold houseHold)
        {
            var transactions = new List<ICollection<Transaction>>();
            foreach (var bankAccount in houseHold.BankAccounts)
            {
                transactions.Add(bankAccount.Transactions);
            }
            return transactions.SelectMany(t => t).ToList();
        }
    }
}
