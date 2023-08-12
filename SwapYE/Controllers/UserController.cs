using SwapYE.Models;
using SwapYE.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SwapYE.Controllers
{
    public class UserController : Controller
    {
        SwapYEEntities db = new SwapYEEntities();

        [HttpPost]
        public ActionResult Login(User _user)
        {
            if (ModelState.IsValid)
            {
                try {
                    User user = db.Users.First(i => i.UserName == _user.UserName && i.Passwords == _user.Passwords);
                    Session["UserID"] = user.UserID;
                    Session["FirstName"] = user.FirstName;
                    Session["LastName"] = user.LastName;
                    Session["UserName"] = user.UserName;
                    Session["Passwords"] = user.Passwords;
                    Session["Email"] = user.Email;
                    Session["UserType_ID"] = user.UserType_ID;

                    if (user.UserType_ID == 2) { return RedirectToAction("Payment", "Admin"); }

                }
                catch(Exception) {
                    return RedirectToAction("Index", "Home",new { errorMessage ="لا يوجد حساب بأسم المستخدم أو كلمة المرور هذه"});
                }
                return RedirectToAction("Index", "Home", new { success = "Login"});

            }
            return RedirectToAction("Index", "Home", new { fail = "Login" });

        }

        [HttpPost]
        public ActionResult Register(User _user)
        {
            if (_user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
  
                try
                {
                    User user = new User()
                    {
                        FirstName = _user.FirstName,
                        LastName = _user.LastName,
                        Email = _user.Email,
                        UserName = _user.UserName,
                        Passwords = _user.Passwords,
                        Dob = _user.Dob,
                        UserType_ID = 1,
                        Gender = _user.Gender,
                        City = _user.City,
                        street = _user.street,
                        Phone = _user.Phone,
                    };
                    db.Users.Add(user);
                    db.SaveChanges();

                    Session["UserID"] = user.UserID;
                    Session["FirstName"] = user.FirstName;
                    Session["LastName"] = user.LastName;
                    Session["UserName"] = user.UserName;
                    Session["Passwords"] = user.Passwords;
                    Session["Email"] = user.Email;
                    Session["UserType_ID"] = user.UserType_ID;

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception)
                {
                    return RedirectToAction("Index", "Home", new { errorMessage = "حدثة مشكلة اثناء محاولة فتح حساب الرجاء المحاولة لاحقاً" });

                }
            }


        public ActionResult LogOut()
        {
            if (Session["FirstName"] != null)
            {

                Session.RemoveAll();
                return RedirectToAction("Index", "Home");
            }
            return new HttpUnauthorizedResult();
        }


        public ActionResult Profile() 
        
        { 
            var user = db.Users.Find((int)Session["UserID"]);
            if (user == null)
            {
                return HttpNotFound();
            }
            var items = db.Items.Where(m => m.UserID == user.UserID);
            UserItem userItem = new UserItem()
            {
                user = user,
                items = items
            };

            return View(userItem); 
        }

        [HttpPost]
        public ActionResult Edit(int id, string UserName,string Passwords,string FirstName, string LastName,string City,string Gender,string Phone, DateTime Dob,string street,string Email)
        {
            try
            {
                User users = db.Users.Find(id);
                users.UserName = UserName;
                users.Passwords = Passwords;
                users.FirstName = FirstName;
                users.LastName = LastName;
                users.City = City;
                users.Gender = Gender;
                users.Phone = Phone;
                users.Dob = Dob;
                users.street = street;
                users.Email = Email;

                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Profile", "User");
            }
            catch (Exception)
            {

                return RedirectToAction("Profile", "User", new { errorMessage = "حدثة مشكلة اثناء محاولة تعديل الحساب الرجاء المحاولة لاحقاً" });
            }

        }

    }
}