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
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<FAUser> _userManager;

        public TransactionsController(ApplicationDbContext context, UserManager<FAUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Transaction.Include(t => t.BankAccount).Include(t => t.CategoryItem).Include(t => t.FAUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.BankAccount)
                .Include(t => t.CategoryItem)
                .Include(t => t.FAUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewData["BankAccountId"] = new SelectList(_context.BankAccount, "Id", "Name");
            ViewData["CategoryItemId"] = new SelectList(_context.CategoryItem, "Id", "Name");
            ViewData["FAUserId"] = new SelectList(_context.Users, "Id", "FullName");
            return View();
        }

        // POST: Transactions/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryItemId,BankAccountId,FAUserId,Created,Type,Memo,Amount,IsDeleted")] Transaction transaction)
        {
            transaction.FAUserId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BankAccountId"] = new SelectList(_context.BankAccount, "Id", "Id", transaction.BankAccountId);
            ViewData["CategoryItemId"] = new SelectList(_context.CategoryItem, "Id", "Id", transaction.CategoryItemId);
            ViewData["FAUserId"] = new SelectList(_context.Users, "Id", "Id", transaction.FAUserId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["BankAccountId"] = new SelectList(_context.BankAccount, "Id", "Id", transaction.BankAccountId);
            ViewData["CategoryItemId"] = new SelectList(_context.CategoryItem, "Id", "Id", transaction.CategoryItemId);
            ViewData["FAUserId"] = new SelectList(_context.Users, "Id", "Id", transaction.FAUserId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryItemId,BankAccountId,FAUserId,Created,Type,Memo,Amount,IsDeleted")] Transaction transaction)///what is using system transactions??
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
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
            ViewData["BankAccountId"] = new SelectList(_context.BankAccount, "Id", "Id", transaction.BankAccountId);
            ViewData["CategoryItemId"] = new SelectList(_context.CategoryItem, "Id", "Id", transaction.CategoryItemId);
            ViewData["FAUserId"] = new SelectList(_context.Users, "Id", "Id", transaction.FAUserId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.BankAccount)
                .Include(t => t.CategoryItem)
                .Include(t => t.FAUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transaction.Any(e => e.Id == id);
        }
    }
}