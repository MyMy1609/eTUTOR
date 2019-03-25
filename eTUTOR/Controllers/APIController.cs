﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;
using Newtonsoft.Json.Linq;
using eTUTOR.Service;

namespace eTUTOR.Controllers
{
    public class APIController : Controller
    {
        //===================================================
        eTUITOREntities db = new eTUITOREntities();
        CommonService commonService = new CommonService();
        //===================================================
        // GET: API
        public PartialViewResult getSessionList(int dotw, int student_id)
        {
            string day;
            switch (dotw)
            {
                case 2:
                    day = "Thứ 2";
                    break;
                case 3:
                    day = "Thứ 3";
                    break;
                case 4:
                    day = "Thứ 4";
                    break;
                case 5:
                    day = "Thứ 5";
                    break;
                case 6:
                    day = "Thứ 6";
                    break;
                case 7:
                    day = "Thứ 7";
                    break;
                case 8:
                    day = "Chủ nhật";
                    break;
                default:
                    day = "all";
                    break;
            }
            List<session> sessionList;
            if (day == "all")
            {
                sessionList = db.sessions.Where(m => m.student_id == student_id).ToList();
            }
            else
            {
                sessionList = db.sessions.Where(m => m.day_otw == day && m.student_id == student_id).ToList();
            }
            ViewData["sessionlist"] = sessionList;
            return PartialView("SessionList");

        }
        public PartialViewResult getSessionListChilds(int dotw, int student_id)
        {
            string day;
            switch (dotw)
            {
                case 2:
                    day = "Thứ 2";
                    break;
                case 3:
                    day = "Thứ 3";
                    break;
                case 4:
                    day = "Thứ 4";
                    break;
                case 5:
                    day = "Thứ 5";
                    break;
                case 6:
                    day = "Thứ 6";
                    break;
                case 7:
                    day = "Thứ 7";
                    break;
                case 8:
                    day = "Chủ nhật";
                    break;
                default:
                    day = "all";
                    break;
            }
            List<session> sessionList;
            if (day == "all")
            {
                sessionList = db.sessions.Where(m => m.student_id == student_id).ToList();
            }
            else
            {
                sessionList = db.sessions.Where(m => m.day_otw == day && m.student_id == student_id).ToList();
            }
            ViewData["sessionlist"] = sessionList;
            return PartialView("SessionTutorList");

        }
        public PartialViewResult getSessionApproved(int dotw, int tutor_id)
        {
            string day;
            switch (dotw)
            {
                case 2:
                    day = "Thứ 2";
                    break;
                case 3:
                    day = "Thứ 3";
                    break;
                case 4:
                    day = "Thứ 4";
                    break;
                case 5:
                    day = "Thứ 5";
                    break;
                case 6:
                    day = "Thứ 6";
                    break;
                case 7:
                    day = "Thứ 7";
                    break;
                case 8:
                    day = "Chủ nhật";
                    break;
                default:
                    day = "all";
                    break;
            }
            List<session> sessionApproved;
            if (day == "all")
            {
                sessionApproved = db.sessions.Where(m => m.tutor_id == tutor_id && m.status_admin == 1).ToList();
            }
            else
            {
                sessionApproved = db.sessions.Where(m => m.day_otw == day && m.tutor_id == tutor_id && m.status_admin == 1).ToList();
            }
            ViewData["sessionapproved"] = sessionApproved;
            return PartialView("SessionApproved");

        }
        [HttpPost]
        public JsonResult addChild(string data)
        {
            int parentId = int.Parse(Session["UserID"].ToString());
            string message;
            var status = HttpStatusCode.OK;
            JObject child = JObject.Parse(data);
            var userName = child["username"].ToString();
            var email = child["email"].ToString();
            var studentUsername = db.students.Where(x => x.username == userName && x.email == email).FirstOrDefault();
            if (studentUsername == null)
            {
                student st = new student();
                //List<parent> pr = db.parents.Where(x => x.parent_id == parentId).ToList();
                st.fullname = child["fullname"].ToString();
                st.username = child["username"].ToString();
                st.password = commonService.hash(child["password"].ToString());
                st.parent_id = parentId;
                //st.phone = pr.
                st.email = child["email"].ToString();
                st.@class = int.Parse(child["class"].ToString());
                st.status = 1;
                st.birthday = DateTime.Parse(child["birthday"].ToString());
                db.students.Add(st);
                db.SaveChanges();
                message = "add student sucess";
                var response = new { message= message, status = status};
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                message = "username hoặc email đã được sử dụng , vui lòng chọn username và email khác";
                var response = new { message = message, status = status };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        public JsonResult editProfile(string data)
        {
        
            int parentId = int.Parse(Session["UserID"].ToString());
            string message;
            var status = HttpStatusCode.OK;
            JObject parent = JObject.Parse(data);
            parent pr = db.parents.Find(parentId);
           
            pr.fullname = parent["fullname"].ToString();
            pr.username = parent["username"].ToString();
            pr.address = parent["address"].ToString();
            pr.phone = parent["phone"].ToString();
            pr.email = parent["email"].ToString();
            
            db.SaveChanges();
            message = "update profile sucess";
            var response = new { message = message, status = status };
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult editProfileTutor(string data)
        {

            int tutorId = int.Parse(Session["UserID"].ToString());
            string message;
            var status = HttpStatusCode.OK;
            JObject tutor = JObject.Parse(data);
            tutor tt = db.tutors.Find(tutorId);

            tt.fullname = tutor["fullname"].ToString();
            tt.username = tutor["username"].ToString();
            tt.address = tutor["address"].ToString();
            tt.birthday = DateTime.ParseExact(tutor["birthday"].ToString(), "dd/MM/yyyy", null);
            tt.phone = tutor["phone"].ToString();
            tt.email = tutor["email"].ToString();
            tt.specialized = tutor["specialized"].ToString();
            tt.job = tutor["job"].ToString();
            tt.experience = tutor["experience"].ToString();

            db.SaveChanges();
            message = "update profile sucess";
            var response = new { message = message, status = status };
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult editProfileStudent(string data)
        {

            int studentId = int.Parse(Session["UserID"].ToString());
            string message;
            var status = HttpStatusCode.OK;
            JObject student = JObject.Parse(data);
            student st = db.students.Find(studentId);

            st.fullname = student["fullname"].ToString();
            st.username = student["username"].ToString();
            st.parent.address = student["address"].ToString();
            st.birthday = DateTime.ParseExact(student["birthday"].ToString(), "dd/MM/yyyy", null);
            st.phone = student["phone"].ToString();
            st.email = student["email"].ToString();
            st.@class = int.Parse(student["class"].ToString());

            db.SaveChanges();
            message = "update profile sucess";
            var response = new { message = message, status = status };
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}