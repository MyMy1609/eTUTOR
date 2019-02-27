using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;
using System.Security.Cryptography;
using System.Text;

namespace eTUTOR.Controllers
{
    public class UserController : Controller
    {
        eTUITOREntities model = new eTUITOREntities();
        MD5 md = MD5.Create();
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
            if (certificate !=null && certificate.ContentLength >0)
            {
                tutor.certificate = certificate.FileName;
            }
           
            byte[] inputBytes = Encoding.ASCII.GetBytes(password);
            byte[] hash = md.ComputeHash(inputBytes);
            StringBuilder sbHash = new StringBuilder();
            foreach (byte b in hash)
            {
                sbHash.Append(String.Format("{0:x2}", b));
            }
            tutor.password = sbHash.ToString();

            model.tutors.Add(tutor);
            model.SaveChanges();

            //save certificate

            //create directory
            string AppPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = AppPath + String.Format("App_Data\\certificates\\{0}", tutor.tutor_id);
            DirectoryInfo direc = Directory.CreateDirectory(filePath);

            //save certificate
            if (certificate != null && certificate.ContentLength > 0)
            {
                string fileName = Path.GetFileName(certificate.FileName);
                string path = String.Format("{0}\\{1}",filePath,fileName);
                certificate.SaveAs(path);
            }
            return RedirectToAction("ConfirmEmail","User");
        }
        public ActionResult RegisterStudent(student student, string password)
        {
            student.status = 2;
            byte[] inputBytes = Encoding.ASCII.GetBytes(password);
            byte[] hash = md.ComputeHash(inputBytes);
            StringBuilder sbHash = new StringBuilder();
            foreach (byte b in hash)
            {
                sbHash.Append(String.Format("{0:x2}", b));
            }
            student.password = sbHash.ToString();

            model.students.Add(student);
            model.SaveChanges();
            return RedirectToAction("ConfirmEmail", "User");
        }
        public ActionResult RegisterParent(parent parent, string password)
        {
          
            parent.status = 2;
            byte[] inputBytes = Encoding.ASCII.GetBytes(password);
            byte[] hash = md.ComputeHash(inputBytes);
            StringBuilder sbHash = new StringBuilder();
            foreach (byte b in hash)
            {
                sbHash.Append(String.Format("{0:x2}", b));
            }
            parent.password = sbHash.ToString();

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

                    return RedirectToAction("InfoOfStudent", "Student", new { id = Session["UserID"] });
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