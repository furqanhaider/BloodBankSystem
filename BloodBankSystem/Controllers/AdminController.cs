using BloodBankSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodBankSystem.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Temp/
        IDonor donorInterface;
        IRegForm regFormInterface;

        BloodBankDbEntities db = new BloodBankDbEntities();

        public AdminController(IDonor d, IRegForm form)
        {
            donorInterface = d;
            regFormInterface = form;
        }

        public ActionResult Index()
        {
            if(!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title ="BloodBank";
            ViewBag.userName = Session["userName"].ToString();
            return View();
        }

        public ActionResult ViewBranches()
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();
            if (TempData["RemoveStatus"] != null)
            {
                ViewBag.RemoveStatus = TempData["RemoveStatus"];
                TempData["RemoveStatus"] = null;
            }

            return View(db.Branches.ToList());
        }

        public ActionResult AddBranch()
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
        public ActionResult AddBranch(Branch br)
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();
            
            db.Branches.Add(br);
            
            if (db.SaveChanges() > 0)
            {
                User mgr=new User();
                mgr.empID="ma_"+br.branchID;
                mgr.password = "123";
                mgr.role = "Manager";

                User lbA = new User();
                lbA.empID = "la_" + br.branchID;
                lbA.password = "123";
                lbA.role = "LabAssistant";

                db.Users.Add(mgr);
                db.Users.Add(lbA);

                if (db.SaveChanges() > 0)
                {
                    ViewBag.Status = true;
                    ViewBag.BranchId = br.branchID;
                }
            }
            else
            {
                ViewBag.Status = false;
            }
            return View();
        }

        public ActionResult RemoveBranch(int id)
        {
            if (!this.checkSession())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "BloodBank";
            ViewBag.userName = Session["userName"].ToString();

            db.Branches.Remove( db.Branches.Find(id) );
            if (db.SaveChanges() > 0 )
            {
                TempData["RemoveStatus"] = "Branch (ID: " + id + "), deleted Successfully...";
            }
            return RedirectToAction("ViewBranches");
        }

        [HttpPost]
        public ActionResult UpdateBranch()
        {
            ViewBag.Title = "BloodBank";
            ViewBag.userName = "User";
            return View(db.Branches.ToList());
        }

        public ActionResult AssignManager()
        {
            ViewBag.Title = "BloodBank";
            ViewBag.userName = "User";
            return View();
        }

        public ActionResult RemoveManager()
        {
            ViewBag.Title = "BloodBank";
            ViewBag.userName = "User";
            return View();
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
            if (Session["userName"] != null && Session["role"].Equals("Admin"))
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
