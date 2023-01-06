using ChatDemo1.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChatDemo1.Services
{
    public class ChatHub : Hub, IChatHub
    {
        private readonly ApplicationDbContext applicationDb;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        //private readonly Creptography creptography;
        private ApplicationUser ApplicationUser;

        public ChatHub(ApplicationDbContext applicationDb, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.applicationDb = applicationDb;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            ApplicationUser = userManager.GetUserAsync(httpContextAccessor.HttpContext.User).Result;
        }
        public async Task SendMessage(string message, string receiverId, int algorithmType)
        {
            await Clients.User(receiverId).SendAsync("receiveMessage", await EncryptAsync(message, receiverId, algorithmType), receiverId, algorithmType);

        }
        public async Task<string> EncryptAsync(string plaintext, string receiverId, int algorithmType)
        {
            var Key = RandomText();
            var cypherText = "";
            if (algorithmType == 1)
            {
                cypherText = DECAlgorithm.Encrypt(plaintext, Key);

            }
            else if (algorithmType == 2)
            {
                byte[] key = new byte[24];
                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(key);
                }
                var plain = System.Text.Encoding.UTF8.GetBytes(plaintext);
                var decKey = System.Text.Encoding.UTF8.GetBytes(Key);
                byte[] IV = { 0xFE, 0xDC, 0xBA, 0x98, 0x76, 0x54, 0x32, 0x10 };
                cypherText =  string.Join(",", _3DEC.Encrypt(plain, key, IV));
                Key = string.Join(",", key);
            }
            if (cypherText != null && receiverId != null)
            {
                var msg = new Data.Message { AlgorithmType = algorithmType, Content = cypherText, Key = Key, receiverId = receiverId, SenderId = ApplicationUser.Id, Send_Time = DateTime.UtcNow };
                applicationDb.Messages.Add(msg);
                await applicationDb.SaveChangesAsync();
            }
            return cypherText;
        }
        public string Decrypt(string ciphertext, int algorithmType)
        {
            var Key = applicationDb.Messages.Where(x => x.Content == ciphertext).First();
            var res = "";
            if (algorithmType == 1)
            {
                res = DECAlgorithm.Decrypt(Key.Content, Key.Key);
            }
            else if (algorithmType == 2)
            {
                var cipher = ciphertext.Split(",").ToList().Select(z=> byte.Parse(z)).ToArray();
                var decKey = Key.Key.Split(",").ToList().Select(z => byte.Parse(z)).ToArray();
                byte[] IV = { 0xFE, 0xDC, 0xBA, 0x98, 0x76, 0x54, 0x32, 0x10 };
                var decrypted = _3DEC.Decrypt(cipher, decKey, IV);
                res =  System.Text.Encoding.UTF8.GetString(decrypted);

            }
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
            return new string(
                  Enumerable.Repeat(characters, length)
                            .Select(s => s[random.Next(s.Length)])
                            .ToArray());
        }
    }
    //another algorithim



}

