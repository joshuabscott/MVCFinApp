using MVCFinApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MVCFinApp.Services
{
    public class INotificationService
    {
        public Task NotifyOverdraft(string userId, BankAccount bankAccount);
        public Task<List<Notification>> GetNotificationsAsync(string userId);
    }
}
