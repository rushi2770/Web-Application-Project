using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Models;
using System.Data.Entity.Validation;
using System.Threading.Tasks;


namespace MVC.Controllers
{
    
    public class AccountController : Controller
    {
        // GET: Account
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Index(userLoginDetails input)
        //{
        //    using (DataquadEntities db = new DataquadEntities())
        //    {
        //        var userDetails = db.userLoginDetails.Where(x => x.userName == input.userName && x.password == input.password).FirstOrDefault();

        //        if (userDetails == null)
        //        {
        //            input.ErrorMessage = "Invalid username or password";
        //            return View("Index", input);
        //        }
        //        else
        //        {
        //            var xyz = db.userPersonalDetails.Where(x => x.id == userDetails.userId).FirstOrDefault();
        //            if (xyz != null)
        //            {
        //                return RedirectToAction("LogOn", "Account", new { ID = xyz.id });
        //            }
        //            else
        //            {
        //                return RedirectToAction("CreateProfile", new { id = userDetails.userId });
        //            }
        //        }
        //    }
        //}
        
        public ActionResult DisplayImages(int? id)
        {
            DataquadEntities db = new DataquadEntities();
            userProfileImages userImages = new userProfileImages();
            
            return View("LogOn", id);

        }
        
        public ActionResult LogOn(int? ID)
        {
            if (ID == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                using (DataquadEntities db = new DataquadEntities())
                {
                    ViewBag.userImages = db.userProfileImages.Where(x => x.UserId == ID).FirstOrDefault();
                    ViewData["UserId"] = ID;
                    var userPersonalDetails = db.userPersonalDetails.Where(x => x.userId == ID).FirstOrDefault();
                    ViewBag.firstName = db.userDetails.Where(x => x.userId == ID).Select(y=>y.FirstName).FirstOrDefault();
                    ViewBag.userId = userPersonalDetails.userId;
                    return View(userPersonalDetails);
                }
            }
        }
        public ActionResult CreateProfile(int id)
        {
            TempData["id"] = id;
            userPersonalDetails newUser = new userPersonalDetails();
            newUser.id = id;
             
            newUser.GenderList.Add(new SelectListItem { Text = "Male", Value = "Male" });
            newUser.GenderList.Add(new SelectListItem { Text = "Female", Value = "Female" });
            newUser.GenderList.Add(new SelectListItem { Text = "Other", Value = "Other" });
            newUser.GenderList.Add(new SelectListItem { Text = "Don't want to specify", Value = "Don't want to specify" });
            using(DataquadEntities db = new DataquadEntities())
            {
                var races = db.Races.ToList();
                foreach(var race in races)
                {
                    newUser.RaceList.Add(new SelectListItem
                    {
                        Text = race.raceName,
                        Value = race.raceId.ToString()
                    });
                }
            }
            
            return View(newUser);
        }
        [HttpPost]
        public ActionResult CreateProfile(userPersonalDetails newUserDetails)
        {
            if (ModelState.IsValid)
            {
                using (DataquadEntities db = new DataquadEntities())
                {
                    //userPersonalDetails newUser = new userPersonalDetails();
                    newUserDetails.Gender = newUserDetails.selectedGender;
                    newUserDetails.Race = newUserDetails.selectedRaceName;
                    newUserDetails.userId = newUserDetails.id;
                    db.userPersonalDetails.Add(newUserDetails);

                    db.SaveChanges();
                    return RedirectToAction("LogOn", new { ID = newUserDetails.userId } );
                }
            }
            return View(newUserDetails);
        }
        
        public async Task<ActionResult> ViewProfile(int id)
        {
            using (DataquadEntities db = new DataquadEntities())
            {
                var userPersonalDetails = db.userPersonalDetails.Where(x => x.userId == id).FirstOrDefault();
                ViewBag.userImages = db.userProfileImages.Where(x => x.UserId == id).FirstOrDefault();
                ViewBag.files = await GetAllFiles.GetAllFilesByUserId(id);
                ViewBag.userId = id;
                return View(userPersonalDetails);
            }

        }

        public ActionResult PrintProfile(int id)
        {
            return new Rotativa.ActionAsPdf("ViewProfile", new { id = id});
        }
        //public static List<userFilesCollection> GetAllFiles(int userId)
        //{
        //    List<userFilesCollection> files = new List<userFilesCollection>();
        //    using (DataquadEntities db = new DataquadEntities())
        //    {
        //        files = db.userFilesCollections.Where(x => x.UserId == userId).ToList();
        //    }
        //    return files;
        //}

