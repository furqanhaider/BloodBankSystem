using BloodBankSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodBankSystem.Controllers
{
    public class HomeController : Controller
    {
        IDonor donorInterface;
        IRegForm regFormInterface;

        BloodBankDbEntities db = new BloodBankDbEntities();

        public HomeController(IDonor d, IRegForm form)
        {
            donorInterface = d;
            regFormInterface = form;
        }

        public ActionResult Index()
        {
            ViewBag.Message = "HomePage";

            return View();
        }

        public ActionResult AboutUs()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult ContactUs()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Volunteer()
        {
            DonorViewModel dvm = new DonorViewModel();
            return View(dvm);
        }

        [HttpPost]
        public ActionResult Volunteer(DonorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.errMsg = "Wrong Input!!! Try Again ...";
                return View(model);
            }

            RegForm rf = new RegForm();

            rf.name = model.Name;
            rf.contactNo = model.ContactNo;
            rf.bloodGroup = model.bloodGroup;
            rf.DOB = model.dob;
            rf.gender = model.gender;
            rf.address = model.Address;
            rf.city = model.City;
            string cnic = model.cnic;
            rf.cnic = cnic;
            rf.PicPath = cnic + '.' + model.Photo.FileName.Split('.')[1];;

            var path = Path.Combine(Server.MapPath("~/Images/Res"), rf.PicPath);
            model.Photo.SaveAs(path);

            db.RegForms.Add(rf);
            db.SaveChanges();

            ViewBag.successMsg = "Request Submitted Successfully, we will contact you shortly!";
            DonorViewModel dvm = new DonorViewModel();
            return View(dvm);
        }

        public ViewResult RequestBlood()
        {
            return View();
        }

        [HttpPost]
        public ViewResult RequestBlood(string requestBloodGroup, string requestQuantity, string requestReqDate, string requestLocation, string requestCity, string recepientName, string recepientContact)
        {
            Branch br = db.Branches.FirstOrDefault(m => m.city.Equals(requestCity));
            if (br == null)
            {
                ViewBag.errMsg = "No Branch found in Requested City!!!";
                return View();
            }
            else
            {
                Recepient rec = new Recepient();
                rec.name = recepientName;
                rec.contact = recepientContact;
                rec.city = requestCity;
                db.Recepients.Add(rec);
                db.SaveChanges();

                Request request = new Request();
                request.reqBloodGroup = requestBloodGroup;
                request.reqQuantity = Int32.Parse(requestQuantity);
                request.requiredDate = DateTime.Parse(requestReqDate);
                request.requestDate = DateTime.Now.Date;
                request.city = requestCity;
                request.reqStatus=1;
                request.location = requestLocation;

                db.Requests.Add(request);
                
                db.SaveChanges();
                ViewBag.infoMsg = "Your Request ID. is '" + request.requestID + "'. Kindly note it for tracking...";
                ViewBag.successMsg="Request Submitted Succesfully.. We will Contact you Shortly...";
                return View();
            }

        }

        public ActionResult TrackRequest()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult TrackRequest(string trackID)
        {
            int id = Int32.Parse(trackID);
            Request req = db.Requests.FirstOrDefault(m=>m.requestID==id);
            if (req != null)
            {
                if (req.reqStatus == 1)
                {
                    ViewBag.waitMsg = "Request is under-concentration...";
                }
                else if (req.reqStatus == 2)
                {
                    ViewBag.rejectMsg = "Request has been Rejected...";
                }
                else if (req.reqStatus == 0)
                {
                    ViewBag.successMsg = "Request has been Accepted and Donation has been made at " + req.supplyDate;
                }
            }
            else
            {
                ViewBag.errorMsg = "No Request Found";
            }
            return View();
        }

        public ActionResult SearchBlood()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SearchBlood(string searchBloodGroup, string searchCity, string searchQuantity)
        {
            Branch br = db.Branches.FirstOrDefault(m=>m.city.Equals(searchCity));
            
            if (br != null)
            {
                var bg = db.BloodBags.Where(m => m.branchId == br.branchID && m.bloodGroup.Equals(searchBloodGroup)).FirstOrDefault();
                if (bg != null)
                {
                    ViewBag.SearchStatus = "BloodBags for Bloodgroup: " + searchBloodGroup + " is available";
                    return View();
                }
                else
                {
                    ViewBag.SearchStatusErr = "BloodBags for Bloodgroup: " + searchBloodGroup + " is not available, Try Again Later";
                    return View();
                }
            }
            else
            {
                TempData["SearchStatus"] = "No Branch Found in " + searchCity;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult VerifyLogin(string userName, string password)
        {
            User user = db.Users.Find(userName);

            if (user != null)
            {
                if (user.password.Equals(password))
                {
                    if (user.role.Equals("Admin"))
                    {
                        Session["userName"]=userName;
                        Session["role"]="Admin";
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (user.role.Equals("Manager"))
                    {
                        Session["userName"] = userName;
                        Session["role"] = "Manager";
                        return RedirectToAction("Index", "Manager");
                    }
                    else if (user.role.Equals("LabAssistant"))
                    {
                        Session["userName"] = userName;
                        Session["role"] = "LabAssistant";
                        return RedirectToAction("Index", "LabAssistant");
                    }
                    else
                    {
                        TempData["LogInStatus"] = "Not Authorized";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["LogInStatus"] = "Wrong Username/Password";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["LogInStatus"] = "No User Exists";
                return RedirectToAction("Index");
            }
        }
    }
}
