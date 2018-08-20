using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }
        public string GetDateTimeString()
        {
            return DateTime.Now.ToString();
        }
        public ActionResult UserDetails()
        {
            DataQuadService.DataQuadServiceClient client = new DataQuadService.DataQuadServiceClient();
            
            DataQuadService.userProfileImageModel image = client.GetProfileImageByUserId(1);

            ViewBag.userImages = image;
            
            return View("Image");
        }

        public ActionResult Image()
        {
            return View();
        }
    }
}