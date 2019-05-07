using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Art_Gallery.Models;

namespace Art_Gallery.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Dashboard(string err_msg = "")
        {
            User u = HttpContext.Session[HttpContext.Session.SessionID.ToString()] as User;
            Parameters p = new Parameters();
            p.obj = CRUD.get_liked(u.user_id);
            p.sim = CRUD.get_sales(u.user_id);
            p.obj1 = CRUD.get_purchases(u.user_id);
            p.user_table = CRUD.get_All_Users();

            ViewBag.Message = err_msg;
            return View(p);
        }

        public ActionResult sell_art()
        {
            User u = HttpContext.Session[HttpContext.Session.SessionID.ToString()] as User;
            Art art = new Art(0,
                              Request["creator"].ToString(),
                              Request["name"].ToString(),
                              Request["description"].ToString(),
                              Request["medium"].ToString(),
                              Request["origin"].ToString(),
                              Request["picture"].ToString(),
                              Request["date"].ToString(),
                              Convert.ToInt32(Request["price"].ToString()));

            int err = CRUD.sell_art(u.user_id, art);

            string a = "";
            if (err == -1)
            {
                a = "Error connecting to database!";
                return RedirectToAction("Dashboard", "Dashboard", new { err_msg = a });
            }
            if (err == -2)
            {
                a = "Unable to add artwork";
                return RedirectToAction("Dashboard", "Dashboard", new { err_msg = a });
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult remove_User(String identifier)
        {
            var err = CRUD.remove_UserDB(identifier);

            string a = "";
            if (err == -1)
            {
                a = "Error connecting to database";
            }
            else if (err == -2)
            {
                a = "Error deleting the person!";
            }
            return RedirectToAction("Dashboard", "Dashboard", new { err_msg = a });
        }

        public ActionResult makeAdmin(String identifier)
        {
            var err = CRUD.makeAdmin(identifier);

            string a = "";
            if (err == -1)
            {
                a = "Error connecting to database";
            }
            else if (err == -2)
            {
                a = "Error deleting the person!";
            }
            return RedirectToAction("Dashboard", "Dashboard", new { err_msg = a });
        }
    }
}