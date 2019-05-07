using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Art_Gallery.Models;

namespace Art_Gallery.Controllers
{
    public class PurchaseController : Controller
    {
        // GET: Purchase
        public ActionResult Purchase(int identifier, string err_msg = "")
        {
            int aa = CRUD.countArt();
            if (aa >= identifier && identifier > 0)
            {
                var passingobj = new Parameters();
                List<Art> obj = CRUD.get_Specific_Art(identifier);
                passingobj.identifier = identifier;
                passingobj.obj = obj;
                if (obj == null)
                {
                    return RedirectToAction("Index", "Home", new { err_msg = "Problem Fetching Artwork for purchase" });
                }
                else
                    ViewBag.Message = err_msg;
                return View(passingobj);
            }
            else
                return RedirectToAction("Index", "Home", new { err_msg = "Artwork not found" });
        }

        public ActionResult confirm_pur(int u_id, string sub_type, int identifier)
        {
            int error = CRUD.purchase(u_id, identifier, sub_type);

            string a = "";
            if(error == -1)
            {
                a = "Error purchasing";
                return RedirectToAction("Purchase", "Purchase", new { identifier, err_msg = a });

            }
            else if (error == -2)
            {
                a = "Error connecting to database!";
                return RedirectToAction("Purchase", "Purchase", new { identifier, err_msg = a });
            }
            identifier = ((identifier + 59) * 123) + 10;
            return RedirectToAction("DisplayArtworkPage", "DisplayArtwork", new { identifier });
        }
    }
}