using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCFinApp.Models;

namespace MVCFinApp.Services
{
    public interface IHouseHoldService
    {
        public Task<List<FAUser>> ListHouseHoldMembersAsync(int? houseHoldId);
        public Task<string> GetRoleAsync(FAUser user);
        public List<Transaction> ListTransactions(HouseHold houseHold);
        public Task<string> GetGreetingAsync(FAUser user);
    }
}
