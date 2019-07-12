using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary;
using ModelLibrary;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using EntityClass;

namespace BusinessLibrary
{
    public class UserDetailsBusiness
    {
        SqlConnection con;
        string cs = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;
        UserDetailsDataAccess UDDataAccess = new UserDetailsDataAccess();
        public void SetUserData(UserDetailsModel UDModel)
        {
            UDDataAccess.SetUserData(UDModel);
        }
        //User Data Setting 
        public void SetUserDataEF(UserDetailsModel UDModel)
        {
            var accessLayer = new AccessLayer();
            accessLayer.SetUserDataEF(UDModel);
        }
        public bool CheckCredential(string email, string password)
        {
            return UDDataAccess.CheckCredential(email, password);
        }
        public bool CheckCredentialEF(string email, string password)
        {
            var accessLayer = new AccessLayer();
            var credentials = accessLayer.CheckCredentialEF(email, password);
            if(credentials.Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public UserDetailsModel GetUserDetailsFromCredential(string email)
        {
            return UDDataAccess.GetUserDetailsFromCredential(email);
        }
        //EF GetUser DetailsData To mmodel for Session Setting
        public UserDetailsModel GetUserDetailsFromCredentialEF(string email)
        {
            List<UserDetailsModel> userList = new List<UserDetailsModel>();
            UserDetailsModel udModel = new UserDetailsModel();
            
            userList=GetUserModelList(email);
            foreach(UserDetailsModel item in userList)
            {
                udModel.userId = item.userId;
                udModel.firstName = item.firstName;
                udModel.lastName = item.lastName;
                udModel.email = item.email;
                udModel.password = item.password;
            }
            return udModel;
        }

        public List<UserDetailsModel> GetUserModelList(string email)
        {
            var accessLayer = new AccessLayer();
            var credential = accessLayer.GetUserDetailsFromCredentialEF(email);
            UserDetailsModel user = new UserDetailsModel();
            return credential.Select(r => new UserDetailsModel()
            {
               userId = (int)r.UserID,
               firstName = r.FirstName,
               lastName = r.LastName,
               email = r.EmailID,
               password = r.Password
            }).ToList();
        }

        public void SetVerificationKey(string email, string status, string code)
        {
            UDDataAccess.SetVerificationCode(email, status, code);
        }
        //EF Verifiaction Status and Key Setup
        public void SetVerificationKeyEF(string email, string status, string code)
        {
            var accessLayer = new AccessLayer();
            accessLayer.SetVerificationKeyEF(email, status, code);
        }
        public string RetriveVerificationKey(string email)
        {
            return UDDataAccess.RetriveVerificationKey(email);
        }
        public string RetriveVerificationKeyEF(string email)
        {
            var accessLayer = new AccessLayer();
            List<EmailVerification> emails = accessLayer.RetriveVerificationKeyEF(email);
            string code = "";
            foreach(EmailVerification item in emails)
            {
                code = item.ActivationCode;
            }
            return code;
        }
        public bool CheckEmailVerification(string email)
        {
            return UDDataAccess.CheckEmailVerification(email);
        }
        //Entity Framework
        public bool CheckEmailVerificationEF(string email)
        {
            List<EmailVerification> emailList = new List<EmailVerification>();
            var accessLayer = new AccessLayer();
            var emailCheck = accessLayer.CheckEmailVerificationEF(email);
            bool result = false;
            emailList = emailCheck.Select(r => new EmailVerification()
            {
                EmailID = r.EmailID,
                VarificationStatus = r.VarificationStatus,
                ActivationCode = r.ActivationCode            
            }).ToList();
            
            foreach(EmailVerification item in emailList)
            {
                if(item.VarificationStatus.Equals("verified"))
                {
                    result = true;
                }
            }
            return result;
        }
        public void DeleteVerificationCode(string email)
        {
            UDDataAccess.DeleteVerificationCode(email);
        }
        public void DeleteCredential(Int32 userID)
        {
            UDDataAccess.DeleteCredential(userID);
        }
        public void UpdateEmailVerification(string email)
        {
            UDDataAccess.UpdateEmailVerification(email);
        }
        public void UpdateEmailVerificationEF(string email)
        {
            var accessLayer = new AccessLayer();
            accessLayer.UpdateEmailVerificationEF(email);
        }
        public SqlDataReader GetUserDetailsFromDB(Int32 userID)
        {
            return UDDataAccess.GetUserDetailsFromDB(userID);
        }
        public void AdditionalUserDetails(Int32 userID)
        {
            UDDataAccess.AdditonUserDetails(userID);
        }
        //Set Addtion User Details
        public void AdditionalUserDetailsEF(Int32 userID)
        {
            var accessLayer = new AccessLayer();
            accessLayer.AdditionalUserDetailsEF(userID);
        }

        public void UpdateAdditionalUserDetails(Int32 userID, string address, Int64 phone)
        {
            UDDataAccess.UpdateAdditonalDetails(userID, address, phone);
        }
        public void UpdateAdditionalUserDetailsEF(Int32 userID, string address, Int64 phone)
        {
            var accessLayer = new AccessLayer();
            accessLayer.UpdateAdditionalUserDetailsEF(userID, address, phone);
        }

        public SqlDataReader GetAdditionalUserDetails(Int32 userID)
        {
            return UDDataAccess.GetAdditionalUserDetails(userID);
        }
        public List<AdditionalUserDetails> GetAdditionalUserDetailsEF(Int32 userID)
        {
            var accessLayer = new AccessLayer();
            var additonal = accessLayer.GetAdditionalUserDetailsEF(userID);
            return additonal.Select(r => new AdditionalUserDetails()
            {
                Address = r.Address,
                Phone = (long)r.PhoneNumber
            }).ToList();
        }
        public void UpdateUserDetailsInCredential(Int32 userID, string firstName, string lastName)
        {
            UDDataAccess.UpdateUserDetailsInCredential(userID, firstName, lastName);
        }
        public void UpdateUserDetailsInCredentialEF(Int32 userID, string firstName, string lastName)
        {
            var accessayer = new AccessLayer();
            accessayer.UpdateUserDetailsInCredentialEF(userID, firstName, lastName);
        }
        public void UpdatePassword(Int32 userID, string password)
        {
            UDDataAccess.UpdatePassword(userID, password);
        }
        public void UpdatePasswordEF(Int32 userID, string password)
        {
            var accessLayer = new AccessLayer();
            accessLayer.UpdatePasswordEF(userID, password);
        }
        public DataTable FetchHospitalDetailsForGrid(Int32 userID)
        {
            return UDDataAccess.FetchHospitalDetailsForGrid(userID);
        }

        public void SetHospitalDetails(HospitalDetailsModel HDModel)
        {
            UDDataAccess.SetHospitalDetails(HDModel);
        }
        //Entity Framework
        
        public void DeleteHospitalDetails(Int32 hospitalID, Int32 userID)
        {
            UDDataAccess.DeleteHospitalDetails(hospitalID, userID);
        }
        public bool CheckHospitalInGrid(Int32 hospitalID)
        {
            return UDDataAccess.CheckHospitalInGrid(hospitalID);
        }
        public SqlDataReader ShowHospitalDataOnModal(Int32 hospitalID, Int32 userID)
        {
            return UDDataAccess.ShowHospitalDataOnModal(hospitalID, userID);
        }
        public void UpdateHospitalDetails(HospitalForTableModel UDHospital)
        {
            UDDataAccess.UpdateHospitalDetails(UDHospital);
        }
        public string GetProfilePicture(Int32 userID)
        {
            return UDDataAccess.GetProfilePicture(userID);
        }
        public string GetProfilePictureEF(Int32 userID)
        {
            var accessLayer = new AccessLayer();
            List<ProfilePicture> profilePic = new List<ProfilePicture>();
            profilePic = accessLayer.GetProfilePictureEF(userID);
            string url = "";
            foreach(ProfilePicture item in profilePic)
            {
                url = item.PictureURL;
            }
            return url;
        }
        public void UpdateProfilePicture(Int32 userID, string url)
        {
            UDDataAccess.UpdateProfilePicture(userID, url);
        }
        public void UpdateProfilePictureEF(Int32 userID, string url)
        {
            var accessLayer = new AccessLayer();
            accessLayer.UpdateProfilePictureEF(userID, url);
        }
        public void InsertProfilePicture(Int32 userID, string url)
        {
            UDDataAccess.InsertProfilePicture(userID, url);
        }
        //Insert ProfileUrl in DB
        public void InsertProfilePictureEF(Int32 userID, string url)
        {
            var accessLayer = new AccessLayer();
            accessLayer.InsertProfilePictureEF(userID, url);
        }
        public DataTable FetchDoctorDetailsForGrid(Int32 userID)
        {
            return UDDataAccess.FetchDoctorDetailsForGrid(userID);
        }
        public void DeleteDoctorDetails(Int32 DoctorID,Int32 userID)
        {
            UDDataAccess.DeleteDoctorDetails(DoctorID,userID);
        }
        public void DeleteDoctorDetailsEF(Int32 doctorID,Int32 userID)
        {
            var accessLayer = new AccessLayer();
            accessLayer.DeleteDoctorDetailsEF(doctorID, userID);
        }
        public void SetDoctorDetails(DoctorDetailsModel DDModel)
        {
            UDDataAccess.SetDoctorDetails(DDModel);
        }
        public DataTable GetSpecialist()
        {
            return UDDataAccess.GetSpecialist();
        }
        public List<string> GetSpecialistEF()
        {
            var accessLayer = new AccessLayer();
            return accessLayer.GetSpecialistEF();
        }
        public DataTable GetDoctor(Int32 userID)
        {
            return UDDataAccess.GetDoctor(userID);
        }
        public void UpdateDoctorDetails(DoctorDetailsModel DDModel, Int32 userID)
        {
            UDDataAccess.UpdateDoctorDetails(DDModel, userID);
        }
        public bool CheckDoctorInDB(Int32 doctoeID, Int32 userID)
        {
            return UDDataAccess.CheckDoctorInDB(doctoeID, userID);
        }
        public bool CheckDoctorInDBEF(Int32 doctorID, Int32 userID)
        {
            var accessLayer = new AccessLayer();
            if(accessLayer.CheckDoctorInDBEF(doctorID,userID).Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public SqlDataReader GetDoctorDetails(Int32 doctorID, Int32 userID)
        {
            return UDDataAccess.GetDoctorDetails(doctorID, userID);
        }
        public List<DoctorDetailsModel> GetAllDoctorList(string sortColumn)
        {
            List<DoctorDetailsModel> listDoctor = new List<DoctorDetailsModel>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "select DoctorID,FirstName,LastName,Address,ContactNumber1 from UserDoctor";
                if (!string.IsNullOrEmpty(sortColumn))
                {
                    query += " order by " + sortColumn;
                }
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DoctorDetailsModel DDModel = new DoctorDetailsModel();
                    DDModel.doctorID = Int32.Parse(reader[0].ToString());
                    DDModel.firstName = reader[1].ToString();
                    DDModel.lastName = reader[2].ToString();
                    DDModel.address = reader[3].ToString();
                    DDModel.contactNumber1 = Int64.Parse(reader[4].ToString());
                    listDoctor.Add(DDModel);
                }

            }
            return listDoctor;
        }
        
        public List<HospitalForTableModel> GetHospitalList(Int32 userID)
        {
            List<HospitalForTableModel> listHospital = new List<HospitalForTableModel>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spSelectDataForHospitalGrid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("userID", userID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HospitalForTableModel HTableModel = new HospitalForTableModel();
                    HTableModel.HospitalID = Int32.Parse(reader[0].ToString());
                    HTableModel.HospitalName = reader[1].ToString();
                    HTableModel.Address = reader[2].ToString();
                    HTableModel.EmailID = reader[3].ToString();
                    HTableModel.Contact1 = Int64.Parse(reader[4].ToString());
                    HTableModel.Contact2 = Int64.Parse(reader[5].ToString());
                    HTableModel.PrimaryMark = reader[6].ToString();
                    listHospital.Add(HTableModel);
                }
            }
            return listHospital;
        }
        //Doctor Detail in List
        public List<DoctorForTable> GetDoctorList(Int32 userID)
        {
            List<DoctorForTable> listDoctor = new List<DoctorForTable>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetDoctorList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("userID", userID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DoctorForTable doctorModel = new DoctorForTable();
                    doctorModel.DoctorID = Int32.Parse(reader[0].ToString());
                    doctorModel.FirstName = reader[1].ToString();
                    doctorModel.LastName = reader[2].ToString();
                    doctorModel.EmailID = reader[3].ToString();
                    doctorModel.RelatedHospital = reader[4].ToString();
                    doctorModel.Specialty = reader[5].ToString();
                    doctorModel.Address = reader[6].ToString();
                    doctorModel.ContactNumber1 = Int64.Parse(reader[7].ToString());
                    doctorModel.ContactNumber2 = Int64.Parse(reader[8].ToString());
                    doctorModel.PrimaryDoctorMark = reader[9].ToString();
                    listDoctor.Add(doctorModel);
                }
            }
            return listDoctor;
        }

