using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using SwapYE.Models;

namespace SwapYE.Controllers
{
    public class ReportsController : Controller
    {
          SwapYEEntities db = new SwapYEEntities();

        //public ActionResult Report_Item(int id)
        //{         
        //  var item = db.Items.First(i => i.ItemID == id);
        //      if (item == null)
        //      {
        //          return RedirectToAction("Index", "Home");
        //      }
        //  ReportItem reportItem = new ReportItem();

        //  reportItem.Description_1 = "بلاغ على المنتج";
        //  reportItem.ItemId = item.ItemID;
        //  reportItem.UserID = (int)(Session["UserID"]);
        //  db.ReportItems.Add(reportItem);
        //  db.SaveChanges();

        //  return RedirectToAction("Index", "Home");
        //}

        [HttpPost]
        public ActionResult create_itemrep(int userid, string text, int itemid)
        {
            if (userid == null) { return Content("the user is not found"); }

            var itemrep = new ReportItem()
            {
                UserID = userid,
                Description_1 = text,
                ItemId = itemid,
            };
            db.ReportItems.Add(itemrep);
            //db.ReportItem.Add(report);
            db.SaveChanges();
            //return View();
            return Redirect("../Home/Index");

        }

        public ActionResult Report_Chat(int id)
        {
            if (id == 0) { return RedirectToAction("Index", "Home"); }
            var chat = db.Chats.First(i => i.ChatId == id);
            if (chat == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ReportChat reportChat = new ReportChat();

            reportChat.Description_1 = "بلاغ على محادثة";
            reportChat.ChatId = chat.ChatId;
            reportChat.UserID = (int)(Session["UserID"]);
            db.ReportChats.Add(reportChat);
            db.SaveChanges();

            return RedirectToAction("Messages", "Chats");
        }

        public ActionResult Report_Comments(int id)
        {
            
            var Comment = db.Comments.First(i => i.CommentId == id);
            if (Comment == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ReportComment reportComment = new ReportComment();

            reportComment.Description_1 = "بلاغ على تعليق";
            reportComment.CommentId = Comment.CommentId;
            reportComment.UserID = (int)(Session["UserID"]);
            db.ReportComments.Add(reportComment);
            db.SaveChanges();

            return RedirectPermanent("/Items/Details/" + Comment.ItemID);
        }
    }
}
