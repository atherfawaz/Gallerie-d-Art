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
        public ActionResult Dashboard()
        {
            User u = HttpContext.Session[HttpContext.Session.SessionID.ToString()] as User;
            Parameters p = new Parameters();
            p.obj = CRUD.get_liked(u.user_id);
            p.sim = CRUD.get_sales(u.user_id);
            p.obj1 = CRUD.get_purchases(u.user_id);

            return View(p);
        }

        public ActionResult remove_art()
        {
            int id = Convert.ToInt32(Request["artid"].ToString());
            int err = CRUD.delete_art(id);
            if (err == -1)
            {
                ViewBag.Message = "Artwork doesn't exist!";
                return RedirectToAction("DisplayArtworkPage", "DisplayArtwork", new { id });
            }
            else if (err == -2)
            {
                ViewBag.Message = "Error Connecting to Database!";
                return RedirectToAction("DisplayArtworkPage", "DisplayArtwork", new { id });
            }
            else if (err == -3)
            {
                ViewBag.Message = "Unable to delete artwork!";
                return RedirectToAction("DisplayArtworkPage", "DisplayArtwork", new { id });
            }
            return RedirectToAction("Index", "Home");
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
            if (err == -1)
            {
                ViewBag.Message = "Error connecting to database!";
                return RedirectToAction("Dashboard", "Dashboard");
            }
            if (err == -2)
            {
                ViewBag.Message = "Unable to add artwork";
                return RedirectToAction("Dashboard", "Dashboard");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}