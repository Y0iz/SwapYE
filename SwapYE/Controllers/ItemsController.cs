using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using SwapYE.Models;
using SwapYE.ViewModels;

namespace SwapYE.Controllers
{
    public class ItemsController : Controller
    {
        private SwapYEEntities db = new SwapYEEntities();

        // GET: Items
        //public ActionResult Index()
        //{
        //    var items = db.Items.Include(i => i.City).Include(i => i.ItemType).Include(i => i.User);
        //    return View(items.ToList());
        //}

        // GET: Items/Details/5
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = db.Items.FirstOrDefault(i => i.ItemID == id);
            var comments = db.Comments.Where(i => i.ItemID == item.ItemID).ToList();
            ItemCommNotif itemCommNotif = new ItemCommNotif()
            {
                Comment = new Comment(),
                Comments = comments,
                item = item,
                reportComment = new ReportComment(),
            };
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(itemCommNotif);
        }

        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            var comment = db.Comments.Where(p => p.ItemID == item.ItemID).ToList();

            if (item == null)
            {
                return HttpNotFound();
            }
            db.Comments.RemoveRange(comment);
            db.Items.Remove(item);
            
            db.SaveChanges();
            
            return RedirectPermanent("/User/Profile"); ;
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string ItemName,string Description_1, string Price,string Transaction_type,string Item_State,int CityId,int TypeId, HttpPostedFileBase ProductImg1, HttpPostedFileBase ProductImg2, HttpPostedFileBase ProductImg3, HttpPostedFileBase Recipt)
        {
            try
            {
                Item item1 = new Item();
                Payment payment = new Payment();
     

                if (ProductImg1 != null && ProductImg1.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/Content/UserImg"), Path.GetFileName(ProductImg1.FileName));
                    ProductImg1.SaveAs(path);
                    item1.Image_1 = "~/Content/UserImg/" + Path.GetFileName(ProductImg1.FileName);
                }
                if (ProductImg2 != null && ProductImg1.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/Content/UserImg"), Path.GetFileName(ProductImg2.FileName));
                    ProductImg2.SaveAs(path);
                    item1.Image_2 = "~/Content/UserImg/" + Path.GetFileName(ProductImg2.FileName);
                }
                if (ProductImg3 != null && ProductImg1.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/Content/UserImg"), Path.GetFileName(ProductImg3.FileName));
                    ProductImg3.SaveAs(path);
                    item1.Image_3 = "~/Content/UserImg/" + Path.GetFileName(ProductImg3.FileName);
                }
                if (Recipt != null && ProductImg1.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/Content/UserImg"), Path.GetFileName(Recipt.FileName));
                    Recipt.SaveAs(path);
                    payment.File_1 = "~/Content/UserImg/" + Path.GetFileName(Recipt.FileName);
                }

                    item1.ItemName = ItemName;
                    item1.Description_1 = Description_1;
                    item1.Price = decimal.Parse(Price);
                    item1.Transaction_type = Transaction_type;
                    item1.Item_State = Item_State;
                    item1.CityId = CityId;
                    item1.TypeId = TypeId;
                    item1.UserID = (int)(Session["UserID"]);

                    payment.ApprovalState = "false";
                    payment.ItemID = item1.ItemID;

                    db.Items.Add(item1);
                    db.Payments.Add(payment);
                    db.SaveChanges();
                    return RedirectToAction("Profile", "User");
            }
            catch (Exception )
            {
                return RedirectToAction("Create", "Home", new { errorMessage = "حدثة مشكلة اثناء اضافة المنتج الرجاء المحاولة لاحقاً" });
            }


        }

        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(int id, string ItemName, string Description_1, string Price, string Transaction_type, string Item_State, int CityId, int TypeId, HttpPostedFileBase ProductImg1, HttpPostedFileBase ProductImg2, HttpPostedFileBase ProductImg3)
        {
            try
            {
                Item item1 = db.Items.Find(id);
                if (ProductImg1 != null && ProductImg1.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/Content/UserImg"), Path.GetFileName(ProductImg1.FileName));
                    ProductImg1.SaveAs(path);
                    item1.Image_1 = "~/Content/UserImg/" + Path.GetFileName(ProductImg1.FileName);
                }
                if (ProductImg2 != null && ProductImg2.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/Content/UserImg"), Path.GetFileName(ProductImg2.FileName));
                    ProductImg2.SaveAs(path);
                    item1.Image_2 = "~/Content/UserImg/" + Path.GetFileName(ProductImg2.FileName);
                }
                if (ProductImg3 != null && ProductImg3.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/Content/UserImg"), Path.GetFileName(ProductImg3.FileName));
                    ProductImg3.SaveAs(path);
                    item1.Image_3 = "~/Content/UserImg/" + Path.GetFileName(ProductImg3.FileName);
                }
               

                item1.ItemName = ItemName;
                item1.Description_1 = Description_1;
                item1.Price = decimal.Parse(Price);
                item1.Transaction_type = Transaction_type;
                item1.Item_State = Item_State;
                item1.CityId = CityId;
                item1.TypeId = TypeId;
                item1.UserID = (int)(Session["UserID"]);

                db.Entry(item1).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Profile", "User");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home", new { errorMessage = "حدثة مشكلة اثناء تعديل المنتج الرجاء المحاولة لاحقاً" });
            }
        }

    }
}