        //public SqlDataReader GetReportDetails(Int32 userID)
        public List<ReportModel> GetRepotDetailsOfUser(Int32 userID)
        {
            SqlDataReader reader = UDDataAccess.GetReportDetails(userID);
            List<ReportModel> listReport = new List<ReportModel>();
            while (reader.Read())
            {
                ReportModel reportModel = new ReportModel();
                reportModel.reportID = Int32.Parse(reader[0].ToString());
                reportModel.reportType = reader[1].ToString();
                reportModel.hospital = reader[2].ToString();
                reportModel.doctor = reader[3].ToString();
                reportModel.date = reader[4].ToString();
                //DateTime.ParseExact(reader[4].ToString(), "yyyy-MM-dd",
                //System.Globalization.CultureInfo.InvariantCulture);
                reportModel.upload = reader[5].ToString();
                listReport.Add(reportModel);

            }
            return listReport;
        }
        public List<ReportModel> GetReportBasedOnDate(string fromDate, string toDate, Int32 userID)
        {
            SqlDataReader reader = UDDataAccess.GetReportBasedOnDate(fromDate, toDate, userID);
            ReportModel reportModel = new ReportModel();
            List<ReportModel> listReport = new List<ReportModel>();
            while (reader.Read())
            {
                ReportModel reportModel1 = new ReportModel();
                reportModel1.reportID = Int32.Parse(reader[0].ToString());
                reportModel1.reportType = reader[1].ToString();
                reportModel1.hospital = reader[2].ToString();
                reportModel1.doctor = reader[3].ToString();
                reportModel1.date = reader[4].ToString();
                //DateTime.ParseExact(reader[4].ToString(), "yyyy-MM-dd",
                //                 System.Globalization.CultureInfo.InvariantCulture);
                reportModel1.upload = reader[5].ToString();
                listReport.Add(reportModel1);

            }
            return listReport;

        }
        public void UpdatePrimaryMark(Int32 userID, Int32 hospitalID)
        {
            UDDataAccess.UpdatePrimaryMark(userID, hospitalID);
        }
        public void UpdatePrimaryMarkEF(Int32 userID, Int32 hospitalID)
        {
            var accessLayer = new AccessLayer();
            accessLayer.UpdatePrimaryMarkEF(userID, hospitalID);
        }
        public void UpdateDoctorPrimaryMark(Int32 userID, Int32 doctorID)
        {
            UDDataAccess.UpdateDoctorPrimaryMark(userID, doctorID);
        }//Update Doctor Primary Mark
        public void UpdateDoctorPrimaryMarkEF(Int32 userID, Int32 doctorID)
        {
            var accessLayer = new AccessLayer();
            accessLayer.UpdateDoctorPrimaryMarkED(userID, doctorID);
        }
        public DataTable GetHospitalForDDL(Int32 userID)
        {
            return UDDataAccess.GetHospitalForDDL(userID);
        }
        public List<string> GetHospitalForDDLEF(Int32 userID)
        {
            var accessLayer = new AccessLayer();
            return accessLayer.GetHospitalForDDLEF(userID);
        }
        public DataTable GetDoctorForDDL(Int32 userID)
        {
            return UDDataAccess.GetDoctorForDDL(userID);
        }
        public void UploadReportDetails(ReportModel reportModel)
        {
            UDDataAccess.UploadReportDetails(reportModel);
        }
        public string GetFileURL(Int32 reportID, Int32 userID)
        {
            return UDDataAccess.GetFileURL(reportID, userID);
        }
        public string IngnoreSpecialCharacter(string str)
        {
            string msg = "";
            foreach (char c in str)
            {
                if (c != '\'')
                {
                    msg = msg + c;
                }
            }
            return msg;
        }
        public string ChangeDateFormat(string date)
        {
            string msg = "";
            foreach (char c in date)
            {
                if (c == '/')
                {
                    msg = msg + '-';
                }
                else
                {
                    msg = msg + c;
                }
            }
            return msg;
        }


