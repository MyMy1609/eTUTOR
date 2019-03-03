using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public ActionResult InfoOfTutor(int id)
        {
            var tutor_id = int.Parse(Session["UserID"].ToString());
            var info = db.tutors.FirstOrDefault(x => x.tutor_id == id);
            List<session> sessionList = db.sessions.Where(x => x.tutor_id == tutor_id && x.status_admin == 2 ).ToList();
            ViewData["sessionlist"] = sessionList;
            List<schedule> scheduleList = db.schedules.Where(x => x.tutor_id == tutor_id).ToList();
            ViewData["scheduleList"] = scheduleList;
            return View(info);
        }
        [HttpGet]
        public ActionResult RegisterTutor(int? id)
        {
            var tatu = db.tutors.FirstOrDefault(x => x.tutor_id == id);
            if(tatu == null)
            {
                ViewBag.mes = "Vui lòng chọn Tutor";
                RedirectToAction("ListOfTutors", "Tutor");
            }
            return View(tatu);
        }
        [HttpPost]
        public ActionResult ConfirmScheduleTutor(int idgiasu, int idmonhoc, int[] idschedule)
        {   
            string idst = Session["UserID"].ToString();
            int idStudent = int.Parse(idst);
            string id = Session["username"].ToString();
            var std = db.students.FirstOrDefault(x => x.username == id);
            var prt = db.parents.FirstOrDefault(x => x.username == id);
            if (std != null) {
                foreach(var item in idschedule) {
                    var schh = db.schedules.FirstOrDefault( x=> x.schedule_id == item);
                    session newss = new session();
                    newss.day_otw = schh.day_otw;
                    newss.start_time = schh.start_time;
                    newss.end_time = schh.end_time;
                    newss.@class = "10";
                    newss.student_id = idStudent;
                    newss.tutor_id = idgiasu;
                    newss.total_sessions = 10;
                    newss.subject_id = idmonhoc;
                    newss.status_admin = 2;
                    newss.status_tutor = 2;
                    newss.status_id = 2;
                    db.sessions.Add(newss);
                    db.SaveChanges();
                }
               
                return RedirectToAction("listRegistCourse", "Student");
            }
            if (prt != null)
            {
                foreach (var item in idschedule)
                {
                    var schh = db.schedules.FirstOrDefault(x => x.schedule_id == item);
                    session newss = new session();
                    newss.day_otw = schh.day_otw;
                    newss.start_time = schh.start_time;
                    newss.end_time = schh.end_time;
                    newss.@class = "10";
                    newss.student_id = idStudent;
                    newss.tutor_id = idgiasu;
                    newss.total_sessions = 10;
                    newss.subject_id = idmonhoc;
                    newss.status_admin = 2;
                    newss.status_tutor = 2;
                    newss.status_id = 2;

                    db.sessions.Add(newss);
                    db.SaveChanges();
                }
                return RedirectToAction("listRegistCourse", "Parent");
            }
            return RedirectToAction("Login", "User");

        }
        
        public ActionResult SessionOfTutor()
        {
            return View();
        }

        public ActionResult CreateSchedule(schedule schedule)
        {
            schedule.status = 2;
            schedule.tutor_id = int.Parse(Session["UserID"].ToString());
            db.schedules.Add(schedule);
            db.SaveChanges();
            return RedirectToAction("InfoOfTutor", "Tutor", new { id = Session["UserID"] });
        }

        public ActionResult DeleteSchedule(int id)
        {
            schedule sch = db.schedules.Single(x => x.schedule_id == id);
            db.schedules.Remove(sch);
            db.SaveChanges();
            return RedirectToAction("InfoOfTutor", "Tutor", new { id = Session["UserID"] });
        }

        public ActionResult SearchTutor(string search)
        {
            var tutor = db.tutors.ToList().Where(x => x.fullname.Contains(search) || x.specialized.Contains(search));
            return View(tutor);
        }


        [HttpPost]
        public ActionResult Duyetkhoahoc(int id)
        {
            int asd = id;
            var se = db.sessions.Find(id);
            se.status_tutor = 1;
            db.SaveChanges();
            return RedirectToAction("Duyetkhoahoc");
        }

    }
}