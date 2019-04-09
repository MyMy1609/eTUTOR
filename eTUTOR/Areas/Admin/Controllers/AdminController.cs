using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;
using eTUTOR.Service;
using System.Web.Helpers;

namespace eTUTOR.Areas.Admin.Controllers
{
    
    public class AdminController : Controller
    {
        eTUITOREntities model = new eTUITOREntities();
        CommonService commonService = new CommonService();
        // GET: Admin/Admin
        public ActionResult Login()
        {
            ViewBag.Message = "Login.";
            return View();
        }
        [HttpPost]
        [Filter.AuthorizeAdmin]
        public ActionResult Login(string email, string password)
        {
            ;
            var admin = model.admins.FirstOrDefault(x => x.email == email);
            
            if (admin != null)
            {
                string pass = commonService.hash(password);
                if (admin.password.Equals(pass))
                {
                    Session["FullName"] = admin.fullname;
                    Session["UserID"] = admin.admin_id;

                    Session["isAdmin"] = "Admin";
                    return RedirectToAction("Dashboard", "Admin");
                }
            }
            else
            {
                ViewBag.mgs = "tài khoản không tồn tại";
            }
            return View();
        }
        [Filter.AuthorizeAdmin]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Login");
        }
        [Filter.AuthorizeAdmin]
        public ActionResult Course()
        {
            if(Session["UserID"] == null) { return RedirectToAction("Login"); }
            var tutor_id = int.Parse(Session["UserID"].ToString());
            var info = model.tutors.FirstOrDefault();
            List<session> sessionList = model.sessions.Where(x => x.status_tutor == 1 && x.status_admin == 2).ToList();
            ViewData["sessionlist"] = sessionList;
            return View(info);
        }
        [Filter.AuthorizeAdmin]
        public ActionResult Schedule()
        {
            var tutor_id = int.Parse(Session["UserID"].ToString());
            var schedule = model.schedules.Where(x => x.status == 2).ToList();
            return View(schedule);
        }
        [Filter.AuthorizeAdmin]
        public ActionResult Dashboard()
        {
            ViewBag.Message = "Dashboard.";
            return View();
        }
        [Filter.AuthorizeAdmin]
        public ActionResult DetailParent(int id)
        {
            var parent = model.parents.Find(id);
            if (parent == null) return HttpNotFound();
            return View(parent);
        }
        [Filter.AuthorizeAdmin]
        public ActionResult DetailStudent(int id)
        {
            var student = model.students.Find(id);
            if (student == null) return HttpNotFound();
            return View(student);
        }
        [Filter.AuthorizeAdmin]
        public ActionResult DetailTutor(int id)
        {
            var tutor = model.tutors.Find(id);
            if (tutor == null) return HttpNotFound();
            return View(tutor);
        }
        [Filter.AuthorizeAdmin]
        public ActionResult Detail()
        {
            ViewBag.Message = "Detail.";
            return View();
        }
        [Filter.AuthorizeAdmin]
        public ActionResult ThongKe()
        {
            return View();
        }
        //[HttpGet]
        //public ActionResult ThongKe(string kieuTK, DateTime dateStart, DateTime dateEnd)
        //{
        //    if (kieuTK == "slDangKy")
        //    {
        //        ViewBag.dateStart = dateStart.ToString();
        //        ViewBag.dateEnd = dateEnd.ToString();
        //        ViewBag.resultParent = model.parents.Count(x => x.dateRegist.Value.Year == DateTime.Now.Year && x.dateRegist > dateStart && x.dateRegist < dateEnd);
        //        ViewBag.resultStudent = model.students.Count(x => x.dateCreate.Value.Year == DateTime.Now.Year && x.dateCreate > dateStart && x.dateCreate < dateEnd);
        //        ViewBag.resultTutor = model.tutors.Count(x => x.dateCreate.Value.Year == DateTime.Now.Year && x.dateCreate > dateStart && x.dateCreate < dateEnd);
        //        ViewBag.mot = model.parents.Count(x => x.dateRegist.Value.Month == 1);
        //        ViewBag.hai = model.parents.Count(x => x.dateRegist.Value.Month == 2);
        //        ViewBag.ba = model.parents.Count(x => x.dateRegist.Value.Month == 3);
        //        ViewBag.bon = model.parents.Count(x => x.dateRegist.Value.Month == 4);
        //        ViewBag.nam = model.parents.Count(x => x.dateRegist.Value.Month == 5);
        //        ViewBag.sau = model.parents.Count(x => x.dateRegist.Value.Month == 6);
        //        ViewBag.bay = model.parents.Count(x => x.dateRegist.Value.Month == 7);
        //        ViewBag.tam = model.parents.Count(x => x.dateRegist.Value.Month == 8);
        //        ViewBag.chin = model.parents.Count(x => x.dateRegist.Value.Month == 9);
        //        ViewBag.muoi = model.parents.Count(x => x.dateRegist.Value.Month == 10);
        //        ViewBag.muoimot = model.parents.Count(x => x.dateRegist.Value.Month == 11);
        //        ViewBag.muoihai = model.parents.Count(x => x.dateRegist.Value.Month == 12);

