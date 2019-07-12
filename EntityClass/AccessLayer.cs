using ModelLibrary;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityClass
{
    public class AccessLayer
    {
        //UserDoctors
        public List<UserDoctor> GetDoctors(Int32 ID)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var doctors = dbContext.UserDoctors.Where(r => r.UserID == ID).ToList();
                return doctors;
            }
        }
        public void UpdateDoctorInEF(UserDoctor docModel, Int32 userID)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                //var author = .Authors.First(a => a.AuthorId == 1);
                var doctor = dbContext.UserDoctors.First(r => r.UserID == userID && r.DoctorID == docModel.DoctorID);
                doctor.FirstName = docModel.FirstName;
                doctor.LastName = docModel.LastName;
                doctor.EmailID = docModel.EmailID;
                doctor.RelatedHostpital = docModel.RelatedHostpital;
                doctor.Specialty = docModel.Specialty;
                doctor.Address = docModel.Address;
                doctor.ContactNumber1 = docModel.ContactNumber1;
                doctor.ContactNumber2 = docModel.ContactNumber2;
                doctor.PrimaryDoctorMark = docModel.PrimaryDoctorMark;
                try
                {
                    dbContext.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    
                    throw;
                }
            }
        }

        public void AddNewDoctorEF(DoctorDetailsModel docModel)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var newDoctor = new UserDoctor
                {
                    DoctorID = docModel.doctorID,
                    FirstName = docModel.firstName,
                    LastName = docModel.lastName,
                    EmailID = docModel.email,
                    RelatedHostpital = docModel.relatedHospital,
                    Specialty = docModel.specialty,
                    Address = docModel.address,
                    ContactNumber1 = docModel.contactNumber1,
                    ContactNumber2 = docModel.contactNumber2,
                    PrimaryDoctorMark = docModel.primaryDoctorMark,
                    UserID = docModel.userID
                };
                dbContext.UserDoctors.Add(newDoctor);
                dbContext.SaveChanges();

            }
        }


        public List<UserHospital> GetHospital(Int32 ID)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var hospital = dbContext.UserHospitals.Where(r => r.UserID == ID).ToList();
                return hospital;
            }
        }

        public void AddNewHositalEF(HospitalDetailsModel hospital)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var newHospital = new UserHospital
                {
                    HospitalID = hospital.hospitalID,
                    HospitalName = hospital.hospitalName,
                    Address = hospital.address,
                    EmailID = hospital.emailID,
                    ContactNumber1 =hospital.contact1,
                    ContactNumber2 = hospital.contact2,
                    PrimaryMark = hospital.primaryMark,
                    UserID = hospital.userID
                };
                dbContext.UserHospitals.Add(newHospital);
                dbContext.SaveChanges();

            }
        }

        //Update Hospital
        public void UpdateHospitalDetailsEF(HospitalForTableModel hospital)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var hos = dbContext.UserHospitals.First(r => r.UserID == hospital.UserID && r.HospitalID == hospital.HospitalID);
                hos.HospitalID = hospital.HospitalID;
                hos.HospitalName = hospital.HospitalName;
                hos.EmailID = hospital.EmailID;
                hos.Address = hospital.Address;
                hos.ContactNumber1 = hospital.Contact1;
                hos.ContactNumber2 = hospital.Contact2;
                hos.PrimaryMark = hospital.PrimaryMark;
                hos.UserID = hospital.UserID;

                try
                {
                    dbContext.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
            
        }

        //Report
        public List<UserReport> GetRepotDetailsOfUserEF(Int32 ID)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var reports = dbContext.UserReports.Where(r => r.UserID == ID).ToList();
                return reports;
            }
        }
        public void AddNewReportEF(ReportModel report)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var newReport = new UserReport
                {
                    ReportID = report.reportID,
                    ReportType = report.reportType,
                    Date = report.date,
                    Hospital = report.hospital,
                    Doctor = report.doctor,
                    FileURL = report.upload,
                    UserID = report.userID
                };
                dbContext.UserReports.Add(newReport);
                dbContext.SaveChanges();

            }
        }

        public List<UserReport> GetReportBasedOnDateEF(string fromDate, string toDate, Int32 userID)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                DateTime toD = DateTime.Parse(toDate);
                DateTime fromD = DateTime.Parse(fromDate);
                var reports = dbContext.UserReports.Where(r => Convert.ToDateTime(r.Date)>=fromD && Convert.ToDateTime(r.Date)<=toD && r.UserID == userID).ToList();
                return reports;
            }
        }


        //Check Email Verification
        public List<EmailVerification> CheckEmailVerificationEF(string email)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var emails = dbContext.EmailVerifications.Where(r => r.EmailID == email).ToList();
                return emails;
            }
        }
        //Check Credential
        public List<UserCredential> CheckCredentialEF(string email, string password)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var credential = dbContext.UserCredentials.Where(r => r.EmailID == email && r.Password == password).ToList();
                return credential;
            }
        }

        //Gte User Details Data to UserModel
        public List<UserCredential> GetUserDetailsFromCredentialEF(string email)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var credentials = dbContext.UserCredentials.Where(r => r.EmailID == email).ToList();
                return credentials;
            }
        }

        public List<AdditionalUserDetail> GetAdditionalUserDetailsEF(int userID)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var additonal = dbContext.AdditionalUserDetails.Where(r => r.UserID==userID).ToList();
                return additonal;
            }
        }

        //Verification State
        public void SetVerificationKeyEF(string email, string status, string code)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var verificationKey = new EmailVerification
                {
                    EmailID = email,
                    VarificationStatus = status,
                    ActivationCode = code
                };
                dbContext.EmailVerifications.Add(verificationKey);
                dbContext.SaveChanges();
            }
        }

        //Setting User Data
        public void SetUserDataEF(UserDetailsModel UDModel)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var newUserDetails = new UserCredential
                {
                    UserID = UDModel.userId,
                    FirstName = UDModel.firstName,
                    LastName = UDModel.lastName,
                    EmailID = UDModel.email,
                    Password = UDModel.password
                };
                dbContext.UserCredentials.Add(newUserDetails);
                dbContext.SaveChanges();
            }
        }
        //Additional User Details
        public void AdditionalUserDetailsEF(Int32 userID)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var addtional = new AdditionalUserDetail
                {
                    UserID = userID,
                    Address = null,
                    PhoneNumber =null
                };
                dbContext.AdditionalUserDetails.Add(addtional);
                dbContext.SaveChanges();
            }
        }
        public void InsertProfilePictureEF(Int32 userID,string url)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var profile = new ProfilePicture
                {
                    UserID = userID,
                    PictureURL = url
                };
                dbContext.ProfilePictures.Add(profile);
                dbContext.SaveChanges();
            }
        }
        public void UpdateEmailVerificationEF(string email)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var emailUpdate = dbContext.EmailVerifications.First(r => r.EmailID==email);
                emailUpdate.VarificationStatus = "verified";
                dbContext.SaveChanges();         
            }
        }
        public List<EmailVerification> RetriveVerificationKeyEF(string email)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var code = dbContext.EmailVerifications.Where(r => r.EmailID == email).ToList();
                return code;
            }
        }
        //Update User 
        public void UpdateUserDetailsInCredentialEF(Int32 userID, string firstName, string lastName)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var userDetails = dbContext.UserCredentials.First(r => r.UserID == userID);
                userDetails.FirstName=firstName;
                userDetails.LastName = lastName;
                dbContext.SaveChanges();
            }
        }
        //Adition User Details
        public void UpdateAdditionalUserDetailsEF(Int32 userID, string address, Int64 phone)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var userDetails = dbContext.AdditionalUserDetails.First(r => r.UserID == userID);
                userDetails.Address = address;
                userDetails.PhoneNumber = phone;
                dbContext.SaveChanges();
            }
        }
        //Update password
        public void UpdatePasswordEF(Int32 userID, string password)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var userDetails = dbContext.UserCredentials.First(r => r.UserID == userID);
                userDetails.Password = password;
                dbContext.SaveChanges();
            }
        }
        //Update Profile Picture in Database
        public void UpdateProfilePictureEF(Int32 userID, string url)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var userDetails = dbContext.ProfilePictures.First(r => r.UserID == userID);
                userDetails.PictureURL = url;
                dbContext.SaveChanges();
            }
        }
        //Get Profile Picture from DB
        public List<ProfilePicture> GetProfilePictureEF(Int32 userID)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var url = dbContext.ProfilePictures.Where(r => r.UserID == userID).ToList();
                return url;
            }
        }
        //DDL List For Hospital 
        public List<string> GetHospitalForDDLEF(Int32 userID)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var hospitals = dbContext.UserHospitals.Where(r => r.UserID == userID).Select(r => r.HospitalName).ToList();
                return hospitals;
            }
        }
        public List<string> GetSpecialistEF()
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var specialist = dbContext.Specialists.Select(r => r.Specialist1).ToList();
                return specialist;
            }
        }
        //Checking Doctor In DB
        public List<UserDoctor> CheckDoctorInDBEF(Int32 doctorID, Int32 userID)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var doctors = dbContext.UserDoctors.Where(r => r.DoctorID == doctorID && r.UserID == userID).ToList();
                return doctors;
            }
        }
        //Update Doctor Primary Mark
        public void UpdateDoctorPrimaryMarkED(Int32 userID, Int32 doctorID)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                //var d = from doc in dbContext.UserDoctors
                //        where doc.UserID == userID &&
                //        doc.DoctorID == (from dk in dbContext.UserDoctors where dk.DoctorID != doctorID select dk.DoctorID)
                //        select doc;

                var docList = Convert.ToString(dbContext.UserDoctors.Where(r => r.DoctorID != doctorID).Select(r => r.DoctorID));
                //var d = dbContext.UserDoctors.Where(r => r.UserID == userID && Convert.ToString(r.DoctorID).Contains(docList));
                var doctor = dbContext.UserDoctors.Where(r => r.DoctorID != doctorID && r.UserID == userID).ToList();
                //var doctor = dbContext.UserDoctors.Where(r => r.UserID == userID && r.DoctorID == (dbContext.UserDoctors.Where(e => e.DoctorID==doctorID).Select(p => p.DoctorID))).ToList();
                doctor.ForEach(r => r.PrimaryDoctorMark = "No");
            }
        }

        //Deleting Doctor Details from DB
        public void DeleteDoctorDetailsEF(Int32 doctorID, Int32 userID)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var doc = dbContext.UserDoctors.Where(r => r.DoctorID == doctorID && r.UserID == userID).First();
                dbContext.UserDoctors.Remove(doc);
                dbContext.SaveChanges();
            }
        }
        //Update Hospital Primary Mark
        public void UpdatePrimaryMarkEF(Int32 userID, Int32 hospitalID)
        {
            using (var dbContext = new HealthcareDBEntities())
            {
                var doctor = dbContext.UserHospitals.Where(r => r.UserID != userID && r.HospitalID != hospitalID).ToList();
                doctor.ForEach(r => r.PrimaryMark = "No");
            }
        }
    }
}
