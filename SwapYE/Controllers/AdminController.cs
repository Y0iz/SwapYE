using SwapYE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SwapYE.ViewModels;

namespace SwapYE.Controllers
{
    class c1
    {
        public int n;
    }
    public class AdminController : Controller
    {
        // GET: Admin
        SwapYEEntities db = new SwapYEEntities();

        [HttpPost]
        public ActionResult Index(int Pid, int Iid)
        {
            return Content($"pay id: {Pid} item id {Iid} ");
        }

        public ActionResult Payment()
        {
            var pa = db.Payments.Where(p => p.ApprovalState != "true");
            return View(pa.ToList());
        }

        //not workig
        public ActionResult delete(int Iid)
        {
            var n = db.Items.Find(Iid);
            var comment = db.Comments.Where(p => p.ItemID == Iid).ToList();
            for (int i = 0; i < comment.Count(); i++)
            {
                db.Comments.Remove(comment[i]);
            }
           
            db.Items.Remove(n);

            db.SaveChanges();
            return Redirect("../Admin/Payment");
        }

        public ActionResult Accept(int pid)
        {
            var p = db.Payments.Find(pid);

            p.ApprovalState = "true";

            db.SaveChanges();
            return Redirect("../Admin/Payment");
        }


        public ActionResult Reports()
        {
            var ri = db.ReportItems.ToList();
            var rc = db.ReportComments.ToList();
            var rm = db.ReportChats.ToList();

            Report vm = new Report();
            vm.reitem = ri;
            vm.recomment = rc;
            vm.remsg = rm;
            return View(vm);
        }

        [HttpGet]
        public ActionResult reitem(int Iid, int m)
        {

            if (m == 0)
            {
                var rep = db.ReportItems.Find(Iid);
                db.ReportItems.Remove(rep);
                db.SaveChanges();
                return Redirect("../Admin/Reports");

            }
            else
            {
                var item = db.Items.Find(Iid);
                var comment = db.Comments.Where(i=>i.ItemID == item.ItemID);
                db.Comments.RemoveRange(comment);
                db.Items.Remove(item);
                db.SaveChanges();
                return Redirect("../Admin/Reports");
            }
        }

        public ActionResult recom(int c_id, int m)
        {
            if (m == 0)
            {
                var rep = db.ReportComments.Find(c_id);
                db.ReportComments.Remove(rep);
                db.SaveChanges();
                return Redirect("../Admin/Reports");

            }
            else
            {
                var item = db.Comments.Find(c_id);
                db.Comments.Remove(item);
                db.SaveChanges();
                return Redirect("../Admin/Reports");

            }
        }

        public ActionResult remsg(int m_id, int m)
        {
            if (m == 0)
            {
                var rep = db.ReportChats.Find(m_id);
                db.ReportChats.Remove(rep);
                db.SaveChanges();
                return Redirect("../Admin/Reports");

            }
            else
            {
                var chat = db.Chats.Find(m_id);
                var messages = db.Messages.Where(i => i.ChatId == chat.ChatId);
                db.Messages.RemoveRange(messages);
                db.Chats.Remove(chat);
                db.SaveChanges();
                return Redirect("../Admin/Reports");
            }
        }

        public ActionResult Add_Nptice(int m_id, int m)
        {
            return Content("hi");
        }
    }
}