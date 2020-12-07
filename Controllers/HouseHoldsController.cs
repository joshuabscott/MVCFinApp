using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCFinApp.Data;
using MVCFinApp.Data.Enums;
using MVCFinApp.Models;
using static MVCFinApp.Data.ContextSeed;

namespace MVCFinApp.Controllers
{
    public class HouseHoldsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<FAUser> _userManager;
        private readonly SignInManager<FAUser> _signInManager;

        public HouseHoldsController(ApplicationDbContext context, UserManager<FAUser> userManager, SignInManager<FAUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: HouseHolds
        public async Task<IActionResult> Index()
        {
            return View(await _context.HouseHold.ToListAsync());
        }
        //Added to allow members to join household
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            user.HouseHoldId = id;
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.AddToRoleAsync(user, Roles.Member.ToString());
            await _context.SaveChangesAsync();

            
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }
        //added to allow members to leave householld
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Leave()
        {
            var user = await _userManager.GetUserAsync(User);
            user.HouseHoldId = null;
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.AddToRoleAsync(user, Roles.New.ToString());
            await _context.SaveChangesAsync();

            
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }


        // GET: HouseHolds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var houseHold = await _context.HouseHold
                .FirstOrDefaultAsync(m => m.Id == id);
            if (houseHold == null)
            {
                return NotFound();
            }

            return View(houseHold);
        }

        // GET: HouseHolds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HouseHolds/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Greeting,Established")] HouseHold houseHold)
        {
            if (ModelState.IsValid)
            {
                _context.Add(houseHold);
                await _context.SaveChangesAsync();
                //Add
                var user = await _userManager.GetUserAsync(User);
                user.HouseHoldId = houseHold.Id;
                await _context.SaveChangesAsync();
                
                await _userManager.AddToRoleAsync(user, Roles.Head.ToString());
                if (User.IsInRole(Roles.New.ToString()))
                {  
                    await _userManager.RemoveFromRoleAsync(user, Roles.New.ToString());
                }
                
                await _signInManager.SignOutAsync();
                await _signInManager.SignInAsync(user, isPersistent: false);
                //
                return RedirectToAction(nameof(Index));
            }
            return View(houseHold);
        }

        // GET: HouseHolds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var houseHold = await _context.HouseHold.FindAsync(id);
            if (houseHold == null)
            {
                return NotFound();
            }
            return View(houseHold);
        }

        // POST: HouseHolds/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Greeting,Established")] HouseHold houseHold)
        {
            if (id != houseHold.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(houseHold);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HouseHoldExists(houseHold.Id))
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
            return View(houseHold);
        }

        // GET: HouseHolds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var houseHold = await _context.HouseHold
                .FirstOrDefaultAsync(m => m.Id == id);
            if (houseHold == null)
            {
                return NotFound();
            }

            return View(houseHold);
        }

        // POST: HouseHolds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            user.HouseHoldId = null; //not a string, int? instead on HouseHold Model

            var houseHold = await _context.HouseHold.FindAsync(id);
            _context.HouseHold.Remove(houseHold);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HouseHoldExists(int id)
        {
            return _context.HouseHold.Any(e => e.Id == id);
        }
    }
}
