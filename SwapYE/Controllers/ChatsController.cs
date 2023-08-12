using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using SwapYE.Models;
using SwapYE.ViewModels;

namespace SwapYE.Controllers
{
    public class ChatsController : Controller
    {
        
        private SwapYEEntities db = new SwapYEEntities();

        chat vm = new chat();

        //plez dont totch the code it's senetive
               
        public ActionResult Messages(int chat_id = -1, string chatwith = "",int reciverId = -1)
        {
            int x = (int)Session["UserID"];

            var chat7 = db.Chats.Where(c => (c.SenderId == x || c.RecieverId == x) && (c.SenderId == reciverId || c.RecieverId == reciverId)).FirstOrDefault();
            
            var chats1 = db.Chats.Include(c => c.User1).Where(c => c.SenderId == x || c.RecieverId == x).ToList();

            if (chat_id != -1 || chat7 != null)
            {
                var msg = db.Messages.Include(s => s.User).Where(m => m.ChatId == chat_id).ToList();
                vm.msg = msg;
                if(chatwith != "") { Session["chatwith"] = chatwith; } else { Session["chatwith"] = chat7.User1.FirstName; }
                
                Session["count"] = msg.Count();
                if(chat_id != -1) { Session["chatid"] = chat_id; }
                else { Session["chatid"] = chat7.ChatId;
                    var msgs = db.Messages.Include(s => s.User).Where(m => m.ChatId == chat7.ChatId).ToList();
                    vm.msg = msgs;
                }
            }
            else
            {
                if (reciverId != -1)
                {
                    Chat chat = new Chat()
                    {
                        SenderId = x,
                        RecieverId = reciverId,
                        Date_Time = DateTime.Now
                    };
                    db.Chats.Add(chat);
                  
                   
                    db.SaveChanges();
                   
                    
                    Session["chatid"] = chat.ChatId; //db.Chats.Where(c => (c.SenderId == x || c.RecieverId == x) && (c.SenderId == reciverId || c.RecieverId == reciverId)).First().ChatId;
                    var chat_with = db.Users.Find( reciverId);
                    
                    Session["chatwith"] = chat_with.FirstName + " " + chat_with.LastName;
                }
                vm.msg = new List<Models.Message>();
            }
            //var messages = db.Messages.Where(m => m.chatid == chats.chat_id).First();
            vm.chats = chats1;
            return View(vm);

        }


        [HttpPost]
        public ActionResult Create(string Content, int id)
        {
            Models.Message message = new Models.Message()
            {
                Msg = Content,
                UserId = id,
                ChatId = (int)(Session["chatid"]),
                CreatedTime = DateTime.Now
            };
            db.Messages.Add(message);
            db.SaveChanges();
            return RedirectToAction("Messages", new { chat_id = message.ChatId, chatwith = Session["chatwith"].ToString() });
        }

    }
}