        public ActionResult EditProfile(int? id)
        {
            using (DataquadEntities db = new DataquadEntities())
            {
                var userDetails = db.userPersonalDetails.Where(x => x.userId == id).FirstOrDefault();
                if(userDetails == null)
                {
                    return RedirectToAction("CreateProfile", "Account", new { id = id });
                }else
                {
                    ViewBag.userId = id;
                    userDetails.GenderList.Add(new SelectListItem { Text = "Male", Value = "Male" });
                    userDetails.GenderList.Add(new SelectListItem { Text = "Female", Value = "Female" });
                    userDetails.GenderList.Add(new SelectListItem { Text = "Other", Value = "Other" });
                    userDetails.GenderList.Add(new SelectListItem { Text = "Don't want to specify", Value = "Don't want to specify" });
                    //userDetails.GenderList = new SelectList(genderList, "Value");
                    var races = db.Races.ToList();
                    foreach (var race in races)
                    {
                        userDetails.RaceList.Add(new SelectListItem
                        {
                            Text = race.raceName,
                            Value = race.raceId.ToString(),
                            Selected = race.raceName == userDetails.Race ? true : false
                        });
                    }
                    
                    return View(userDetails);
                }
               
            }

        }

        [HttpPost]
        public ActionResult EditProfile(userPersonalDetails newEditedDetails, int? id)
        {
            
            if (ModelState.IsValid)
            {

                using (DataquadEntities db = new DataquadEntities())
                {
                    var userFromDB = db.userPersonalDetails.Where(x => x.userId == id).FirstOrDefault();
                    userFromDB.Gender = newEditedDetails.selectedGender;
                    userFromDB.Race = newEditedDetails.selectedRaceName;
                    userFromDB.TechnologyIntersted = newEditedDetails.TechnologyIntersted;
                    userFromDB.Ethnicity = newEditedDetails.Ethnicity;
                    
                    db.SaveChanges();
                    return RedirectToAction("ViewProfile", new { id = userFromDB.userId });
                }
            }
            
            return View(newEditedDetails);
        }
            
        public ActionResult Upload(int? id)
        {
            DataquadEntities db = new DataquadEntities();
            userProfileImages image = new userProfileImages();
            image.UserId = id;
            ViewBag.userImages = db.userProfileImages.Where(x => x.UserId == id).FirstOrDefault();
            return View(image);
        }
        [HttpPost]
        public ActionResult Upload(userProfileImages image)
        {
            if(image.File != null)
            {
                if (image.File.ContentLength > (8 * 1024 * 1024))
                {
                    ModelState.AddModelError("CustomError", "File size must be less than 8 MB");
                    return View();
                }
                if (!(image.File.ContentType == "image/jpeg" || image.File.ContentType == "image/gif"))
                {
                    ModelState.AddModelError("CustomError", "File type allowed : jpeg and gif");
                    return View();
                }

                //Images image = new Images();
                image.FileName = image.File.FileName;
                image.ImageSize = image.File.ContentLength;

                byte[] data = new byte[image.File.ContentLength];
                image.File.InputStream.Read(data, 0, image.File.ContentLength);

                image.ImageData = data;

               // image.UserId = userId;
                if (ModelState.IsValid)
                {
                    using (DataquadEntities db = new DataquadEntities())
                    {
                        var recordFromDB = db.userProfileImages.Where(x => x.UserId == image.UserId).FirstOrDefault();
                        if (recordFromDB == null)
                        {
                            db.userProfileImages.Add(image);
                            db.SaveChanges();
                            return RedirectToAction("ViewProfile", new { id = image.UserId });
                        }
                        else
                        {
                            recordFromDB.FileName = image.FileName;
                            recordFromDB.ImageData = data;
                            recordFromDB.ImageSize = image.ImageSize;
                            recordFromDB.UserId = image.UserId;
                            db.SaveChanges();
                            return RedirectToAction("ViewProfile", new { id = recordFromDB.UserId });
                        }

                    }
                }
                return RedirectToAction("Home", "Home", new { id = image.UserId });
            }
            else
            {
                ModelState.AddModelError("FileName", "Please select an image to upload");
                DataquadEntities db = new DataquadEntities();
                userProfileImages abc = new userProfileImages();
                //image.UserId = userId;
                ViewBag.userImages = db.userProfileImages.Where(x => x.UserId == image.UserId).FirstOrDefault();
                return View(image);

            }
            
        }

        public ActionResult Logout()
        {            
            Session.Abandon();
            return RedirectToAction("Index", "Account");
        }
    }
}
