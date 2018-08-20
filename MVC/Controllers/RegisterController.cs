using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Models;
using System.Web.Script.Serialization;


namespace MVC.Controllers
{
    public class RegisterController : Controller
    {
        private DataquadEntities db = new DataquadEntities();

        public ActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Index(userLoginDetails input)
        {
            if (ModelState.IsValid)
            {
                db.userLoginDetails.Add(input);
                db.SaveChanges();
                return RedirectToAction("Index", "Account");
            }
            else
            {
                return View(input);
            }

        }
        public bool UserNameExists(string userName)
        {
            return db.userLoginDetails.Any(x => x.userName == userName);
        }

        public ActionResult userNameCheck()
        {
            return View();
        }

        public ActionResult GetAvailableUserName(string name)
        {
            userLoginDetails regsitration = new userLoginDetails();
            regsitration.UserNameInUse = false;

            while (UserNameExists(name))
            {
                Random random = new Random();
                int randomNumber = random.Next(1, 100);
                name = name + randomNumber.ToString();
                regsitration.UserNameInUse = true;
            }

            regsitration.userName = name;
            return Json(regsitration,JsonRequestBehavior.AllowGet);
            
        }
    
      
        public JsonResult IsUserNameAvailable(string userName)
        {
            return Json(!db.userLoginDetails.Any(x => x.userName == userName), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsEmailIdAvailable(string emailData)
        {
            return Json(!db.userLoginDetails.Any(x => x.email.ToLower() == emailData.ToLower()), JsonRequestBehavior.AllowGet);
        }
    }
}