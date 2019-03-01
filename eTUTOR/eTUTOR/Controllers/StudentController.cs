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
        eTUITOREntities model = new eTUITOREntities();
        // GET: Student
        public ActionResult InfoOfStudent( int id)
        {
            var info = model.students.FirstOrDefault(x => x.student_id == id);
            return View(info);
        }
    }
}