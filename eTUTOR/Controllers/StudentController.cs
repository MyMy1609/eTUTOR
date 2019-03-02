using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;

namespace eTUTOR.Controllers
{
    public class StudentController : Controller
    {
        eTUITOREntities db = new eTUITOREntities();
        // GET: Student
        public ActionResult InfoOfStudent(int id)
        {
            var info = db.students.FirstOrDefault(x => x.student_id == id);
            return View(info);
        }
        public ActionResult listRegistCourse()
        {
            string id = Session["username"].ToString();
            var prt = db.students.FirstOrDefault(x => x.username == id);
            var listCourse = db.sessions.ToList().Where(x => x.status_tutor == 2 && x.student_id == prt.student_id);
            return View(listCourse);
        }
        public ActionResult SessionOfStudent(string p)
        {
            ViewData["IsNewGroup"] = false;
            if (string.IsNullOrWhiteSpace(p))
            {
                //Guid g = Guid.NewGuid();
                //p = Convert.ToBase64String(g.ToByteArray());
                //p = p.Replace("=", "");
                //p = p.Replace("+", "");
                p = "demo";
                ViewData["IsNewGroup"] = true;
                ViewData["url"] = Request.Url.AbsoluteUri.ToString();
            }
            else
            {
                ViewData["url"] = Request.Url.AbsoluteUri.ToString();
            }

            ViewData["GroupName"] = p;
            ViewBag.GroupName = p;

            return View();
        }
    }
}