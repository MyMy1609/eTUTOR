using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;
namespace eTUTOR.Controllers
{
    public class TutorController : Controller
    {
        eTUITOREntities tutor = new eTUITOREntities();
        // GET: Tutor
        public ActionResult ListOfTutors()
        {
            return View();
        }
        public ActionResult ViewDetailTutor()
        {
            return View();
        }
        public ActionResult InfoOfTutor()
        {
            return View();
        }
        public ActionResult RegisterTutor()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SearchTutor(string search)
        {
            var t = tutor.tutors.ToList().Where(x => x.fullname.Contains(search));
            return View(t);
        }
    }
}