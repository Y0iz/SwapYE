using SwapYE.Models;
using SwapYE.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SwapYE.Controllers
{
    public class HomeController : Controller
    {
         SwapYEEntities db = new SwapYEEntities();
      
        public ActionResult Index()
        {
            var items = db.Items.ToList();
            if (items == null)
            {
                return HttpNotFound();
            }
            return View(items);
        }

        [HttpGet]
        public ActionResult AboutUs()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Search(string search)
        {
            var items = db.Items.Where(x => x.ItemType.TypeNme.StartsWith(search)).ToList();
            return View(items);
        }

    }
}
