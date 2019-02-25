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
            return View();
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
    }
}