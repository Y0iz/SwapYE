using SwapYE.Models;
using SwapYE.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace SwapYE.Controllers
{
    public class CommentsController : Controller
    {

        private SwapYEEntities db = new SwapYEEntities();


        [HttpPost]
        public ActionResult Create(string Content, HttpPostedFileBase image1, int id)
        {
            try
            {
                Comment comment = new Comment();
                if (Content == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                if (image1 != null && image1.ContentLength > 0 )
                {
                    string path = Path.Combine(Server.MapPath("~/Content/UserImg"), Path.GetFileName(image1.FileName));
                    image1.SaveAs(path);
                    comment.Image_1 = "~/Content/UserImg/" + Path.GetFileName(image1.FileName);
                }

                if (Content != null)
                {
                    comment.ItemID = id;
                    comment.SenderId = (int)Session["UserID"];
                    comment.Dateposted = DateTime.Now;
                    comment.Content = Content;
                   

                    db.Comments.Add(comment);
                    db.SaveChanges();
                }
                return RedirectPermanent("/Items/Details/" + id);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home", new { errorMessage = "حدثة مشكلة اثناء اضافة التعليق الرجاء المحاولة لاحقاً" });

            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                Comment comment = db.Comments.Find(id);
                if (comment == null)
                {
                    return HttpNotFound();
                }
                int ItemId = comment.ItemID;
                db.Comments.Remove(comment);
                db.SaveChanges();
                return RedirectPermanent("/Items/Details/" + ItemId);
            }
            catch(Exception) {
                return RedirectToAction("Index", "Home", new { errorMessage = "حدثة مشكلة اثناء حذف التعليق الرجاء المحاولة لاحقاً" });

            }
           
        }
      
    }
}
