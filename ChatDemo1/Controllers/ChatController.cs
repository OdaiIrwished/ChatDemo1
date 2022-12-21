using ChatDemo1.Data;
using ChatDemo1.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatDemo1.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IChatHub chatHub;
        private readonly UserManager<ApplicationUser> userManager;

        public ChatController(ApplicationDbContext context,IChatHub chatHub, UserManager<ApplicationUser> userManager )
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


            var Messages = await context.Messages.Where(x => ( x.receiverId == receiverId && x.SenderId == Sender.Id )|| x.SenderId == receiverId && x.receiverId == Sender.Id).ToListAsync();

           
         



            if (!string.IsNullOrWhiteSpace(receiverId))
            {

                return Json(new { receiver = receiver, receiverid = receiver.Id, Sender  = Sender, Senderid = Sender.Id, Messages= Messages });
            }
            return null;

        }
    }
}
