using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;
using eTUTOR.Service;

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
                    return RedirectToAction("Dashboard", "Admin");
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
            Session.Clear();
            return RedirectToAction("Login");
        }
        public ActionResult Course()
        {
            var tutor_id = int.Parse(Session["UserID"].ToString());
            var info = model.tutors.FirstOrDefault();
            List<session> sessionList = model.sessions.Where(x => x.status_tutor == 1 && x.status_admin == 2).ToList();
            ViewData["sessionlist"] = sessionList;
            return View(info);
        }
        public ActionResult Schedule()
        {
            var tutor_id = int.Parse(Session["UserID"].ToString());
            var schedule = model.schedules.Where(x => x.status == 2).ToList();
            return View(schedule);
        }

        public ActionResult Dashboard()
        {
            ViewBag.Message = "Dashboard.";
            return View();
        }

        public ActionResult DetailParent(int id)
        {
            var parent = model.parents.Find(id);
            if (parent == null) return HttpNotFound();
            return View(parent);
        }

        public ActionResult DetailStudent(int id)
        {
            var student = model.students.Find(id);
            if (student == null) return HttpNotFound();
            return View(student);
        }

        public ActionResult DetailTutor(int id)
        {
            var tutor = model.tutors.Find(id);
            if (tutor == null) return HttpNotFound();
            return View(tutor);
        }

        public ActionResult Detail()
        {
            ViewBag.Message = "Detail.";
            return View();
        }
        public ActionResult ThongKe()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ThongKe(string kieuTK, DateTime dateStart, DateTime dateEnd)
        {
            if (kieuTK == "slDangKy")
            {
                ViewBag.dateStart = dateStart.ToString();
                ViewBag.dateEnd = dateEnd.ToString();
                ViewBag.resultParent = model.parents.Count(x => x.dateRegist.Value.Year == DateTime.Now.Year && x.dateRegist > dateStart && x.dateRegist < dateEnd);
                ViewBag.resultStudent = model.students.Count(x => x.dateCreate.Value.Year == DateTime.Now.Year && x.dateCreate > dateStart && x.dateCreate < dateEnd);
                ViewBag.resultTutor = model.tutors.Count(x => x.dateCreate.Value.Year == DateTime.Now.Year && x.dateCreate > dateStart && x.dateCreate < dateEnd);
                ViewBag.mot = model.parents.Count(x => x.dateRegist.Value.Month == 1);
                ViewBag.hai = model.parents.Count(x => x.dateRegist.Value.Month == 2);
                ViewBag.ba = model.parents.Count(x => x.dateRegist.Value.Month == 3);
                ViewBag.bon = model.parents.Count(x => x.dateRegist.Value.Month == 4);
                ViewBag.nam = model.parents.Count(x => x.dateRegist.Value.Month == 5);
                ViewBag.sau = model.parents.Count(x => x.dateRegist.Value.Month == 6);
                ViewBag.bay = model.parents.Count(x => x.dateRegist.Value.Month == 7);
                ViewBag.tam = model.parents.Count(x => x.dateRegist.Value.Month == 8);
                ViewBag.chin = model.parents.Count(x => x.dateRegist.Value.Month == 9);
                ViewBag.muoi = model.parents.Count(x => x.dateRegist.Value.Month == 10);
                ViewBag.muoimot = model.parents.Count(x => x.dateRegist.Value.Month == 11);
                ViewBag.muoihai = model.parents.Count(x => x.dateRegist.Value.Month == 12);

                //hocsinh
                ViewBag.smot = model.students.Count(x => x.dateCreate.Value.Month == 1);
                ViewBag.shai = model.students.Count(x => x.dateCreate.Value.Month == 2);
                ViewBag.sba = model.students.Count(x => x.dateCreate.Value.Month == 3);
                ViewBag.sbon = model.students.Count(x => x.dateCreate.Value.Month == 4);
                ViewBag.snam = model.students.Count(x => x.dateCreate.Value.Month == 5);
                ViewBag.ssau = model.students.Count(x => x.dateCreate.Value.Month == 6);
                ViewBag.sbay = model.students.Count(x => x.dateCreate.Value.Month == 7);
                ViewBag.stam = model.students.Count(x => x.dateCreate.Value.Month == 8);
                ViewBag.schin = model.students.Count(x => x.dateCreate.Value.Month == 9);
                ViewBag.smuoi = model.students.Count(x => x.dateCreate.Value.Month == 10);
                ViewBag.smuoimot = model.students.Count(x => x.dateCreate.Value.Month == 11);
                ViewBag.smuoihai = model.students.Count(x => x.dateCreate.Value.Month == 12);

                //tutor
                ViewBag.tmot = model.tutors.Count(x => x.dateCreate.Value.Month == 1);
                ViewBag.thai = model.tutors.Count(x => x.dateCreate.Value.Month == 2);
                ViewBag.tba = model.tutors.Count(x => x.dateCreate.Value.Month == 3);
                ViewBag.tbon = model.tutors.Count(x => x.dateCreate.Value.Month == 4);
                ViewBag.tnam = model.tutors.Count(x => x.dateCreate.Value.Month == 5);
                ViewBag.tsau = model.tutors.Count(x => x.dateCreate.Value.Month == 6);
                ViewBag.tbay = model.tutors.Count(x => x.dateCreate.Value.Month == 7);
                ViewBag.ttam = model.tutors.Count(x => x.dateCreate.Value.Month == 8);
                ViewBag.tchin = model.tutors.Count(x => x.dateCreate.Value.Month == 9);
                ViewBag.tmuoi = model.tutors.Count(x => x.dateCreate.Value.Month == 10);
                ViewBag.tmuoimot = model.tutors.Count(x => x.dateCreate.Value.Month == 11);
                ViewBag.tmuoihai = model.tutors.Count(x => x.dateCreate.Value.Month == 12);
                return View();

            }
            return View();
        }
        public ActionResult User()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Duyetkhoahoc(int id)
        {
            int asd = id;
            var se = model.sessions.Find(id);
            se.status_admin = 1;
            model.SaveChanges();
            return RedirectToAction("Duyetkhoahoc");
        }

        [HttpPost]
        public ActionResult Duyetschedule(int id)
        {
            int asd = id;
            var se = model.schedules.Find(id);
            se.status = 1;
            model.SaveChanges();
            return RedirectToAction("Duyetschedule");
        }

        public ActionResult Contact()
        {
            var contact = model.contact_admin.ToList();
            return View(contact);
        }

        public ActionResult Blockuser()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Duyettutor(int id)
        {
            int asd = id;
            var se = model.tutors.Find(id);
            se.status = 1;
            model.SaveChanges();
            return RedirectToAction("User");
        }

        [HttpPost]
        public ActionResult Duyetparent(int id)
        {
            int asd = id;
            var se = model.parents.Find(id);
            se.status = 1;
            model.SaveChanges();
            return RedirectToAction("User");
        }

        [HttpPost]
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
            return RedirectToAction("Khoatutor");
        }

        [HttpPost]
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
            return RedirectToAction("Khoaparent");
        }

        [HttpPost]
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
            return RedirectToAction("Khoastudent");
        }
    }
}
