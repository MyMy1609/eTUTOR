using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;

namespace eTUTOR.Controllers
{
    public class ParentController : Controller
    {
        eTUITOREntities db = new eTUITOREntities();
        // GET: Parent
        public ActionResult InfoOfParent()
        {
            var parent_id = int.Parse(Session["UserID"].ToString());
            var info = db.parents.FirstOrDefault(x => x.parent_id == parent_id);
            return View(info);
        }
        public ActionResult listPr()
        {
            var listpr = db.parents.ToList();
            return View(listpr);
        }
        [HttpGet]
        public ActionResult Detail(int id)
        {
            var pr = db.parents.FirstOrDefault(x => x.parent_id == id);
            return View(pr);
        }
        public ActionResult listRegistCourse()
        {
            string id = Session["username"].ToString();
            var prt = db.parents.FirstOrDefault(x => x.username == id);
            var listCourse = db.sessions.ToList().Where(x => x.status_tutor == 2 && x.student_id == prt.parent_id);
            return View(listCourse);
        }
        public ActionResult SessionOfChilds()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateChildAccount(student student)
        {
           
                student.parent_id = int.Parse(Session["UserID"].ToString());
                db.students.Add(student);
                db.SaveChanges();
                return RedirectToAction("InfoOfParent", "Parent", new { id = Session["UserID"] });
        }
    }
}