using Microsoft.AspNetCore.Mvc;
using ViewApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProject1.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TestView()
        {
            return View();
        }
        public ActionResult ErrorMessage()
        {
            return View();
        }
        public string Welcome(string Name, int numTimes = 1)
        {
            return HttpUtility.HtmlEncode("Hello, " + Name + " Number of Times is " + numTimes);
        }
        public string Welcome2(string Name, int ID = 1)
        {
            return HttpUtility.HtmlEncode("Hello, " + Name + " ID is " + ID);
        }

        public string PrintMessage()
        {
            return "<h1>Welcome</h1><p>This is the first custom page of your app</p>";
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Payment()
        {
            return View();
        }
        public ActionResult ConfirmCode()
        {
            return View();
        }



    }
}