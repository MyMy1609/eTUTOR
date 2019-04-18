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
        [TestMethod]
        public void TutorLogin_WithValidInput()
        {
            //Arrange
            tutor tutorAccount = new tutor
            {
                email = "tieumynvx@gmail.com",
                password ="12345678",
            };
            Mock<HttpRequestBase> moqRequest = new Mock<HttpRequestBase>();
            Mock<HttpContextBase> moqContext = new Mock<HttpContextBase>();
            moqContext.Setup(r => r.Request).Returns(moqRequest.Object);
            var controller = new UserController();
            controller.ControllerContext = new ControllerContext();

            
            var validationResults = TestModelHelper.ValidateModel(controller, tutorAccount);
            //Action
            var result = controller.Login(tutorAccount.email, tutorAccount.password, null) as RedirectToRouteResult;
            //Assert
            Assert.IsNotNull(result);
            

        }
    }
}
