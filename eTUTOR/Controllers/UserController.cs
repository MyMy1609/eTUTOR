using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTUTOR.Models;
using eTUTOR.Service;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mail;
using System.Web.UI;
using System.Net.Mail;

namespace eTUTOR.Controllers
{

    public class UserController : BaseController
    {
        //===================================================
        eTUITOREntities model = new eTUITOREntities();
        CommonService commonService = new CommonService();
        //===================================================

        [HttpGet]
        public ActionResult Register()
        {
            var user = new tutor();
            return View(user);
        }
        [HttpPost]
        public ActionResult RegisterTuTor(tutor tutor, HttpPostedFileBase certificate, string password)
        {
            //add new tutor
            tutor.status = 2;
            if (certificate != null && certificate.ContentLength > 0)
            {
                tutor.certificate = certificate.FileName;
            }
            tutor.specialized = tutor.specialized.ToUpper();
            tutor.fullname = tutor.fullname.ToUpper();

            tutor.password = commonService.hash(tutor.password);
            tutor.dateCreate = DateTime.Now;
            model.tutors.Add(tutor);
            model.SaveChanges();

            //save certificate

            //create directory
            string AppPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = AppPath + String.Format("Content\\img\\certificates\\{0}", tutor.tutor_id);
            DirectoryInfo direc = Directory.CreateDirectory(filePath);

            //save certificate
            if (certificate != null && certificate.ContentLength > 0)
            {
                string fileName = Path.GetFileName(certificate.FileName);
                string path = String.Format("{0}\\{1}", filePath, fileName);
                certificate.SaveAs(path);
            }
            return RedirectToAction("ConfirmEmail", "User");
        }
        public ActionResult RegisterStudent(student student, string password)
        {
            student.status = 2;

            student.password = commonService.hash(student.password);
            student.dateCreate = DateTime.Now;
            model.students.Add(student);
            model.SaveChanges();
            return RedirectToAction("ConfirmEmail", "User");
        }
        public ActionResult RegisterParent(parent parent, string password)
        {

            parent.status = 2;

            parent.password = commonService.hash(parent.password);
            parent.dateRegist = DateTime.Now;
            model.parents.Add(parent);
            model.SaveChanges();
            return RedirectToAction("ConfirmEmail", "User");
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        public ActionResult CheckForgotPW(string username, string email)
        {
            var student = model.students.FirstOrDefault(x => x.username == username);
            var parent = model.parents.FirstOrDefault(x => x.username == username);
            var tutorr = model.tutors.FirstOrDefault(x => x.username == username);
            if (student != null)
            {
                if (student.email != email)
                {
                    ViewBag.Err = "Email sai rồi !";
                    return View("ForgotPassword");
                }
                string newPW = CreateLostPassword(10);
                CapNhatMatKhau(student.username, newPW, "std");
                guiMail(student.username, newPW, email);

                setAlert("Mật khẩu đã được thay đổi vui lòng kiểm tra emil !", "success");
                return View("Login");

            }
            if (parent != null)
            {
                if (parent.email != email)
                {
                    ViewBag.Err = "Email sai rồi !";
                    return View("ForgotPassword");
                }
                string newPW = CreateLostPassword(10);
                CapNhatMatKhau(parent.username, newPW, "prt");
                guiMail(parent.username, newPW, email);
                ViewBag.sc = "Mật khẩu đã được thay đổi vui lòng kiểm tra email !";
                return View("Login");
            }
            if (tutorr != null)
            {
                if (tutorr.email != email)
                {
                    ViewBag.Err = "Email sai rồi !";
                    return View("ForgotPassword");
                }
                string newPW = CreateLostPassword(10);
                CapNhatMatKhau(tutorr.username, newPW, "tutorr");
                guiMail(tutorr.username, newPW, email);
                ViewBag.sc = "Mật khẩu đã được thay đổi vui lòng kiểm tra email !";
                return View("Login");
            }
            else
            {
                ViewBag.Err = "Tên đăng nhập sai rồi !";
                return View("ForgotPassword");
            }


        }
        public ActionResult ConfirmEmail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var tutor = model.tutors.FirstOrDefault(x => x.email == email);
            var student = model.students.FirstOrDefault(x => x.username == email);
            var parent = model.parents.FirstOrDefault(x => x.email == email);
            var admin = model.admins.FirstOrDefault(x => x.email == email);
            password = commonService.hash(password);
            if (tutor != null)
            {
                if (tutor.password.Equals(password))
                {
                    Session["FullName"] = tutor.fullname;
                    Session["UserID"] = tutor.tutor_id;
                    Session["username"] = tutor.username;
                    Session["Role"] = "tutor";
                    return RedirectToAction("InfoOfTutor", "Tutor");
                }
                else
                {
                    ViewBag.msg = "Mật khẩu sai rồi vui lòng nhập lại mật khẩu !";
                    return View("Login");
                }

            }
            if (student != null)
            {
                if (student.password.Equals(password))
                {
                    Session["FullName"] = student.fullname;

                    Session["username"] = student.username;
                    Session["UserID"] = student.student_id;
                    Session["username"] = student.username;
                    Session["Role"] = "student";
                    return RedirectToAction("InfoOfStudent", "Student");
                }
                else
                {
                    ViewBag.msg = "Mật khẩu sai rồi vui lòng nhập lại mật khẩu !";
                    return View("Login");
                }

            }

            if (parent != null)
            {
                if (parent.password.Equals(password))
                {
                    Session["FullName"] = parent.fullname;
                    Session["username"] = parent.username;
                    Session["UserID"] = parent.parent_id;
                    Session["username"] = parent.username;
                    Session["Role"] = "parent";
                    return RedirectToAction("InfoOfParent", "Parent");
                }
                else
                {
                    ViewBag.msg = "Mật khẩu sai rồi vui lòng nhập lại mật khẩu !";
                    return View("Login");
                }

            }
            if (admin != null)
            {
                if (admin.password.Equals(password))
                {
                    Session["FullName"] = admin.fullname;
                    Session["UserID"] = admin.admin_id;
                    Session["username"] = admin.username;
                    Session["Role"] = "admin";
                    return RedirectToAction("dashboard", "home", new { area = "admin" });
                }
                else
                {
                    ViewBag.msg = "Mật khẩu sai rồi vui lòng nhập lại mật khẩu !";
                    return RedirectToAction("Login");
                }

            }
            ViewBag.msg = "username không tồn tại";
            return View("Login");


        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        //tạo pass mới ngẫu nhiên
        public string CreateLostPassword(int PasswordLength)
        {
            string _allowedChars = "abcdefghijk0123456789mnopqrstuvwxyz";
            Random randNum = new Random(); char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }

        private void CapNhatMatKhau(string TenDangNhap, string MatKhau, string typeUser)
        {
            if (typeUser == "std")
            {
                var stdd = model.students.FirstOrDefault(x => x.username == TenDangNhap);
                stdd.password = commonService.hash(MatKhau);
                model.SaveChanges();

            }
            else if (typeUser == "tutorr")
            {
                var tt = model.tutors.FirstOrDefault(x => x.username == TenDangNhap);
                tt.password = commonService.hash(MatKhau);
                model.SaveChanges();
            }
            else
            {
                var pr = model.parents.FirstOrDefault(x => x.username == TenDangNhap);
                pr.password = commonService.hash(MatKhau);
                model.SaveChanges();
            }
        }
        //nội dung gửi maill
        private string NoiDungMail(string TenDangNhap, string PW)
        {
            string NoiDung = "";
            NoiDung = "Đây là Mail gửi đến từ website của ETUTOR. ";
            NoiDung += "Mật khẩu mới của bạn là:  " + PW;
            NoiDung += "  . Sau khi đăng nhập bạn nhớ đổi lại mật khẩu để tiện cho việc đăng nhập lần tiếp theo";
            NoiDung += ". Vui lòng không trả lời Mail này!";
            return NoiDung;
        }
        public void guiMail(string TenDangNhap, string PW, string mail)
        {
            /*
            MailMessage objEmail = new MailMessage
            {
                To = mail,
                From = "trancongdu1997@gmail.com",
                Subject = "Thông tin về mật khẩu của bạn",
                BodyEncoding = Encoding.UTF8,
                Body = NoiDungMail(TenDangNhap, PW),
                Priority = MailPriority.High,
                BodyFormat = MailFormat.Html
            };

            try
            {
                SmtpMail.Send(objEmail);
            }
            catch (Exception exc)
            {
                Response.Write("Send failure: " + exc.ToString());
            }
            */
            using (System.Net.Mail.MailMessage emailMessage = new System.Net.Mail.MailMessage())
            {
                emailMessage.From = new MailAddress("td159855@gmail.com");
                emailMessage.To.Add(new MailAddress(mail));
                emailMessage.Subject = "eTUTOR - Lay lai mat khau";
                emailMessage.Body = NoiDungMail(TenDangNhap, PW);
                emailMessage.Priority = System.Net.Mail.MailPriority.Normal;
                using (SmtpClient MailClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    MailClient.EnableSsl = true;
                    MailClient.Credentials = new System.Net.NetworkCredential("td159855@gmail.com", "215437331");
                    MailClient.Send(emailMessage);
                }
            }
        }
        public ActionResult ChangePW()
        {
            return View();
        }
        //Đổi mk
        [HttpPost]
        public ActionResult ChangePW(string currentPW, string newPW, string cfNewPW)
        {
            if (currentPW == string.Empty || newPW == string.Empty || cfNewPW == string.Empty)
            {

                ViewBag.iff = "Vui lòng điền đầy đủ thông tin để thay đổi mật khẩu";
                return View("ChangePW");

            }
            else if (newPW == cfNewPW)
            {
                string s = Session["username"].ToString();
                var tutor = model.tutors.FirstOrDefault(x => x.username == s);
                var student = model.students.FirstOrDefault(x => x.username == s);
                var parent = model.parents.FirstOrDefault(x => x.username == s);
                if (tutor != null)
                {
                    string passCur = commonService.hash(currentPW);
                    if (tutor.password != passCur)
                    {
                        ViewBag.iff = "Mật khẩu hiện tại không đúng";
                        return View("ChangePW");
                    }
                    else
                    {
                        newPW = commonService.hash(newPW);
                        tutor.password = newPW;
                        model.SaveChanges();
                        setAlert("Đổi mật khẩu thành công", "success");
                        return RedirectToAction("InfoOfTutor", "Tutor");
                    }
                }
                if (student != null)
                {

                    string passCur = commonService.hash(currentPW);
                    if (student.password != passCur)
                    {
                        ViewBag.iff = "Mật khẩu hiện tại không đúng";
                        return View("ChangePW");
                    }
                    else
                    {
                        newPW = commonService.hash(newPW);
                        student.password = newPW;
                        model.SaveChanges();
                        setAlert("Đổi mật khẩu thành công", "success");
                        return RedirectToAction("InfoOfStudent", "Student");
                    }

                }

                if (parent != null)
                {

                    string passCur = commonService.hash(currentPW);
                    if (parent.password != passCur)
                    {
                        ViewBag.iff = "Mật khẩu hiện tại không đúng";
                        return View("ChangePW");
                    }
                    else
                    {
                        newPW = commonService.hash(newPW);
                        parent.password = newPW;
                        model.SaveChanges();
                        setAlert("Đổi mật khẩu thành công", "success");
                        return View("InfoOfParent", "Parent");
                    }

                }
                return View("ChangePW");
            }
            else
            {
                ViewBag.iff = "Xác nhận mật khẩu mới không trùng khớp";
                return View("ChangePW");
            }

        }
        [HttpPost]
        public ActionResult danhgiatutor(string ttID, string message)
        {
            string s = Session["UserID"].ToString();
            comment newcm = new comment();
            newcm.content = message;
            newcm.tutor_id = int.Parse(ttID);
            newcm.student_id = int.Parse(s);
            newcm.dateTimeCmt = DateTime.Now;
            model.comments.Add(newcm);
            model.SaveChanges();
            setAlert("Cảm ơn bạn đã đóng góp ý kiến", "success");
            return RedirectToAction("ViewDetailTutor", "Tutor", new { id = int.Parse(ttID) });
        }
        public ActionResult checkre(string username, string email)
        {
            var student = model.students.FirstOrDefault(x => x.username == username);
            var parent = model.parents.FirstOrDefault(x => x.username == username);
            var tutor = model.tutors.FirstOrDefault(x => x.username == username);
            if (student.email != email)
            {
                ViewBag.Er = "Email đã tồn tại !";
                return View("Register");
            }
            if (parent.email != email)
            {
                ViewBag.Er = "Email đã tồn tại !";
                return View("Register");
            }
            if (tutor.email != email)
            {
                ViewBag.Er = "Email đã tồn tại !";
                return View("Register");
            }
            return View();
        }

    }

}