        //        //hocsinh
        //        ViewBag.smot = model.students.Count(x => x.dateCreate.Value.Month == 1);
        //        ViewBag.shai = model.students.Count(x => x.dateCreate.Value.Month == 2);
        //        ViewBag.sba = model.students.Count(x => x.dateCreate.Value.Month == 3);
        //        ViewBag.sbon = model.students.Count(x => x.dateCreate.Value.Month == 4);
        //        ViewBag.snam = model.students.Count(x => x.dateCreate.Value.Month == 5);
        //        ViewBag.ssau = model.students.Count(x => x.dateCreate.Value.Month == 6);
        //        ViewBag.sbay = model.students.Count(x => x.dateCreate.Value.Month == 7);
        //        ViewBag.stam = model.students.Count(x => x.dateCreate.Value.Month == 8);
        //        ViewBag.schin = model.students.Count(x => x.dateCreate.Value.Month == 9);
        //        ViewBag.smuoi = model.students.Count(x => x.dateCreate.Value.Month == 10);
        //        ViewBag.smuoimot = model.students.Count(x => x.dateCreate.Value.Month == 11);
        //        ViewBag.smuoihai = model.students.Count(x => x.dateCreate.Value.Month == 12);

        //        //tutor
        //        ViewBag.tmot = model.tutors.Count(x => x.dateCreate.Value.Month == 1);
        //        ViewBag.thai = model.tutors.Count(x => x.dateCreate.Value.Month == 2);
        //        ViewBag.tba = model.tutors.Count(x => x.dateCreate.Value.Month == 3);
        //        ViewBag.tbon = model.tutors.Count(x => x.dateCreate.Value.Month == 4);
        //        ViewBag.tnam = model.tutors.Count(x => x.dateCreate.Value.Month == 5);
        //        ViewBag.tsau = model.tutors.Count(x => x.dateCreate.Value.Month == 6);
        //        ViewBag.tbay = model.tutors.Count(x => x.dateCreate.Value.Month == 7);
        //        ViewBag.ttam = model.tutors.Count(x => x.dateCreate.Value.Month == 8);
        //        ViewBag.tchin = model.tutors.Count(x => x.dateCreate.Value.Month == 9);
        //        ViewBag.tmuoi = model.tutors.Count(x => x.dateCreate.Value.Month == 10);
        //        ViewBag.tmuoimot = model.tutors.Count(x => x.dateCreate.Value.Month == 11);
        //        ViewBag.tmuoihai = model.tutors.Count(x => x.dateCreate.Value.Month == 12);
        //        return View();

