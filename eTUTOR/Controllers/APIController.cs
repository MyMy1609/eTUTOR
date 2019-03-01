using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;
namespace eTUTOR.Controllers
{
    public class APIController : Controller
    {
        //===================================================
        eTUITOREntities db = new eTUITOREntities();
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
                sessionApproved = db.sessions.Where(m => m.tutor_id == tutor_id && m.status_admin == 2).ToList();
            }
            else
            {
                sessionApproved = db.sessions.Where(m => m.day_otw == day && m.tutor_id == tutor_id && m.status_admin == 2).ToList();
            }
            ViewData["sessionapproved"] = sessionApproved;
            return PartialView("SessionApproved");

        }
    }
}