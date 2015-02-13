using BloodBankSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodBankSystem.Controllers
{
    public class LabAssistantController : Controller
    {
        //
        // GET: /LabAssistant/
        IDonor donorInterface;
        IRegForm regFormInterface;

        BloodBankDbEntities db = new BloodBankDbEntities();

        public LabAssistantController(IDonor d, IRegForm form)
        {
            donorInterface = d;
            regFormInterface = form;
        }

        public ActionResult Index()
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();

            return View();
        }

        public ActionResult RegisterDonor()
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();

            if (TempData["FormStatus"] != null)
            {
                ViewBag.FormStatus = TempData["FormStatus"];
                TempData["FormStatus"] = null;
            }

            return View(db.RegForms.ToList());
        }

        public ActionResult RejectForm(int id)
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();

            if(regFormInterface.remove(db.RegForms.Find(id)))
            {
                TempData["FormStatus"] = "Volunteer (ID: " + id + "), deleted Successfully...";
            }
            return RedirectToAction("RegisterDonor");
        }

        public ActionResult AcceptForm(int id)
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();

            RegForm reg=db.RegForms.Find(id);

            Donor donor = new Donor();
            donor.Name = reg.name;
            donor.cnic = reg.cnic;
            donor.gender = reg.gender;
            donor.regFormID = reg.formID;
            donor.totalDonations = 0;
            donor.DOB = reg.DOB;
            donor.address = reg.address;
            donor.branchID = 1;
            donor.city = reg.city;
            donor.contactNo = reg.contactNo;
            donor.lastDonationTime = null;
            donor.bloodGroup = reg.bloodGroup;
            donor.PicPath = reg.PicPath;

            if (donorInterface.save(donor))
            {
                TempData["FormStatus"] = "New Donor (ID: " + donor.donorID + "), Added Successfully...";
            }
            return RedirectToAction("RegisterDonor");
        }

        public ActionResult ViewDonors()
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();

            return View( db.Donors.ToList() );
        }

        public ActionResult AddBloodTake()
        {
            ViewBag.Title = "BloodBank";
            ViewBag.userName = "User";
            return View();
        }

        public ActionResult ViewBloodQuantity()
        {
            ViewBag.Title = "BloodBank";
            ViewBag.userName = "User";
            return View();
        }

        public ActionResult ViewRequests()
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();

            return View(db.Requests.Where(x => x.reqStatus != 0).ToList());
        }

        public ActionResult ViewDonationLog()
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();
            return View(db.DonationLogs.ToList());
        }

        public ActionResult ChangePassword()
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string oldPass, string newPass)
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();

            User u = db.Users.Find(Session["userName"].ToString());
            if (u.password.Equals(oldPass))
            {
                u.password = newPass;
                if(db.SaveChanges() > 0)
                {
                    ViewBag.isSuccess = true;
                    ViewBag.ChangePasswordStatus = "Password Changed Successfully";
                }
            }
            else
            {
                ViewBag.isSuccess = false;
                ViewBag.ChangePasswordStatus = "Can't Change Password";
            }

            return View();
        }

        public JsonResult CheckPassword(string Password1)
        {
            if (!this.checkSession())
            {
                return this.Json("", JsonRequestBehavior.DenyGet);
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();
            bool status = false;

            User u = db.Users.Find( Session["userName"].ToString() );
            if(u.password.Equals(Password1))
            {
                status = true;
            }
            return this.Json(status, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session["userName"] = null;
            Session["role"] = null;
            return RedirectToAction("Index", "Home");
        }

        private bool checkSession()
        {
            if (Session["userName"] != null && Session["role"].Equals("LabAssistant"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