        //    }
        //    return View();
        //}
        [HttpGet]
        [Filter.AuthorizeAdmin]
        public ActionResult ThongKee(string kieuTK, DateTime dateStart, DateTime dateEnd)
        {
            if (kieuTK == "slDangKy")
            {
                //var thangDangky = new int[12];
                //var listAccountTutorCreate = model.tutors.ToList();
                //for(int i = 0; i < 12; i++)
                //{
                //    thangDangky[i] = (model.parents.Count(
                //        x => x.dateRegist >= dateStart &&
                //        x.dateRegist <= dateEnd &&
                //        x.dateRegist.Value.Month == (i + 1))) +
                //                     (model.tutors.Count(
                //        x => x.dateCreate >= dateStart &&
                //        x.dateCreate <= dateEnd &&
                //        x.dateCreate.Value.Month == (i + 1))) +
                //                     (model.students.Count(
                //        x => x.dateCreate >= dateStart &&
                //        x.dateCreate <= dateEnd &&
                //        x.dateCreate.Value.Month == (i + 1)));
                //}
                //TempData["arrayDangKy"] = thangDangky;
                //return RedirectToAction("ThongKe");
               

            }
            return View();
        }
        [Filter.AuthorizeAdmin]
        public ActionResult tkDangky()
        {
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            var result = model.Users.ToList();
            decimal y1 = model.Users.Count(x => x.dateCreate.Value.Month == 1);
            decimal y2 = model.Users.Count(x => x.dateCreate.Value.Month == 2);
            decimal y3 = model.Users.Count(x => x.dateCreate.Value.Month == 3);
            decimal y4 = model.Users.Count(x => x.dateCreate.Value.Month == 4);
            decimal y5 = model.Users.Count(x => x.dateCreate.Value.Month == 5);
            decimal y6 = model.Users.Count(x => x.dateCreate.Value.Month == 6);
            decimal y7 = model.Users.Count(x => x.dateCreate.Value.Month == 7);
            decimal y8 = model.Users.Count(x => x.dateCreate.Value.Month == 8);
            decimal y9 = model.Users.Count(x => x.dateCreate.Value.Month == 9);
            decimal y10 = model.Users.Count(x => x.dateCreate.Value.Month == 10);
            decimal y11 = model.Users.Count(x => x.dateCreate.Value.Month == 11);
            decimal y12 = model.Users.Count(x => x.dateCreate.Value.Month == 12);

            //for(int i = 0; i < 12; i++)
            //{
            //    int u = i + 1;
            //    xValue[i] = "Tháng " + u;
            //}
             
            new Chart(width: 800, height: 550)
                .AddTitle("Số lượng người đăng ký trong năm nay")
                .AddSeries( 
                chartType: "Column", 
                xValue: new[] {"Thg1", "Thg2", "Thg3", "Thg4", "Thg5", "Thg6", "Thg7", "Thg8", "Thg9", "Thg10", "Thg11", "Thg12" }
                , yValues: new[] {y1, y2, y3, y4, y5, y6, y7, y8, y9, y10, y11, y12 })
                .Write("png");
            return null;
        }
        [Filter.AuthorizeAdmin]
        public ActionResult tkDayhoc()
        {
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            var result = model.history_lessons.ToList();
            decimal y1 = model.history_lessons.Count(x => x.day.Value.Month == 1);
            decimal y2 = model.history_lessons.Count(x => x.day.Value.Month == 2);
            decimal y3 = model.history_lessons.Count(x => x.day.Value.Month == 3);
            decimal y4 = model.history_lessons.Count(x => x.day.Value.Month == 4);
            decimal y5 = model.history_lessons.Count(x => x.day.Value.Month == 5);
            decimal y6 = model.history_lessons.Count(x => x.day.Value.Month == 6);
            decimal y7 = model.history_lessons.Count(x => x.day.Value.Month == 7);
            decimal y8 = model.history_lessons.Count(x => x.day.Value.Month == 8);
            decimal y9 = model.history_lessons.Count(x => x.day.Value.Month == 9);
            decimal y10 = model.history_lessons.Count(x => x.day.Value.Month == 10);
            decimal y11 = model.history_lessons.Count(x => x.day.Value.Month == 11);
            decimal y12 = model.history_lessons.Count(x => x.day.Value.Month == 12);

            //for(int i = 0; i < 12; i++)
            //{
            //    int u = i + 1;
            //    xValue[i] = "Tháng " + u;
            //}

            new Chart(width: 800, height: 550)
                .AddTitle("Số lượng các buổi học đã dạy trong năm nay")
                .AddSeries(
                chartType: "Column",
                xValue: new[] { "Thg1", "Thg2", "Thg3", "Thg4", "Thg5", "Thg6", "Thg7", "Thg8", "Thg9", "Thg10", "Thg11", "Thg12" }
                , yValues: new[] { y1, y2, y3, y4, y5, y6, y7, y8, y9, y10, y11, y12 })
                .Write("png");
            return null;
        }
        [Filter.AuthorizeAdmin]
        public ActionResult tkDangkyhoc()
        {
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            var result = model.sessions.ToList();
            decimal y1 = model.sessions.Count(x => x.dateCreate.Value.Month == 1);
            decimal y2 = model.sessions.Count(x => x.dateCreate.Value.Month == 2);
            decimal y3 = model.sessions.Count(x => x.dateCreate.Value.Month == 3);
            decimal y4 = model.sessions.Count(x => x.dateCreate.Value.Month == 4);
            decimal y5 = model.sessions.Count(x => x.dateCreate.Value.Month == 5);
            decimal y6 = model.sessions.Count(x => x.dateCreate.Value.Month == 6);
            decimal y7 = model.sessions.Count(x => x.dateCreate.Value.Month == 7);
            decimal y8 = model.sessions.Count(x => x.dateCreate.Value.Month == 8);
            decimal y9 = model.sessions.Count(x => x.dateCreate.Value.Month == 9);
            decimal y10 = model.sessions.Count(x => x.dateCreate.Value.Month == 10);
            decimal y11 = model.sessions.Count(x => x.dateCreate.Value.Month == 11);
            decimal y12 = model.sessions.Count(x => x.dateCreate.Value.Month == 12);

            //for(int i = 0; i < 12; i++)
            //{
            //    int u = i + 1;
            //    xValue[i] = "Tháng " + u;
            //}

            new Chart(width: 800, height: 550)
                .AddTitle("Số lượng đăng ký học dạy kèm trong năm nay")
                .AddSeries(
                chartType: "Column",
                xValue: new[] { "Thg1", "Thg2", "Thg3", "Thg4", "Thg5", "Thg6", "Thg7", "Thg8", "Thg9", "Thg10", "Thg11", "Thg12" }
                , yValues: new[] { y1, y2, y3, y4, y5, y6, y7, y8, y9, y10, y11, y12 })
                .Write("png");
            return null;
        }
        [Filter.AuthorizeAdmin]
        public ActionResult User()
        {
            return View();
        }



