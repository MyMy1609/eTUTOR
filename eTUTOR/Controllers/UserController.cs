using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;

namespace eTUTOR.Controllers
{
    public class UserController : Controller
    {
        eTUITOREntities model = new eTUITOREntities();
        [HttpGet]
        public ActionResult Register()
        {
            var user = new tutor();
            return View(user);
        }
        [HttpPost]
        public ActionResult RegisterTuTor(tutor tutor/*, string fullname, string username, string email, string password, string phone, string address, DateTime birthday, string specialized, string job, string experience, string certificate, int status*/)
        {
           
            model.tutors.Add(tutor);
            model.SaveChanges();
            //gan session
            return RedirectToAction("ConfirmEmail","User");
        }
        public ActionResult RegisterStudent(student student/*, string fullname, string username, string email, string password, string phone, string address, DateTime birthday, string specialized, string job, string experience, string certificate, int status*/)
        {
            
            model.students.Add(student);
            model.SaveChanges();
            return RedirectToAction("ConfirmEmail", "User");
        }
        [HttpPost]
        public ActionResult RegisterParent(parent parent/*, string fullname, string username, string email, string password, string phone, string address, DateTime birthday, string specialized, string job, string experience, string certificate, int status*/)
        {

            model.parents.Add(parent);
            model.SaveChanges();
            return RedirectToAction("ConfirmEmail", "User");
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult ConfirmEmail()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var tutor = model.tutors.FirstOrDefault(x => x.email == email);
            var student = model.students.FirstOrDefault(x => x.username == email);
            var parent = model.parents.FirstOrDefault(x => x.email == email);
            if (tutor != null)
            {
                if (tutor.password.Equals(password))
                {
                    Session["FullName"] = tutor.fullname;
                    Session["UserID"] = tutor.tutor_id;
                    Session["username"] = tutor.username;

                    return RedirectToAction("Index", "Home");
                }
            }
            if (student != null)
            {
                if (student.password.Equals(password))
                {
                    Session["FullName"] = student.fullname;
                    Session["UserID"] = student.student_id;

                    Session["username"] = student.username;
                    return RedirectToAction("Index", "Home");
                }
            }
            if (parent != null)
            {
                if (parent.password.Equals(password))
                {
                    Session["FullName"] = parent.fullname;
                    Session["UserID"] = parent.parent_id;

                    Session["username"] = parent.username;
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.mgs = "tài khoản không tồn tại";
            }
            return View();
        }

        public ActionResult Logout(int id)
        {
            var user = model.tutors.FirstOrDefault(x => x.tutor_id == id);
            if (user != null)
            {
                Session["FullName"] = null;
                Session["UserID"] = null;
            }
            return RedirectToAction("Login");
        }
    }
}