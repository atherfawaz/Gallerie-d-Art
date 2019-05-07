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
        public static int min_index = 1;
        // GET: DisplayArtwork
        public ActionResult DisplayArtworkPage(int identifier, string err_msg = "")
        {
            
            identifier = (identifier - 10) / 123 - 59;
            
            int aa = CRUD.countArt();
            if (aa >= identifier && identifier > 0)
            {
                var passingobj = new Parameters();
                List<Art> obj = CRUD.get_Specific_Art(identifier);
                passingobj.identifier = identifier;
                passingobj.rating = CRUD.getAvgRating(identifier);
                passingobj.obj = obj;

                passingobj.purCheck = CRUD.checkPurchase(identifier);

                User u = HttpContext.Session[HttpContext.Session.SessionID] as User;
                if (u != null)
                    passingobj.error = CRUD.owns_art(u.user_id, identifier);

                List<Art> sim = CRUD.get_Similar(obj[0].Origin, identifier);
                passingobj.sim = sim;
                if (obj == null)
                {
                    return RedirectToAction("Index", "Home", new { err_msg = "Problem fetching Artwork display" });
                }
                else
                    ViewBag.Message = err_msg;

                return View(passingobj);
            }
            else
                return RedirectToAction("Index", "Home", new { err_msg = "Artwork not found" });
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public ActionResult RandomArtwork(string err_msg = "")
        {
            int i = RandomNumber(min_index, 20);
            var passingobj = new Parameters();
            List<Art> obj = CRUD.get_Specific_Art(i);
            passingobj.identifier = i;
            passingobj.obj = obj;
            passingobj.rating = CRUD.getAvgRating(i);

            passingobj.purCheck = CRUD.checkPurchase(i);

            List<Art> sim = CRUD.get_Similar(obj[0].Origin, i);
            passingobj.sim = sim;

            User u = HttpContext.Session[HttpContext.Session.SessionID] as User;
            if (u != null)
                passingobj.error = CRUD.owns_art(u.user_id, i);
            if (obj == null)
            {
                ViewBag.Message = "Problem fetching random artwork!";
                return RedirectToAction("Index", "Home");
            }
            else
                ViewBag.Message = err_msg;
            return View(passingobj);
        }

        public ActionResult like_art(int identifier)
        {
            User u = HttpContext.Session[HttpContext.Session.SessionID.ToString()] as User;
            int err = CRUD.like_art(u.user_id, identifier);

            string a = "";
            if (err == -1)
            {
                a = "Artwork already liked!";
            }
            else if (err == -2)
            {
                a = "Error connecting to database!";
            }
            else if (err == -3)
            {
                a = "Unable to like artwork!";
            }
            identifier = ((identifier + 59) * 123) + 10;
            return RedirectToAction("DisplayArtworkPage", "DisplayArtwork", new { identifier, err_msg = a });
        }

        public ActionResult rate_art(int identifier)
        {
            User u = HttpContext.Session[HttpContext.Session.SessionID.ToString()] as User;

            string com = Request["comment"].ToString();
            int err = CRUD.rate_art(u.user_id, 
                                    identifier, 
                                    Convert.ToInt32(Request["rating"].ToString()), 
                                    (com == null) ? "" : com);

            string a = "";
            if (err == -1)
            {
                a = "Artwork already rated!";
            }
            else if (err == -2)
            {
                a = "Error Connecting to Database!";
            }
            else if (err == -3)
            {
                a = "Unable to rate artwork!";
            }
            identifier = ((identifier + 59) * 123) + 10;
            return RedirectToAction("DisplayArtworkPage", "DisplayArtwork", new { identifier, err_msg = a });
        }

        public ActionResult delete_art(int identifier)
        {

            Tuple<int, int> ret = CRUD.delete_art(identifier);
            min_index = ret.Item2;
            int err = ret.Item1;
            identifier = ((identifier + 59) * 123) + 10;
            string a = "";
            if (err == -1)
            {
                a = "Artwork doesn't exist!";
                return RedirectToAction("DisplayArtworkPage", "DisplayArtwork", new { identifier, err_msg = a });
            }
            else if (err == -2)
            {
                a = "Error Connecting to Database!";
                return RedirectToAction("DisplayArtworkPage", "DisplayArtwork", new { identifier, err_msg = a });
            }
            else if (err == -3)
            {
                a = "Unable to delete artwork!";
                return RedirectToAction("DisplayArtworkPage", "DisplayArtwork", new { identifier, err_msg = a });
            }
            return RedirectToAction("Index", "Home");
        }
    }
}