        [HttpPost]
        [Filter.AuthorizeAdmin]
        public ActionResult Duyetkhoahoc(int id)
        {
            int asd = id;
            var se = model.sessions.Find(id);
            se.status_admin = 1;
            model.SaveChanges();
            return RedirectToAction("Duyetkhoahoc");
        }

        [HttpPost]
        [Filter.AuthorizeAdmin]
        public ActionResult Duyetschedule(int id)
        {
            int asd = id;
            var se = model.schedules.Find(id);
            se.status = 1;
            model.SaveChanges();
            return RedirectToAction("Duyetschedule");
        }
        [Filter.AuthorizeAdmin]
        public ActionResult Contact()
        {
            var contact = model.contact_admin.ToList();
            return View(contact);
        }
        [Filter.AuthorizeAdmin]
        public ActionResult Blockuser()
        {

            return View();
        }

        [HttpPost]
        [Filter.AuthorizeAdmin]
        public ActionResult Duyettutor(int id)
        {
            int asd = id;
            var se = model.tutors.Find(id);
            se.status = 1;
            model.SaveChanges();
            return RedirectToAction("User");
        }

        [HttpPost]
        [Filter.AuthorizeAdmin]
        public ActionResult Duyetparent(int id)
        {
            int asd = id;
            var se = model.parents.Find(id);
            se.status = 1;
            model.SaveChanges();
            return RedirectToAction("User");
        }

        [HttpPost]
        [Filter.AuthorizeAdmin]
        public ActionResult Duyetstudent(int id)
        {
            int asd = id;
            var se = model.students.Find(id);
            se.status = 1;
            model.SaveChanges();
            return RedirectToAction("User","Admin");
        }

        //khoa
        [HttpPost]
        [Filter.AuthorizeAdmin]
        public ActionResult Khoatutor(int id)
        {
            int asd = id;
            var se = model.tutors.Find(id);
            if (se.status == 1)
            {
                se.status = 3;
                model.SaveChanges();
            }
            else if (se.status == 3)
            {
                se.status = 1;
                model.SaveChanges();
            }
            return RedirectToAction("Blockuser");
        }

        [HttpPost]
        [Filter.AuthorizeAdmin]
        public ActionResult Khoaparent(int id)
        {
            int asd = id;
            var se = model.parents.Find(id);
            if (se.status == 1)
            {
                se.status = 3;
                model.SaveChanges();
            }
            else if (se.status == 3)
            {
                se.status = 1;
                model.SaveChanges();
            }
            return RedirectToAction("Blockuser");
        }

        [HttpPost]
        [Filter.AuthorizeAdmin]
        public ActionResult Khoastudent(int id)
        {
            int asd = id;
            var se = model.students.Find(id);
            if (se.status == 1)
            {
                se.status = 3;
                model.SaveChanges();
            }
            else if (se.status == 3)
            {
                se.status = 1;
                model.SaveChanges();
            }
            return RedirectToAction("Blockuser");
        }
    }
}
