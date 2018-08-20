using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Models;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Home(int id)
        {
            using (DataquadEntities db = new DataquadEntities())
            {
                ViewBag.userImages = db.userProfileImages.Where(x => x.UserId == id).FirstOrDefault();
                ViewData["UserId"] = id;
                var userPersonalDetails = db.userPersonalDetails.Where(x => x.userId == id).FirstOrDefault();
                ViewBag.userId = userPersonalDetails.userId;
                ViewBag.id = id;
                ViewBag.email = db.userDetails.Where(x => x.userId == id).Select(y => y.emailId).Single();
                return View(userPersonalDetails);
            }
        }
        //AboutUs Action Method
        public ActionResult AboutUs(int? id)
        {
            using(DataquadEntities db = new DataquadEntities())
            {
                var userDetails = db.userPersonalDetails.Where(x => x.userId == id).FirstOrDefault();
                return View(userDetails);
            }
            
        }
        //Services Action Method
        public ActionResult Services(int? id)
        {
            return View();
        }
        //Industries Action Method
        public ActionResult Industries(int? id)
        {
            return View();
        }
        //Careers Action Method
        public ActionResult Careers(int? id)
        {

            return View();
        }
        //ContactUs Action Method
        public ActionResult ContactUs(int? id)
        {
            return View();
        }

        [HttpPost]
        [ActionName("ContactUs")]
        public ActionResult ContactUsPost(int? id)
        {

            return View();
        }
    }
}