        //Enity FrameWork

        public void SetHospitalDetailsEF(HospitalDetailsModel hospital)
        {
            var accessLayer = new AccessLayer();
            accessLayer.AddNewHositalEF(hospital);
        }

        public void AddNewDoctorEF(DoctorDetailsModel doctor)
        {
            var accessLayer = new AccessLayer();
            accessLayer.AddNewDoctorEF(doctor);
        }

        public void UpdateDoctorInEF(DoctorDetailsModel docModel , Int32 userID)
        {
            var accessLayer = new AccessLayer();
            UserDoctor udModel = new UserDoctor();
            udModel.DoctorID = docModel.doctorID;
            udModel.FirstName = docModel.firstName;
            udModel.LastName = docModel.lastName;
            udModel.EmailID = docModel.email;
            udModel.RelatedHostpital = docModel.relatedHospital;
            udModel.Specialty = docModel.specialty;
            udModel.Address = docModel.address;
            udModel.ContactNumber1 = docModel.contactNumber1;
            udModel.ContactNumber2 = docModel.contactNumber2;
            udModel.PrimaryDoctorMark = docModel.primaryDoctorMark;
            udModel.UserID = docModel.userID;
            accessLayer.UpdateDoctorInEF(udModel, userID);
        }

        public List<DoctorForTable> GetDoctorListEF(Int32 ID)
        {
            var accessLayer = new AccessLayer();
            var doctors = accessLayer.GetDoctors(ID);
            return doctors.Select(r => new DoctorForTable()
            {
                DoctorID = (int)r.DoctorID,
                FirstName = r.FirstName,
                LastName = r.LastName,
                EmailID = r.EmailID,
                Address = r.Address,
                Specialty = r.Specialty,
                RelatedHospital = r.RelatedHostpital,
                ContactNumber1 = (long)r.ContactNumber1,
                ContactNumber2 = (long)r.ContactNumber2,
                PrimaryDoctorMark = r.PrimaryDoctorMark,
            }).ToList();
        }

