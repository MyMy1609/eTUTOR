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
        eTUITOREntities db = new eTUITOREntities();
        // GET: Tutor
        public ActionResult ListOfTutors()
        {
            var listTT = db.tutors.ToList();
            return View(listTT);
        }
        [HttpGet]
        public ActionResult ViewDetailTutor(int id)
        {
            var tatu = db.tutors.FirstOrDefault(x => x.tutor_id == id);
            if (tatu == null)
            {
                return View();
            }
            else
            {
                return View(tatu);
            }
        }
        public ActionResult InfoOfTutor()
        {
            return View();
        }
        public ActionResult RegisterTutor()
        {
            return View();
        }
    }
}