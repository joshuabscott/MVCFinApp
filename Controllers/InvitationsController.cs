﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCFinApp.Data;
using MVCFinApp.Data.Enums;
using MVCFinApp.Models;
using MVCFinApp.Services;

namespace MVCFinApp.Controllers
{
    [Authorize]
    public class InvitationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<FAUser> _signInManager;
        private readonly UserManager<FAUser> _userManager;
        private readonly IAvatarService _fileService;
        private readonly IEmailSender _emailService;

        public InvitationsController(ApplicationDbContext context, SignInManager<FAUser> signInManager, UserManager<FAUser> userManager, IAvatarService fileService, IEmailSender emailService)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _fileService = fileService;
            _emailService = emailService;
        }

        // GET: Invitations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Invitation.Include(i => i.HouseHold);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Invitations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitation = await _context.Invitation
                .Include(i => i.HouseHold)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invitation == null)
            {
                return NotFound();
            }

            return View(invitation);
        }

        // GET: Invitations/Create
        [Authorize(Roles = "Administrator,Head")]
        public IActionResult Create()
        {
            ViewData["HouseHoldId"] = new SelectList(_context.HouseHold, "Id", "Name");
            return View();
        }

        // POST: Invitations/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Head")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,HouseHoldId,Created,Expires,Accepted,EmailTo,Subject,Body,Code")] Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                //first try
                //_context.Add(invitation);
                //await _context.SaveChangesAsync();

                //1. we do not want to send an invite if user is already a member of house hold
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == invitation.EmailTo);
                if (user != null && user.HouseHoldId != null)
                {
                    TempData["Script"] = "CantInvite()";
                    return RedirectToAction("Dashboard", "HouseHolds");
                }

                //2. this will create the record of the invitation
                _context.Add(invitation);
                await _context.SaveChangesAsync();
                
                //3. build the email to be sent as the invitation body
                //var callbackUrl = Url.Action("Accept", "Invitations", new { email = invitation.EmailTo, code = invitation.Code }, protocol: Request.Scheme);
                //string houseHoldName = (await _context.HouseHold.FirstOrDefaultAsync(hh => hh.Id == invitation.HouseHoldId)).Name;
                //var emailBody =
                //    //$"{invitation.Body} <br/><p><h3>Your invited to join the {houseHoldName} household.</h3><br/><a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Click here to accept</a>.";
                //    $"<h3>You are invited to join the <em>{houseHoldName}</em> household.</h3>" +
                //    $"<h6>{invitation.Body}</h6><br/>" +
                //    $"<a href='{HtmlEncoder.Default.Encode(acceptUrl)}'>Accept</a>" +
                //    $" Or " +
                //    $"<a href='{HtmlEncoder.Default.Encode(declineUrl)}'> Deny</a>.";

                //4. send the email to invite the user to become a member
                //await _emailService.SendEmailAsync(invitation.EmailTo, invitation.Subject, emailBody);

                //5. Alert
                TempData["Script"] = "CanInvite()";
                return RedirectToAction("Dashboard", "HouseHolds");
                //return RedirectToAction(nameof(Index));
            }
            ViewData["HouseHoldId"] = new SelectList(_context.HouseHold, "Id", "Name", invitation.HouseHoldId);
            return View(invitation);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Accept(string email, string code)
        {
            // force signed out
            await _signInManager.SignOutAsync();

            // validate invitation to make sure it is not expired 
            var invitation = await _context.Invitation.FirstOrDefaultAsync(i => i.Code.ToString() == code);
            if (invitation == null || invitation.Accepted == true || DateTime.Now > invitation.Expires)
            {
                return NotFound();
            }

            // User is not a member yet
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                TempData["Email"] = email;
                TempData["Code"] = code;
                return View();
            }

            // User is already a member
            invitation.Accepted = true;
            user.HouseHoldId = invitation.HouseHoldId;
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.AddToRoleAsync(user, Roles.Member.ToString());
            await _signInManager.SignInAsync(user, false);
            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard", "HouseHolds");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Accept(string email, string code, string firstName, string lastName, IFormFile avatar, string password)
        {
            var invitation = await _context.Invitation.FirstOrDefaultAsync(i => i.Code.ToString() == code);
            byte[] fileData;
            string fileName;
            if (avatar != null)
            {
                fileData = await _fileService.ConvertFileToByteArrayAsync(avatar);
                fileName = avatar.FileName;
            }
            else
            {
                fileData = await _fileService.GetDefaultAvatarFileBytesAsync();
                fileName = _fileService.GetDefaultAvatarFileName();
            }
            var user = new FAUser
            {
                FirstName = firstName,
                LastName = lastName,
                FileName = fileName,
                FileData = fileData,
                UserName = email,
                Email = email,
                HouseHoldId = invitation.HouseHoldId,
                EmailConfirmed = true
            };
            invitation.Accepted = true;
            var result = await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, Roles.Member.ToString());
            await _signInManager.SignInAsync(user, /*isPersistent:*/ false);
            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard", "HouseHolds");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Decline(string code)
        {
            // ensure signed out
            await _signInManager.SignOutAsync();

            var invitation = await _context.Invitation.FirstOrDefaultAsync(i => i.Code.ToString() == code);
            if (invitation == null || invitation.Accepted == true || DateTime.Now > invitation.Expires)
            {
                return NotFound();
            }
            invitation.Accepted = true;
            await _context.SaveChangesAsync();

            TempData["HouseHoldName"] = (await _context.HouseHold.FirstOrDefaultAsync(hh => hh.Id == invitation.HouseHoldId)).Name;
            return View();
        }

        // GET: Invitations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitation = await _context.Invitation.FindAsync(id);
            if (invitation == null)
            {
                return NotFound();
            }
            ViewData["HouseHoldId"] = new SelectList(_context.HouseHold, "Id", "Name", invitation.HouseHoldId);
            return View(invitation);
        }

        // POST: Invitations/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HouseHoldId,Created,Expires,Accepted,EmailTo,Subject,Body,Code")] Invitation invitation)
        {
            if (id != invitation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invitation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvitationExists(invitation.Id))
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
            ViewData["HouseHoldId"] = new SelectList(_context.HouseHold, "Id", "Name", invitation.HouseHoldId);
            return View(invitation);
        }

        // GET: Invitations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitation = await _context.Invitation
                .Include(i => i.HouseHold)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invitation == null)
            {
                return NotFound();
            }

            return View(invitation);
        }

        // POST: Invitations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invitation = await _context.Invitation.FindAsync(id);
            _context.Invitation.Remove(invitation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvitationExists(int id)
        {
            return _context.Invitation.Any(e => e.Id == id);
        }
    }
}