        public List<HospitalForTableModel> GetHospitalEF(Int32 ID)
        {
            var accessLayer = new AccessLayer();
            var hospitals = accessLayer.GetHospital(ID);
            return hospitals.Select(r => new HospitalForTableModel()
            {
                HospitalID = (int)r.HospitalID,
                HospitalName = r.HospitalName,
                EmailID = r.EmailID,
                Address = r.Address,
                Contact1 = (long)r.ContactNumber1,
                Contact2 = (long)r.ContactNumber2,
                PrimaryMark = r.PrimaryMark,
                UserID = (int)r.UserID
            }).ToList();
        }

        public void UpdateHospitalDetailsEF(HospitalForTableModel hospital)
        {
            var accessLayer = new AccessLayer();
            accessLayer.UpdateHospitalDetailsEF(hospital);
        }

        //Report
        public List<ReportModel> GetRepotDetailsOfUserEF(Int32 userID)
        {
            var accessLayer = new AccessLayer();
            var reports = accessLayer.GetRepotDetailsOfUserEF(userID);
            return reports.Select(r => new ReportModel()
            {
                reportID = (int)r.ReportID,
                reportType = r.ReportType,
                date = r.Date,
                doctor = r.Doctor,
                hospital = r.Hospital,
                userID = (int)r.UserID,
                upload = r.FileURL
            }).ToList();
        }

        public void AddNewReportEF(ReportModel report)
        {
            var accessLayer = new AccessLayer();
            accessLayer.AddNewReportEF(report);
        }

        public List<ReportModel> GetReportBasedOnDateEF(string fromDate, string toDate, Int32 userID)
        {
            var accessLayer = new AccessLayer();
            var reports = accessLayer.GetReportBasedOnDateEF(fromDate,toDate,userID);
            return reports.Select(r => new ReportModel()
            {
                reportID = (int)r.ReportID,
                reportType = r.ReportType,
                date = r.Date,
                doctor = r.Doctor,
                hospital = r.Hospital,
                userID = (int)r.UserID,
                upload = r.FileURL
            }).ToList();
        }


    }
}
