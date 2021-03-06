﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCFinApp.Data;
using MVCFinApp.Models;

namespace MVCFinApp.Controllers
{
    [Authorize]
    public class CategoryItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<FAUser> _userManager;

        public CategoryItemsController(ApplicationDbContext context, UserManager<FAUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: CategoryItems
        [Authorize(Roles = "Administrator,Head,Member")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var houseHold = await _context.HouseHold
                .Include(hh => hh.Categories)
                .ThenInclude(c => c.CategoryItems)
                .FirstOrDefaultAsync(hh => hh.Id == user.HouseHoldId);

            var items = houseHold.Categories.SelectMany(c => c.CategoryItems).ToList();
            return View(items);
        }

        // GET: CategoryItems/Details/5
        [Authorize(Roles = "Administrator,Head,Member")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var houseHold = await _context.HouseHold
                .Include(hh => hh.Categories)
                .ThenInclude(c => c.CategoryItems)
                .FirstOrDefaultAsync(hh => hh.Id == user.HouseHoldId);

            var categoryItem = houseHold.Categories.SelectMany(c => c.CategoryItems).FirstOrDefault(m => m.Id == id);
            if (categoryItem == null)
            {
                return NotFound();
            }

            return View(categoryItem);
        }

        // GET: CategoryItems/Create
        [Authorize(Roles = "Administrator,Head,Member")]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            var houseHold = await _context.HouseHold
                .Include(hh => hh.Categories)
                .ThenInclude(c => c.CategoryItems)
                .FirstOrDefaultAsync(hh => hh.Id == user.HouseHoldId);
            ViewData["CategoryId"] = new SelectList(houseHold.Categories, "Id", "Name");
            return View();
        }

        // POST: CategoryItems/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Head,Member")]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,Name,Description,TargetAmount,ActualAmount")] CategoryItem categoryItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard", "HouseHolds");
            }
            var user = await _userManager.GetUserAsync(User);
            var houseHold = await _context.HouseHold
                .Include(hh => hh.Categories)
                .ThenInclude(c => c.CategoryItems)
                .FirstOrDefaultAsync(hh => hh.Id == user.HouseHoldId);
            ViewData["CategoryId"] = new SelectList(houseHold.Categories, "Id", "Id", categoryItem.CategoryId);
            return View(categoryItem);
        }

        // GET: CategoryItems/Edit/5
        [Authorize(Roles = "Administrator,Head,Member")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var houseHold = await _context.HouseHold
                .Include(hh => hh.Categories)
                .ThenInclude(c => c.CategoryItems)
                .FirstOrDefaultAsync(hh => hh.Id == user.HouseHoldId);

            var categoryItem = houseHold.Categories.SelectMany(c => c.CategoryItems).FirstOrDefault(m => m.Id == id);
            if (categoryItem == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(houseHold.Categories, "Id", "Name", categoryItem.CategoryId);
            return View(categoryItem);
        }

        // POST: CategoryItems/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Head,Member")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,Name,Description,TargetAmount,ActualAmount")] CategoryItem categoryItem)
        {
            if (id != categoryItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryItemExists(categoryItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var user = await _userManager.GetUserAsync(User);
            var houseHold = await _context.HouseHold
                .Include(hh => hh.Categories)
                .ThenInclude(c => c.CategoryItems)
                .FirstOrDefaultAsync(hh => hh.Id == user.HouseHoldId);
            ViewData["CategoryId"] = new SelectList(houseHold.Categories, "Id", "Id", categoryItem.CategoryId);
            return View(categoryItem);
        }

        // GET: CategoryItems/Delete/5
        [Authorize(Roles = "Administrator,Head,Member")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var houseHold = await _context.HouseHold
                .Include(hh => hh.Categories)
                .ThenInclude(c => c.CategoryItems)
                .FirstOrDefaultAsync(hh => hh.Id == user.HouseHoldId);

            var categoryItem = houseHold.Categories.SelectMany(c => c.CategoryItems).FirstOrDefault(m => m.Id == id);
            if (categoryItem == null)
            {
                return NotFound();
            }

            return View(categoryItem);
        }

        // POST: CategoryItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Head,Member")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoryItem = await _context.CategoryItem.FindAsync(id);
            _context.CategoryItem.Remove(categoryItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard", "HouseHolds");
        }

        private bool CategoryItemExists(int id)
        {
            return _context.CategoryItem.Any(e => e.Id == id);
        }
    }
}