using BusinessLibrary;
using ModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HealthCareLog;
using System.Data;

namespace UserHealthPortal.Controllers
{
    public class DoctorController : Controller
    {
        UserDetailsBusiness UDBusiness = new UserDetailsBusiness();
        // GET: Doctor
        public ActionResult Index()
        {
            try
            {
                if (Session["userModel"] != null)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "MainPage");
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
                return RedirectToAction("Login", "MainPage");
            }

        }

        public ActionResult GetDoctor()
        {
            List<DoctorForTable> doctorList = new List<DoctorForTable>();
            try
            {
                UserDetailsModel udModel = (UserDetailsModel)Session["userModel"];
                //doctorList = UDBusiness.GetDoctorList(udModel.userId);
                doctorList = UDBusiness.GetDoctorListEF(udModel.userId);
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
            return Json(new { data = doctorList },JsonRequestBehavior.AllowGet);
            //return Json(doctorList);
        }

        public ActionResult GetDDLHospital()
        {
            List<string> hospitalList = new List<string>();
            try
            {
                UserDetailsModel udModel = (UserDetailsModel)Session["userModel"];
                /*
                DataTable dt = UDBusiness.GetHospitalForDDL(udModel.userId);
                foreach (DataRow row in dt.Rows)
                {
                    string item = row["HospitalName"].ToString();
                    hospitalList.Add(item);
                }*/
                hospitalList = UDBusiness.GetHospitalForDDLEF(udModel.userId);
                return Json(hospitalList, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                WriteLog write = new WriteLog();
                string message = ex.Message;
                write.WriteLogMessage(message);
                string strMsg = UDBusiness.IngnoreSpecialCharacter(message);
                string script = "<script language=\"javascript\" type=\"text/javascript\">alert('" + strMsg + "');</script>";
                Response.Write(script);
                return RedirectToAction("Login", "MainPage");
            }      
        }

        public ActionResult GetDDlSpecialty()
        {
            List<String> specialtyList = new List<string>();
            try
            {
                /*
                DataTable dt = UDBusiness.GetSpecialist();
                foreach (DataRow row in dt.Rows)
                {
                    string item = row["Specialist"].ToString();
                    specialtyList.Add(item);
                }
                */
                specialtyList = UDBusiness.GetSpecialistEF();
                return Json(specialtyList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog write = new WriteLog();
                string message = ex.Message;
                write.WriteLogMessage(message);
                string strMsg = UDBusiness.IngnoreSpecialCharacter(message);
                string script = "<script language=\"javascript\" type=\"text/javascript\">alert('" + strMsg + "');</script>";
                Response.Write(script);
                return RedirectToAction("Login", "MainPage");
            }
        }
        public void AddNewDoctor(DoctorForTable doctorModel)
        {
            try
            {
                UserDetailsModel udModel = (UserDetailsModel)Session["userModel"];
                DoctorDetailsModel docModel = new DoctorDetailsModel();
                docModel.doctorID = doctorModel.DoctorID;
                docModel.firstName = doctorModel.FirstName;
                docModel.lastName = doctorModel.LastName;
                docModel.email = doctorModel.EmailID;
                docModel.relatedHospital = doctorModel.RelatedHospital;
                docModel.specialty = doctorModel.Specialty;
                docModel.address = doctorModel.Address;
                docModel.contactNumber1 = doctorModel.ContactNumber1;
                docModel.contactNumber2 = doctorModel.ContactNumber2;
                docModel.primaryDoctorMark = doctorModel.PrimaryDoctorMark;
                docModel.userID = udModel.userId;
                //Checking Doctordetails InDataBase
                //bool checkDoctorInDB = UDBusiness.CheckDoctorInDB(docModel.doctorID,docModel.userID);
                bool checkDoctorInDB = UDBusiness.CheckDoctorInDBEF(docModel.doctorID, docModel.userID);
                if (!checkDoctorInDB)
                {
                    //setting model to Doctor DB
                    UDBusiness.AddNewDoctorEF(docModel);
                    //Updating Primary Mark
                    if(docModel.primaryDoctorMark.Equals("Yes"))
                    {
                        //UDBusiness.UpdateDoctorPrimaryMark(docModel.userID, docModel.doctorID);
                        UDBusiness.UpdateDoctorPrimaryMarkEF(docModel.userID, docModel.doctorID);
                    }                   
                }
                else
                {
                    //Update Doctor Details in DB
                    //UDBusiness.UpdateDoctorDetails(docModel, docModel.userID);
                    UDBusiness.UpdateDoctorInEF(docModel, docModel.userID);
                    if (docModel.primaryDoctorMark.Equals("Yes"))
                    {
                        UDBusiness.UpdateDoctorPrimaryMarkEF(docModel.userID, docModel.doctorID);
                    }
                }
               
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
        public void DeleteDoctor(string id)
        {
            try
            {
                UserDetailsModel udModel = (UserDetailsModel)Session["userModel"];
                int doctorID = Int32.Parse(id);
                //UDBusiness.DeleteDoctorDetails(doctorID, udModel.userId);
                UDBusiness.DeleteDoctorDetailsEF(doctorID, udModel.userId);
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
    }
}