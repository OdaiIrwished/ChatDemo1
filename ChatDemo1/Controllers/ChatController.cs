using ChatDemo1.Data;
using ChatDemo1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatDemo1.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IChatHub chatHub;
        private readonly UserManager<ApplicationUser> userManager;

        public ChatController(ApplicationDbContext context, IChatHub chatHub, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.chatHub = chatHub;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {

            return View();
        }



        [HttpGet]
        public async Task<IActionResult> User_Search(string name)
        {
            var Users = await context.MyUser.Where(x => x.UserName.Contains(name)).ToListAsync();

            if (name == "all")
            {
                Users = await context.MyUser.ToListAsync();

            }
            return Json(Users);

        }


        public async Task<IActionResult> Chat_details(string receiverId)
        {

            var receiver = await userManager.FindByIdAsync(receiverId);
            var Sender = await userManager.GetUserAsync(User);
            byte[] IV = { 0xFE, 0xDC, 0xBA, 0x98, 0x76, 0x54, 0x32, 0x10 };


            var Messages = context.Messages
              .Where(x => (x.receiverId == receiverId && x.SenderId == Sender.Id) || x.SenderId == receiverId && x.receiverId == Sender.Id)
             .ToList().Select(z =>
             {
                 if (z.AlgorithmType == 1)
                 {
                     z.Content = DECAlgorithm.Decrypt(z.Content, z.Key);
                 }
                 else if (z.AlgorithmType == 2)
                 {
                     var dec = _3DEC.Decrypt(z.Content.Split(",").ToList().Select(z => byte.Parse(z)).ToArray(),
                                          z.Key.Split(",").ToList().Select(z => byte.Parse(z)).ToArray(), IV);
                     z.Content = System.Text.Encoding.UTF8.GetString(dec);
                 }

                 return z;
             }).ToList();






            if (!string.IsNullOrWhiteSpace(receiverId))
            {

                return Json(new { receiver = receiver, receiverid = receiver.Id, Sender = Sender, Senderid = Sender.Id, Messages = Messages });
            }
            return null;

        }
    }
}
