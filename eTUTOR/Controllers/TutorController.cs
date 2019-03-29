using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;
using eTUTOR.Service;
using Newtonsoft.Json.Linq;


namespace eTUTOR.Controllers
{



    public class TutorController : BaseController
    {
        eTUITOREntities db = new eTUITOREntities();
        CommonService commonService = new CommonService();
        // GET: Tutor
        [AllowAnonymous]
        public ActionResult ListOfTutors()
        {

            var listTT = db.tutors.ToList().Where(x => x.status_register == 1);
            return View(listTT);
        }

        [HttpGet]
        public ActionResult ViewDetailTutor(int? id)
        {
            if (Session["Role"] != null)
            {
                ViewBag.typeUser = Session["Role"].ToString();
            }
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
        [Filter.Authorize]
        public ActionResult InfoOfTutor()
        {
            var tutor_id = int.Parse(Session["UserID"].ToString());
            var info = db.tutors.FirstOrDefault(x => x.tutor_id == tutor_id);
            List<session> sessionList = db.sessions.Where(x => x.tutor_id == tutor_id && x.status_admin == 2).ToList();
            ViewData["sessionlist"] = sessionList;
            List<schedule> scheduleList = db.schedules.Where(x => x.tutor_id == tutor_id).ToList();
            ViewData["scheduleList"] = scheduleList;
            return View(info);
        }
        [Filter.Authorize]
        public ActionResult checkSession(int iddd, string types)
        {
            if (types == "student" || types == "tutor")
            {

                return RedirectToAction("RegisterTutor", new { id = iddd , type = types });
            }
            else { 
            
                return RedirectToAction("RegisterTutorParent", new { idd = iddd });
            }
        }
        [Filter.Authorize]
        [HttpGet]
        public ActionResult RegisterTutor(int? id, string type)
        {

            if (Session["Role"].ToString() == "parent")
            {
                return RedirectToAction("RegisterTutorParent", new { idd = id });

            }
            var tatu = db.tutors.FirstOrDefault(x => x.tutor_id == id);
            if (tatu == null)
            {
                setAlert("Vui lòng chọn Tutor", "warning");
                RedirectToAction("ListOfTutors", "Tutor");
            }
            else if (type == "student" || type == "tutor")
            {
                return View(tatu);
            }
            
            return View(tatu);
        }
        [Filter.Authorize]
        [HttpGet]
        public ActionResult RegisterTutorParent(int? idd)
        {
            if (Session["Role"].ToString() != "parent")
            {
                return RedirectToAction("RegisterTutor", new { id = idd });

            }
            var tatu = db.tutors.FirstOrDefault(x => x.tutor_id == idd);
            if (tatu == null)
            {
                setAlert("Vui lòng chọn Tutor", "warning");
                RedirectToAction("ListOfTutors", "Tutor");
            }

            return View(tatu);

        }
        [Filter.Authorize]
        [HttpPost]
        public ActionResult ConfirmScheduleTutor(int idgiasu, int idmonhoc, int[] idschedule)
        {
            string idst = Session["UserID"].ToString();
            int idStudent = int.Parse(idst);
            string id = Session["username"].ToString();
            var std = db.students.FirstOrDefault(x => x.username == id);
            if (std != null)
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
                    newss.dateCreate = DateTime.Now;
                    db.sessions.Add(newss);
                    db.SaveChanges();
                }

                return RedirectToAction("listRegistCourse", "Student");
            }
            return RedirectToAction("Login", "User");

        }
        [Filter.Authorize]
        [HttpPost]
        public ActionResult ConfirmScheduleTutorParent(int idgiasu, int idmonhoc, int[] idschedule, int idSon)
        {

            int idStudent = idSon;
            string id = Session["username"].ToString();
            var std = db.students.FirstOrDefault(x => x.student_id == idStudent);
            if (std != null)
            {
                foreach (var item in idschedule)
                {
                    var schh = db.schedules.FirstOrDefault(x => x.schedule_id == item);
                    session newss = new session();
                    newss.day_otw = schh.day_otw;
                    newss.start_time = schh.start_time;
                    newss.end_time = schh.end_time;
                    newss.@class = std.@class.ToString();
                    newss.student_id = idStudent;
                    newss.tutor_id = idgiasu;
                    newss.total_sessions = 10;
                    newss.subject_id = idmonhoc;
                    newss.status_admin = 2;
                    newss.status_tutor = 2;
                    newss.status_id = 2;
                    newss.dateCreate = DateTime.Now;
                    db.sessions.Add(newss);
                    db.SaveChanges();
                }

                return RedirectToAction("InfoOfParent", "Parent");

            }
            return RedirectToAction("Login", "User");

        }
        [Filter.Authorize]
        public ActionResult SessionOfTutor(string p)
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
                ViewData["url"] = "http://localhost:52781/Student/SessionOfStudent?p=" + p;
            }
            else
            {
                ViewData["url"] = "http://localhost:52781/Student/SessionOfStudent";
            }

            ViewData["GroupName"] = p;
            ViewBag.GroupName = p;

            return View();
        }
        [Filter.Authorize]
        public ActionResult CreateSchedule(schedule schedule)
        {
            schedule.status = 2;
            schedule.dateCreate = DateTime.Now;
            schedule.tutor_id = int.Parse(Session["UserID"].ToString());
            db.schedules.Add(schedule);
            db.SaveChanges();
            return RedirectToAction("InfoOfTutor", "Tutor", new { id = Session["UserID"] });
        }

        [Filter.Authorize]

        public ActionResult DeleteSchedule(int id)
        {
            schedule sch = db.schedules.Single(x => x.schedule_id == id);
            db.schedules.Remove(sch);
            db.SaveChanges();
            return RedirectToAction("InfoOfTutor", "Tutor", new { id = Session["UserID"] });
        }

        public ActionResult SearchTutor(string search)
        {
            string searchM = search.ToUpper();
            var tutor = db.tutors.ToList().Where(x => x.fullname.Contains(searchM) || x.specialized.Contains(searchM));
            return View(tutor);
        }

        [Filter.Authorize]
        [HttpPost]
        public ActionResult Duyetkhoahoc(int id)
        {
            int asd = id;
            var se = db.sessions.Find(id);
            se.status_tutor = 1;
            db.SaveChanges();
            return RedirectToAction("Duyetkhoahoc");
        }

        [HttpPost]
        public ActionResult Contact(string name, string email, string subject, string message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderemail = new MailAddress("ppcrentalteam04@gmail.com", "tutor"); // mail tutor 
                    var receiveremail = new MailAddress("hoak21t@gmail.com", "Cong ty"); //mail cong ty

                    var password = "K21t1team04";// mật khẩu địa chỉ mail 
                    var sub = subject;
                    var body = "Tên: " + name + " Email: " + email + " Tiêu đề: " + subject + " Nội dung: " + message;
                    // nội dung tin nhắn


                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderemail.Address, password)


                    };

                    using (var mess = new MailMessage(senderemail, receiveremail)
                    {
                        Subject = subject,
                        Body = body
                    }
                    )
                    {
                        smtp.Send(mess);
                    }
                    return RedirectToAction("Confirm", "Tutor");
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "There are some problem in sending email";
            }
            return View();
        }

        public ActionResult Confirm()
        {
            return View();
        }
        [Filter.Authorize]
        public ActionResult registScheduleManager(int types)
        {
            string s = types.ToString();
            int n = int.Parse(s);
            var tutor = db.tutors.FirstOrDefault(x => x.tutor_id == n);
            if (tutor.status_register == 1)
            {
                tutor.status_register = 2;
                db.SaveChanges();
            }
            else if (tutor.status_register == 2)
            {
                tutor.status_register = 1;
                db.SaveChanges();
            }
            setAlert("Bạn đã thay đổi thành công", "success");
            return RedirectToAction("InfoOfTutor");
        }



    }
}