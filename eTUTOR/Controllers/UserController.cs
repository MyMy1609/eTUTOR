using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;
using eTUTOR.Service;
using System.Security.Cryptography;
using System.Text;

namespace eTUTOR.Controllers
{
    public class UserController : Controller
    {
        //===================================================
        eTUITOREntities model = new eTUITOREntities();
        CommonService commonService = new CommonService();
        //===================================================

        [HttpGet]
        public ActionResult Register()
        {
            var user = new tutor();
            return View(user);
        }
        [HttpPost]
        public ActionResult RegisterTuTor(tutor tutor, HttpPostedFileBase certificate, string password)
        {
            //add new tutor
            tutor.status = 2;
            if (certificate != null && certificate.ContentLength > 0)
            {
                tutor.certificate = certificate.FileName;
            }


            tutor.password = commonService.hash(tutor.password);

            model.tutors.Add(tutor);
            model.SaveChanges();

            //save certificate

            //create directory
            string AppPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = AppPath + String.Format("Content\\img\\certificates\\{0}", tutor.tutor_id);
            DirectoryInfo direc = Directory.CreateDirectory(filePath);

            //save certificate
            if (certificate != null && certificate.ContentLength > 0)
            {
                string fileName = Path.GetFileName(certificate.FileName);
                string path = String.Format("{0}\\{1}", filePath, fileName);
                certificate.SaveAs(path);
            }
            return RedirectToAction("ConfirmEmail", "User");
        }
        public ActionResult RegisterStudent(student student, string password)
        {
            student.status = 2;

            student.password = commonService.hash(student.password);

            model.students.Add(student);
            model.SaveChanges();
            return RedirectToAction("ConfirmEmail", "User");
        }
        public ActionResult RegisterParent(parent parent, string password)
        {

            parent.status = 2;

            parent.password = commonService.hash(parent.password);
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
            password = commonService.hash(password);
            if (tutor != null)
            {
                if (tutor.password.Equals(password))
                {
                    Session["FullName"] = tutor.fullname;
                    Session["UserID"] = tutor.tutor_id;
                    Session["username"] = tutor.username;
                    Session["Role"] = "tutor";
                    return RedirectToAction("InfoOfTutor", "Tutor", new { id = Session["UserID"] });
                }
            }
            if (student != null)
            {
                if (student.password.Equals(password))
                {
                    Session["FullName"] = student.fullname;
                    Session["UserID"] = student.student_id;
                    Session["username"] = student.username;
                    Session["Role"] = "student";
                    return RedirectToAction("InfoOfStudent", "Student", new { id = Session["UserID"] });
                }
            }
            if (parent != null)
            {
                if (parent.password.Equals(password))
                {
                    Session["FullName"] = parent.fullname;
                    Session["UserID"] = parent.parent_id;
                    Session["username"] = parent.username;
                    Session["Role"] = "parent";
                    return RedirectToAction("InfoOfParent", "Parent", new { id = Session["UserID"] });
                }
            }
            else
            {
                ViewBag.mgs = "tài khoản không tồn tại";
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("FullName");
            Session.Remove("UserID");
            Session.Remove("Role");
            return RedirectToAction("Login");
        }
        
    }
}