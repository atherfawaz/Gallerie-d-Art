using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Art_Gallery.Models;

namespace Art_Gallery.Controllers
{
    public class SubmitController : Controller
    {
        // GET: Submit
        public ActionResult Submit()
        {
            return View();
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
            if(err == -1)
            {
                ViewBag.Message = "Error connecting to database!";
                return RedirectToAction("Submit", "Submit");
            }
            if (err == -2)
            {
                ViewBag.Message = "Unable to add artwork";
                return RedirectToAction("Submit", "Submit");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}