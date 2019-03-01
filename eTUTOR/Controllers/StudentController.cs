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
        public ActionResult InfoOfStudent()
        {
            return View();
        }
        public ActionResult listRegistCourse()
        {
            string id = Session["username"].ToString();
            var prt = db.students.FirstOrDefault(x => x.username == id);
            var listCourse = db.sessions.ToList().Where(x => x.status_tutor == 2 && x.student_id == prt.student_id);
            return View(listCourse);
        }
    }
}