using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCFinApp.Models;

namespace MVCFinApp.Services
{
    public class IHouseHoldService
    {
        public Task<List<FAUser>> ListHouseHoldMembersAsync(FAUser user);
        public Task<string> GetRoleAsync(FAUser user);
    }
}
