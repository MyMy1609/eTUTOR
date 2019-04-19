using System;
using Moq;
using System.Web;
using System.Web.Mvc;
using eTUTOR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using eTUTOR.Controllers;
using eTUTOR.Models;
using System.Web.Routing;
using System.Web.SessionState;
using eTUTOR.Tests.Support;

namespace eTUTOR.Tests.Controllers
{
    [TestClass]
    public class UserControllerTest
    {
        /*
            UNIT TEST FOR TUTOR LOGIN FUNCTION
        */
        //Test case : tutor login with valid email and password
        [TestMethod]
        public void TutorLogin_WithValidInput()
        {
            //Arrange
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpSessionStateBase> moqSession = new Mock<HttpSessionStateBase>();
            moqContext.Setup(ctx => ctx.Session).Returns(moqSession.Object);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            var controller = new UserController();
            var tutorAccount = new tutor
            {
                email = "tieumynvx@gmail.com",
                password = "12345678",
            };

            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData() , controller);
            var validationResults = TestModelHelper.ValidateModel(controller, tutorAccount);

            //Action
            var redirectRoute = controller.Login(tutorAccount.email, tutorAccount.password, null) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(redirectRoute);
            Assert.AreEqual("InfoOfTutor", redirectRoute.RouteValues["action"]);
            Assert.AreEqual("Tutor", redirectRoute.RouteValues["controller"]);
        }
        //Test case : tutor login with invalid email
        [TestMethod]
        public void TutorLogin_WithInvalidUserName()
        {
            //Arrange
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpSessionStateBase> moqSession = new Mock<HttpSessionStateBase>();
            moqContext.Setup(ctx => ctx.Session).Returns(moqSession.Object);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            var controller = new UserController();
            var tutorAccount = new tutor
            {
                email = "invalidusername@gmail.com",
                password = "12345678",
            };

            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, tutorAccount);

            //Action
            var redirectRoute = controller.Login(tutorAccount.email, tutorAccount.password, null) as ViewResult;

            //Assert
            Assert.IsNotNull(redirectRoute);
            Assert.AreEqual("username không tồn tại", redirectRoute.ViewBag.msg);
        }
        //Test case : tutor login with Valid username and Invalid password
        [TestMethod]
        public void TutorLogin_WithValidUsernameAndInvalidPassword()
        {
            //Arrange
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpSessionStateBase> moqSession = new Mock<HttpSessionStateBase>();
            moqContext.Setup(ctx => ctx.Session).Returns(moqSession.Object);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            var controller = new UserController();
            var tutorAccount = new tutor
            {
                email = "tieumynvx@gmail.com",
                password = "invalidPassword",
            };

            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, tutorAccount);

            //Action
            var redirectRoute = controller.Login(tutorAccount.email, tutorAccount.password, null) as ViewResult;

            //Assert
            Assert.IsNotNull(redirectRoute);
            Assert.AreEqual("Mật khẩu sai rồi !", redirectRoute.ViewBag.msg);
        }
        //Test case : tutor login with account Status is 2 (Account not activated yet)
        [TestMethod]
        public void TutorLogin_WithAccountStatusIs2()
        {
            //Arrange
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpSessionStateBase> moqSession = new Mock<HttpSessionStateBase>();
            moqContext.Setup(ctx => ctx.Session).Returns(moqSession.Object);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            var controller = new UserController();
            var tutorAccount = new tutor
            {
                email = "tieumynvx@gmail.com",
                password = "invalidPassword",
            };

            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, tutorAccount);

            //Action
            var redirectRoute = controller.Login(tutorAccount.email, tutorAccount.password, null) as ViewResult;

            //Assert
            Assert.IsNotNull(redirectRoute);
            Assert.AreEqual("Mật khẩu sai rồi !", redirectRoute.ViewBag.msg);
        }
        /*
            UNIT TEST FOR STUDENT LOGIN FUNCTION
        */


        [TestMethod]
        public void StudentLogin_WithValidInput()
        {
            //Arrange
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpSessionStateBase> moqSession = new Mock<HttpSessionStateBase>();
            moqContext.Setup(ctx => ctx.Session).Returns(moqSession.Object);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            var controller = new UserController();
            var studentAccount = new tutor
            {
                username = "vyvy",
                password = "12345678",
            };

            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, studentAccount);

            //Action
            var redirectRoute = controller.Login(studentAccount.username, studentAccount.password, null) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(redirectRoute);
            Assert.AreEqual("InfoOfStudent", redirectRoute.RouteValues["action"]);
            Assert.AreEqual("Student", redirectRoute.RouteValues["controller"]);
        }



        /*
           UNIT TEST FOR PARENT LOGIN FUNCTION
       */


        [TestMethod]
        public void ParentLogin_WithValidInput()
        {
            //Arrange
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpSessionStateBase> moqSession = new Mock<HttpSessionStateBase>();
            moqContext.Setup(ctx => ctx.Session).Returns(moqSession.Object);
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);

            var controller = new UserController();
            var parentAccount = new tutor
            {
                email = "parent@gmail.com",
                password = "12345678",
            };

            controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);
            var validationResults = TestModelHelper.ValidateModel(controller, parentAccount);

            //Action
            var redirectRoute = controller.Login(parentAccount.email, parentAccount.password, null) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(redirectRoute);
            Assert.AreEqual("InfoOfParent", redirectRoute.RouteValues["action"]);
            Assert.AreEqual("Parent", redirectRoute.RouteValues["controller"]);
        }
    }
}
