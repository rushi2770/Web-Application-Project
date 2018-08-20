using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        DataQuadService.DataQuadServiceClient client = new DataQuadService.DataQuadServiceClient();
        // GET: newRegistration
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "isEmailVerified,activationCode,ConfirmPassword")] DataQuadService.userDetailsModel user)
        {
            bool Status = false;
            string message = "";
            
            // Model Validation 
            if (ModelState.IsValid)
            {
                #region //Email is already Exist 
                var isExist = IsEmailExist(user.emailId);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }
                #endregion

                #region Generate Activation Code 
                user.activationCode = Guid.NewGuid();
                #endregion

                #region  Password Hashing 
                user.password = Crypto.Hash(user.password);
                //user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword); //
                #endregion
                user.isEmailVerified = false;

                #region Save to Database
                using (DataquadEntities dc = new DataquadEntities())
                {
                    //WCFService.userDetails WCFuser = new WCFService.userDetails();
                    //WCFuser.activationCode = user.activationCode;
                    //WCFuser.dateOfBirth= user.dateOfBirth;
                    //WCFuser.emailId = user.emailId;
                    //WCFuser.FirstName = user.FirstName;
                    //WCFuser.LastName = user.LastName;
                    //WCFuser.password = user.password;

                    //client.RegisterUser(WCFuser);

                    // dc.userDetails.Add(user);
                    //dc.SaveChanges();
                    if (client.RegisterUser(user))
                    {
                        //Send Email to User
                        SendVerificationLinkEmail(user.emailId, user.activationCode.ToString());
                        message = "Registration successfully done. Account activation link " +
                            " has been sent to your email id:" + user.emailId;
                        Status = true;
                    }else
                    {
                        message = "Invalid Request";
                    }
                }
                #endregion
            }
            else
            {
                message = "Invalid Request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;

            return View(user);
        }
        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (DataquadEntities dc = new DataquadEntities())
            {
                var v = dc.userDetails.Where(a => a.emailId == emailID).FirstOrDefault();
                return v != null;
            }
        }
        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/DataQuadInc/User/" + emailFor + "/" + activationCode;
            
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("rushikeshchikka@gmail.com", "Dotnet Awesome");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "9866920223"; // Replace with actual password
            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created!";
                body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                    " successfully created. Please click on the below link to verify your account" +
                    " <br/><br/><a href=" + link + ">Activate the Account</a>";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi,<br/><br/>We got request for reset your account password. Please click on the below link to reset your password" +
                    "<br/><br/><a href=" + link + ">Reset Password link</a>";
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (DataquadEntities dc = new DataquadEntities())
            {
                dc.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                                                                // Confirm password does not match issue on save changes
                var v = dc.userDetails.Where(a => a.activationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.isEmailVerified = true;
                    dc.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(userLogin login, string ReturnUrl = "")
        {
            string message = "";
            using (DataquadEntities dc = new DataquadEntities())
            {
                var v = dc.userDetails.Where(a => a.emailId == login.emailId).FirstOrDefault();
                if (v != null)
                {
                    if (!v.isEmailVerified)
                    {
                        ViewBag.Message = "Please verify your email first";
                        return View();
                    }
                    if (string.Compare(Crypto.Hash(login.password), v.password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
                        var ticket = new FormsAuthenticationTicket(login.emailId, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);


                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            using(DataquadEntities db = new DataquadEntities())
                            {
                                var xyz = db.userPersonalDetails.Where(x => x.userId == v.userId).FirstOrDefault();
                                if (xyz != null)
                                {
                                    return RedirectToAction("LogOn", "Account", new { ID = xyz.userId });
                                }
                                else
                                {
                                    return RedirectToAction("CreateProfile","Account", new { id = v.userId });
                                }
                            }

                        }
                    }
                    else
                    {
                        message = "Invalid credential provided";
                    }
                }
                else
                {
                    message = "Invalid credential provided";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            //Verify Email ID
            //Generate Reset password link 
            //Send Email 
            string message = "";
            

            using (DataquadEntities dc = new DataquadEntities())
            {
                var account = dc.userDetails.Where(a => a.emailId == EmailID).FirstOrDefault();
                if (account != null)
                {
                    //Send email for reset password
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.emailId, resetCode, "ResetPassword");
                    account.resetPasswordCode = resetCode;
                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                    //in our model class in part 1
                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.SaveChanges();
                    message = "Reset password link has been sent to your email id.";
                }
                else
                {
                    message = "Account not found";
                }
            }
            ViewBag.Message = message;
            return View();
        }
        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            using (DataquadEntities dc = new DataquadEntities())
            {
                var user = dc.userDetails.Where(a => a.resetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (DataquadEntities dc = new DataquadEntities())
                {
                    var user = dc.userDetails.Where(a => a.resetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.password = Crypto.Hash(model.NewPassword);
                        user.resetPasswordCode = "";
                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        message = "New password updated successfully";
                    }
                }
            }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message = message;
            return View(model);
        }
    }
}