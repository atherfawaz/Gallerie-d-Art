using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Art_Gallery.Models;
using System.Data;

namespace Art_Gallery.Controllers
{
    public class DisplayArtworkController : Controller
    {
        // GET: DisplayArtwork
        public ActionResult DisplayArtworkPage(int identifier)
        {
            var passingobj = new Parameters();
            List<Art> obj = CRUD.get_Specific_Art(identifier);
            passingobj.identifier = identifier;
            passingobj.obj = obj;

            List<Art> sim = CRUD.get_Similar(obj[0].Origin, identifier);
            passingobj.sim = sim;
            if (obj == null)
            {
                ViewBag.Message = "Problem fetching Artwork display";
                return RedirectToAction("Index", "Home");
            }

            return View(passingobj);
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public ActionResult RandomArtwork()
        {
            int i = RandomNumber(0, 20);
            var passingobj = new Parameters();
            List<Art> obj = CRUD.get_Specific_Art(i);
            passingobj.identifier = i;
            passingobj.obj = obj;

            List<Art> sim = CRUD.get_Similar(obj[0].Origin, i);
            passingobj.sim = sim;
            if (obj == null)
            {
                ViewBag.Message = "Problem fetching random artwork!";
                return RedirectToAction("Index", "Home");
            }
            return View(passingobj);
        }

        public ActionResult like_art(int identifier)
        {
            User u = HttpContext.Session[HttpContext.Session.SessionID.ToString()] as User;
            int err = CRUD.like_art(u.user_id, identifier);
            if (err == -1)
            {
                ViewBag.Message = "Artwork already liked!";
            }
            else if (err == -2)
            {
                ViewBag.Message = "Error connecting to database!";
            }
            else if (err == -3)
            {
                ViewBag.Message = "Unable to like artwork!";
            }
            return RedirectToAction("DisplayArtworkPage", "DisplayArtwork", new { identifier });
        }

        public ActionResult rate_art(int identifier)
        {
            User u = HttpContext.Session[HttpContext.Session.SessionID.ToString()] as User;

            string com = Request["comment"].ToString();
            int err = CRUD.rate_art(u.user_id, 
                                    identifier, 
                                    Convert.ToInt32(Request["rating"].ToString()), 
                                    (com == null) ? "" : com);
            if (err == -1)
            {
                ViewBag.Message = "Artwork already rated!";
            }
            else if (err == -2)
            {
                ViewBag.Message = "Error Connecting to Database!";
            }
            else if (err == -3)
            {
                ViewBag.Message = "Unable to rate artwork!";
            }
            return RedirectToAction("DisplayArtworkPage", "DisplayArtwork", new { identifier });
        }

        public ActionResult delete_art(int identifier)
        {

            int err = CRUD.delete_art(identifier);
            if (err == -1)
            {
                ViewBag.Message = "Artwork doesn't exist!";
                return RedirectToAction("DisplayArtworkPage", "DisplayArtwork", new { identifier });
            }
            else if (err == -2)
            {
                ViewBag.Message = "Error Connecting to Database!";
                return RedirectToAction("DisplayArtworkPage", "DisplayArtwork", new { identifier });
            }
            else if (err == -3)
            {
                ViewBag.Message = "Unable to delete artwork!";
                return RedirectToAction("DisplayArtworkPage", "DisplayArtwork", new { identifier });
            }
            return RedirectToAction("Index", "Home");
        }
    }
}