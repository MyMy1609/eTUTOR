using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;

namespace eTUTOR.Controllers
{
    [Filter.Authorize]
    public class ParentController : BaseController
    {
        eTUITOREntities db = new eTUITOREntities();
        // GET: Parent
        public ActionResult InfoOfParent()
        {
            var parent_id = int.Parse(Session["UserID"].ToString());
            var info = db.parents.FirstOrDefault(x => x.parent_id == parent_id);
            return View(info);
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
        public ActionResult listRegistCourse()
        {
            string id = Session["username"].ToString();
            var prt = db.parents.FirstOrDefault(x => x.username == id);
            var listCourse = db.sessions.ToList().Where(x => x.status_tutor == 2 && x.student_id == prt.parent_id);
            return View(listCourse);
        }
        public ActionResult SessionOfChilds()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateChildAccount(student student, HttpPostedFileBase avatarSon)
        {
            
            if (ModelState.IsValid)
            {
                if (avatarSon == null)
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }
                else if (avatarSon.ContentLength > 0)
                {
                    int MaxContentLength = 1024 * 1024 * 3; //3 MB
                    string[] AllowedFileExtensions = new string[] { ".jpg", ".png", ".pdf" };

                    if (!AllowedFileExtensions.Contains(avatarSon.FileName.Substring(avatarSon.FileName.LastIndexOf('.'))))
                    {
                        ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                    }

                    else if (avatarSon.ContentLength > MaxContentLength)
                    {
                        ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
                    }
                    else
                    {
                        //TO:DO
                        var fileName = Path.GetFileName(avatarSon.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Upload"), fileName);
                        avatarSon.SaveAs(path);
                        ModelState.Clear();
                        student.parent_id = int.Parse(Session["UserID"].ToString());
                        student.dateCreate = DateTime.Now;
                        student.avatar = path;
                        db.students.Add(student);
                        db.SaveChanges();
                        return RedirectToAction("InfoOfParent", "Parent", new { id = Session["UserID"] });
                    }
                }
            }
            setAlert("Vui lòng tải ảnh đại diện cho con của bạn", "danger");
            return View("InfoOfParent");

        }
    }
}