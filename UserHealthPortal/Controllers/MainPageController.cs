using BusinessLibrary;
using HealthCareLog;
using ModelLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace UserHealthPortal.Controllers
{
    public class MainPageController : Controller
    {
        string currentEmail = "";
        UserDetailsModel UDModel = new UserDetailsModel();
        UserDetailsBusiness UDBusiness = new UserDetailsBusiness();
        bool flag;
        bool emailVerification;
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SubmitLogin(string nameEmail,string namePassword,string check)
        {
            try
            {
                if(check == "on")
                {
                    //FormsAuthentication.SetAuthCookie(nameEmail,false);                    
                    //Response.Write("Cheecked");
                }
                ViewBag.Credential = "";
                //flag = UDBusiness.CheckCredential(nameEmail, namePassword);
                flag = UDBusiness.CheckCredentialEF(nameEmail, namePassword);
                //emailVerification = UDBusiness.CheckEmailVerification(nameEmail);
                emailVerification = UDBusiness.CheckEmailVerificationEF(nameEmail);

                if (flag && emailVerification)
                {
                    //UDModel = UDBusiness.GetUserDetailsFromCredential(nameEmail);
                    UDModel = UDBusiness.GetUserDetailsFromCredentialEF(nameEmail);
                    Session["userModel"] = UDModel;
                    return RedirectToAction("UserDashBoard");
                }
                else
                {
                    ViewBag.Credential = "Wrong Credential!";
                    return View("Login");
                }
            }
            catch (Exception ex)
            {
                WriteLog write = new WriteLog();
                string message = ex.Message;
                write.WriteLogMessage(message);
                string strMsg = UDBusiness.IngnoreSpecialCharacter(message);
                string script = "<script language=\"javascript\" type=\"text/javascript\">alert('" + strMsg + "');</script>";
                Response.Write(script);
                return View("Login");
            }
            
            
        }
        
        public ActionResult UserDashBoard()
        {
            if (Session["userModel"] != null)
            {
                try
                {
                    UserDetailsModel udModel = (UserDetailsModel)Session["userModel"];
                    //UserDetailsModel userModel = UDBusiness.GetUserDetailsFromCredential(udModel.email);
                    UserDetailsModel userModel = UDBusiness.GetUserDetailsFromCredentialEF(udModel.email);
                    ViewBag.Session = userModel;
                    /*
                    SqlDataReader reader = UDBusiness.GetAdditionalUserDetails(userModel.userId);
                    while (reader.Read())
                    {
                        Session["Address"] = reader[1].ToString();
                        Session["Contact"] = reader[2].ToString();
                    }
                    */
                    List<AdditionalUserDetails> additional = UDBusiness.GetAdditionalUserDetailsEF(userModel.userId);
                    foreach(AdditionalUserDetails item in additional)
                    {
                        Session["Address"] = item.Address;
                        Session["Contact"] = item.Phone;
                    }

                    //ProfileController profile = new ProfileController();
                    //Session["imgURL"] = profile.SetProfilePicture();
                }
                catch(Exception ex)
                {
                    WriteLog write = new WriteLog();
                    string message = ex.Message;
                    write.WriteLogMessage(message);
                    string strMsg = UDBusiness.IngnoreSpecialCharacter(message);
                    string script = "<script language=\"javascript\" type=\"text/javascript\">alert('" + strMsg + "');</script>";
                    Response.Write(script);
                }
                
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult SignUp(string nameUserId,string nameFirstName,string nameLastName,string namePassword,string nameConPassword,string nameEmail)
        {
            try
            {
                UDModel.userId = Int32.Parse(nameUserId);
                UDModel.firstName = nameFirstName;
                UDModel.lastName = nameLastName;
                UDModel.password = namePassword;
                UDModel.email = nameEmail;
                currentEmail = UDModel.email;
                Guid verificationKey = Guid.NewGuid();
                string code = verificationKey + "?" + "email=" + nameEmail;
                SendCodeToEmail(UDModel.firstName, code, UDModel.email);
                //UDBusiness.SetVerificationKey(UDModel.email, "unverified", verificationKey.ToString());
                UDBusiness.SetVerificationKeyEF(UDModel.email, "unverified", verificationKey.ToString());
                //UDBusiness.SetUserData(UDModel);
                UDBusiness.SetUserDataEF(UDModel);
                //UDBusiness.AdditionalUserDetails(UDModel.userId);
                UDBusiness.AdditionalUserDetailsEF(UDModel.userId);
                //UDBusiness.InsertProfilePicture(UDModel.userId, "~/profile.jpg");
                UDBusiness.InsertProfilePictureEF(UDModel.userId, "~/profile.jpg");
            }
            catch(Exception ex)
            {
                WriteLog write = new WriteLog();
                string message = ex.Message;
                write.WriteLogMessage(message);
                string strMsg = UDBusiness.IngnoreSpecialCharacter(message);
                string script = "<script language=\"javascript\" type=\"text/javascript\">alert('" + strMsg + "');</script>";
                Response.Write(script);
            }
            
            return View();          
        }
        protected void SendCodeToEmail(string fName, string code, string emailAddress)
        {
            try
            {
                var client = new SmtpClient("smtp.mailtrap.io", 2525)
                {
                    Credentials = new NetworkCredential("b755faa9098bab", "45c75acaddcc09"),
                    EnableSsl = true
                };
                //string Body = "<a href=\"healthcare.com/Home/verify?verify=" + code + "&email=" + emailAddress + "\"> click here to verify</a>";
                string body = "Hello " + fName + ",";
                body += "<br /><br />Please click the following link to activate your account";
                body += "<br /><a href = '" + string.Format("{0}://{1}/MainPage/Activation/{2}", Request.Url.Scheme, Request.Url.Authority, code) + "'>Click here to activate your account.</a>";
                body += "<br /><br />Thanks";

                client.Send("magisriturajverma@gmail.com", emailAddress, "Verification mail", body);
            }
            catch(Exception ex)
            {
                WriteLog write = new WriteLog();
                string message = ex.Message;
                write.WriteLogMessage(message);
                string strMsg = UDBusiness.IngnoreSpecialCharacter(message);
                string script = "<script language=\"javascript\" type=\"text/javascript\">alert('" + strMsg + "');</script>";
                Response.Write(script);
            }            
        }
        public ActionResult Activation(string email)
        {
            ViewBag.Message = "Invalid Activation code.";
            string vCode = RouteData.Values["id"].ToString();
            if (RouteData.Values["id"] != null)
            {
                //string DBCode = UDBusiness.RetriveVerificationKey(email);
                string DBCode = UDBusiness.RetriveVerificationKeyEF(email);
                if (DBCode.Equals(vCode))
                {
                    //UDBusiness.UpdateEmailVerification(email);
                    UDBusiness.UpdateEmailVerificationEF(email);
                    ViewBag.Message = "Email Activated";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
        
        public ActionResult LogOut()
        {
            Session["userModel"] = null;
            return RedirectToAction("Login");
        }
    }
}