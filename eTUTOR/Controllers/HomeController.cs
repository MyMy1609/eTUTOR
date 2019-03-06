using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;

namespace eTUTOR.Controllers
{
    public class HomeController : Controller
    {
        eTUITOREntities model = new eTUITOREntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            var contact = new contact();
            return View(contact);
        }

        [HttpPost]
        public ActionResult Contact(contact contact, string fullname, string phone, string email, string content)
        {
            contact.fullname = fullname;
            contact.phone = phone;
            contact.email = email;
            contact.content = content;
            model.contacts.Add(contact);
            model.SaveChanges();
            return RedirectToAction("Confirm", "Home");
        }

        public ActionResult Confirm()
        {
            return View();
        }
    }
}