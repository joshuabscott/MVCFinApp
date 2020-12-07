using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MVCFinApp.Data;
using MVCFinApp.Models;
using MVCFinApp.Extensions;
using MVCFinApp.Services;
using MVCFinApp.Data.Enums;

namespace MVCFinApp.Data
{
    public class ContextSeed
    {
        public static async Task SeedDataBaseAsync(ApplicationDbContext context, UserManager<FAUser> userManager, RoleManager<IdentityRole> roleManager, IAvatarService fileService)
        {
            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager, fileService);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Administrator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Head.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Member.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.New.ToString()));
        }

        private static async Task SeedUsersAsync(UserManager<FAUser> userManager, IAvatarService fileService)
        {
            #region Administrator
            var user = new FAUser();
            user.UserName = "JScott@mailinator.com";
            user.Email = "JScott@mailinator.com";
            user.FirstName = "Josh";
            user.LastName = "Scott";
            user.FileData = await fileService.GetDefaultAvatarFileBytesAsync();
            user.FileName = fileService.GetDefaultAvatarFileName();
            user.EmailConfirmed = true;
            try
            {
                var test = await userManager.FindByEmailAsync(user.Email);
                if (test == null)
                {
                    await userManager.CreateAsync(user, "!1Qwerty");
                    await userManager.AddToRoleAsync(user, Roles.Administrator.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("========= ERROR ============");
                Console.WriteLine($"Error Seeding {user.Email}");
                Console.WriteLine(ex.Message);
                Console.WriteLine("============================");
                throw;
            }
            #endregion

            #region HeadOfHouse
            user = new FAUser
            {
                UserName = "JOsterman@mailinator.com",
                Email = "JOsterman@mailinator.com",
                FirstName = "Jon",
                LastName = "Osterman",
                FileData = await fileService.GetDefaultAvatarFileBytesAsync(),
                FileName = fileService.GetDefaultAvatarFileName(),
                EmailConfirmed = true
            };
            try
            {
                var test = await userManager.FindByEmailAsync(user.Email);
                if (test == null)
                {
                    await userManager.CreateAsync(user, "!1Qwerty");
                    await userManager.AddToRoleAsync(user, Roles.Head.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("========= ERROR ============");
                Console.WriteLine($"Error Seeding {user.Email}");
                Console.WriteLine(ex.Message);
                Console.WriteLine("============================");
                throw;
            }
            #endregion

            #region Member
            user = new FAUser
            {
                UserName = "WKovacs@mailinator.com",
                Email = "WKovacs@mailinator.com",
                FirstName = "Walter",
                LastName = "Kovacs",
                FileData = await fileService.GetDefaultAvatarFileBytesAsync(),
                FileName = fileService.GetDefaultAvatarFileName(),
                EmailConfirmed = true
            };
            try
            {
                var test = await userManager.FindByEmailAsync(user.Email);
                if (test == null)
                {
                    await userManager.CreateAsync(user, "!1Qwerty");
                    await userManager.AddToRoleAsync(user, Roles.Member.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("========= ERROR ============");
                Console.WriteLine($"Error Seeding {user.Email}");
                Console.WriteLine(ex.Message);
                Console.WriteLine("============================");
                throw;
            }
            #endregion

            #region New
            user = new FAUser
            {
                UserName = "AVeidt@mailinator.com",
                Email = "AVeidt@mailinator.com",
                FirstName = "Adrian",
                LastName = "Veidt",
                FileData = await fileService.GetDefaultAvatarFileBytesAsync(),
                FileName = fileService.GetDefaultAvatarFileName(),
                EmailConfirmed = true
            };
            try
            {
                var test = await userManager.FindByEmailAsync(user.Email);
                if (test == null)
                {
                    await userManager.CreateAsync(user, "!1Qwerty");
                    await userManager.AddToRoleAsync(user, Roles.New.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("========= ERROR ============");
                Console.WriteLine($"Error Seeding {user.Email}");
                Console.WriteLine(ex.Message);
                Console.WriteLine("============================");
                throw;
            }
            #endregion
        }
    }
}
