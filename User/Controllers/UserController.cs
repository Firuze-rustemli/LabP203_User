using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using User.Models;

namespace User.Controllers
{
    public class UserController : Controller
    {
        UserEntities db = new UserEntities();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main()
        {
            return View();
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        //register
        [HttpPost]
        public ActionResult Register(Users usr)
        {
           
            if (usr.Firstname != null && usr.Lastname != null && usr.Email != null && usr.Password != null && usr.Confirm!=null && usr.Password == usr.Confirm)
            {
                usr.Password = Crypto.HashPassword(usr.Password);
                usr.Confirm = Crypto.HashPassword(usr.Confirm);
                db.Users.Add(usr);
                db.SaveChanges();
                Session["added"] = "Add olundunuz!";
                return RedirectToAction("Index");
            }


            Session["error"] = "Bosh qoymayin";
            return RedirectToAction("Index");
        }

        //login

        [HttpPost]
        public ActionResult Login(Users usr)
        {
            if(usr.Email!=null && usr.Password != null)
            {
                Users Loginuser = db.Users.FirstOrDefault(u => u.Email == usr.Email);

                if (Crypto.VerifyHashedPassword(Loginuser.Password, usr.Password))
                {
                    Session["Msj"] = "Her shey duzdur";
                    return RedirectToAction("Main");
                }
                else
                {
                    Session["Msj"] = "Email ve ya shifre sehvdir";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                Session["Error"] = "Bosh buraxmayin";
                return RedirectToAction("Index");
            }
        }
    }
}