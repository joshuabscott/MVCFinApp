﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCFinApp.Data;
using MVCFinApp.Models;
using MVCFinApp.Models.ViewModels;

namespace MVCFinApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<FAUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<FAUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            string houseHoldName = null;
            if (user.HouseHoldId != null)
            {
                houseHoldName = _context.HouseHold.FirstOrDefault(u => u.Id == user.HouseHoldId).Name;
            }
            var model = new LobbyVM
            {
                Role = (await _userManager.GetRolesAsync(user))[0],
                HouseHold = houseHoldName
            };
            if (user.HouseHoldId != null)
            {
                return RedirectToAction("Dashboard", "HouseHolds");
            }
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}