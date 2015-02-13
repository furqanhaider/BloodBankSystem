using BloodBankSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodBankSystem.Controllers
{
    public class ManagerController : Controller
    {
        //
        // GET: /Manager/
        IDonor donorInterface;
        IRegForm regFormInterface;

        BloodBankDbEntities db = new BloodBankDbEntities();

        public ManagerController(IDonor d, IRegForm form)
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

        public ActionResult ViewDonors()
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();
            if (TempData["RemoveDonorStatus"] != null)
            {
                ViewBag.RemoveDonorStatus = TempData["RemoveDonorStatus"];
                TempData["RemoveDonorStatus"] = null;
            }

            return View(db.Donors.ToList());
        }

        public ActionResult AddDonor()
        {
            return View();
        }

        public ActionResult RemoveDonor(int id)
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();

            db.Donors.Remove(db.Donors.Find(id));
            if (db.SaveChanges() > 0)
            {
                TempData["RemoveDonorStatus"] = "Donor (ID: " + id + "), deleted Successfully...";
            }
            return RedirectToAction("ViewDonors");
        }

        public ActionResult AddLabAssistant()
        {
            return View();
        }

        public ActionResult RemoveLabAssistant()
        {
            return View();
        }

        public ActionResult Employees()
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();
            if (TempData["RemoveEmployeeStatus"] != null)
            {
                ViewBag.RemoveEmployeeStatus = TempData["RemoveEmployeeStatus"];
                TempData["RemoveEmployeeStatus"] = null;
            }

            return View(db.Employees.ToList());
        }

        public ActionResult RemoveEmployee(int id)
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();

            db.Employees.Remove(db.Employees.Find(id.ToString()));
            if (db.SaveChanges() > 0)
            {
                TempData["RemoveEmployeeStatus"] = "Employee (ID: " + id + "), deleted Successfully...";
            }
            return RedirectToAction("Employees");
        }

        public ActionResult AddEmployee()
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();
            Employee emp = new Employee();
            return View(emp);
        }

        [HttpPost]
        public ActionResult AddEmployee(Employee emp)
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Status = "Wrong Input!!! Try Again ...";
                return View(emp);
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();
            
            emp.empID="emp_"+(db.Employees.Count()+1);
            emp.branchID = Int32.Parse(Session["userName"].ToString().Split('_')[1]);
            db.Employees.Add(emp);
            if(db.SaveChanges() > 0)
            {
                ViewBag.Status = "Successfully Added Employee (ID :"+emp.empID+")";
            }

            
            return View((new Employee()));
        }

        public ActionResult BloodQuantity()
        {
            var q = db.BloodBags.Where(m=>m.branchId==1).ToList();



            return View();
        }

        public ActionResult DonationLog()
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();

            return View(db.DonationLogs.ToList());
        }

        public ActionResult Requests()
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();

            if (TempData["ChangeRequestStatus"] != null)
            {
                ViewBag.ChangeRequestStatus = TempData["ChangeRequestStatus"];
                TempData["ChangeRequestStatus"] = null;
            }

            return View(db.Requests.Where(x=>x.reqStatus!=0).ToList());
        }

        public ActionResult RejectRequest(int id)
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();

            Request req=db.Requests.Find(id);
            req.reqStatus=2;
            if (db.SaveChanges() > 0)
            {
                TempData["ChangeRequestStatus"] = "Request (ID: " + id + "), Rejected Successfully...";
            }
            return RedirectToAction("Requests");
        }

        public ActionResult AcceptRequest(int id)
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();

            Request req = db.Requests.Find(id);
            req.reqStatus = 0;

            DonationLog log = new DonationLog();
            log.requestID = req.requestID;
            log.supplyDate = DateTime.Now.Date;
            log.branchID = 1;
            log.bloodGroup = req.reqBloodGroup;
            log.quantity = req.reqQuantity;
            db.DonationLogs.Add(log);

            if (db.SaveChanges() > 0)
            {
                TempData["ChangeRequestStatus"] = "Request (ID: " + id + "), Accepted Successfully...";
            }
            return RedirectToAction("Requests");
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
                if (db.SaveChanges() > 0)
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

            User u = db.Users.Find(Session["userName"].ToString());
            if (u.password.Equals(Password1))
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
            if (Session["userName"] != null && Session["role"].Equals("Manager"))
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
