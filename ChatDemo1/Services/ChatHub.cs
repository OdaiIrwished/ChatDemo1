using ChatDemo1.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatDemo1.Services
{
    public class ChatHub : Hub, IChatHub
    {
        private readonly ApplicationDbContext applicationDb;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        //private readonly Creptography creptography;
        private  ApplicationUser ApplicationUser;

        public ChatHub(ApplicationDbContext applicationDb,UserManager<ApplicationUser> userManager,IHttpContextAccessor httpContextAccessor)
        {
            this.applicationDb = applicationDb;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            ApplicationUser =  userManager.GetUserAsync(httpContextAccessor.HttpContext.User).Result;
        }
        public async Task SendMessage(string message, string receiverId)
        {
            await Clients.User(receiverId).SendAsync("receiveMessage",await EncryptAsync(message, receiverId), receiverId);

        }
        public async Task<string> EncryptAsync(string plaintext,string receiverId)
        {
            var Key = RandomText();
            var cypherText = DECAlgorithm.Encrypt(plaintext, Key);
            if (cypherText != null && receiverId != null)
            {
                var msg = new Data.Message { Content = cypherText,Key = Key, receiverId = receiverId, SenderId = ApplicationUser.Id, Send_Time = DateTime.UtcNow };
                applicationDb.Messages.Add(msg);
                await applicationDb.SaveChangesAsync();
            }
            return cypherText;
        }
        public string Decrypt(string ciphertext)
        {
            var Key = applicationDb.Messages.Where(x=> x.Content == ciphertext).First();
            var res = DECAlgorithm.Decrypt(Key.Content, Key.Key);
            return res;
        }
        private string RandomText()
        {
            int length = 9;

            // Create a new instance of the Random class
            Random random = new Random();

            // Create a string of all possible characters
            string characters = "abcdefghijklmnopqrstuvwxyz0123456789";

            // Generate a random string by selecting characters from the string of possible characters
          return  new string(
                Enumerable.Repeat(characters, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
        }
    }
    //another algorithim


   
}

