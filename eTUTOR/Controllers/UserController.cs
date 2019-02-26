using System;
using System.Collections.Generic;
using System.IO;
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
        public ActionResult RegisterTuTor(tutor tutor, HttpPostedFileBase certificate)
        {
            tutor.status = 2;
            model.tutors.Add(tutor);
            model.SaveChanges();

            //save certificate

            //create directory
            string AppPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = AppPath + String.Format("App_Data\\certificates\\{0}", tutor.tutor_id);
            DirectoryInfo direc = Directory.CreateDirectory(filePath);

            //save certificate
            if (certificate != null)
            {
                if (certificate.ContentLength > 0)
                {
                    try
                    {
                        string filename = Path.GetFileName(certificate.FileName);
                        string path = Path.Combine(Server.MapPath(filePath), filename);
                        certificate.SaveAs(path);
                    }
                    catch (Exception)
                    {
                        //throw error here
                    }
                }
            }
            return RedirectToAction("ConfirmEmail","User");
            
        }
        public ActionResult RegisterStudent(student student)
        {
            student.status = 2;
            model.students.Add(student);
            model.SaveChanges();
            return RedirectToAction("ConfirmEmail", "User");
        }
        public ActionResult RegisterParent(parent parent)
        {
            parent.status = 2;
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

                    return RedirectToAction("InfoOfTutor", "Tutor");
                }
            }
            if (student != null)
            {
                if (student.password.Equals(password))
                {
                    Session["FullName"] = student.fullname;
                    Session["UserID"] = student.student_id;

                    return RedirectToAction("InfoOfStudent", "Student");
                }
            }
            if (parent != null)
            {
                if (parent.password.Equals(password))
                {
                    Session["FullName"] = parent.fullname;
                    Session["UserID"] = parent.parent_id;

                    return RedirectToAction("InfoOfParent", "Parent");
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