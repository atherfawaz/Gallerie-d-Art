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
        public ActionResult Purchase(int identifier)
        {
            var passingobj = new Parameters();
            List<Art> obj = CRUD.get_Specific_Art(identifier);
            passingobj.identifier = identifier;
            passingobj.obj = obj;
            if (obj == null)
            {
                ViewBag.Message = "Problem Fetching Artwork for purchase";
            }
            return View(passingobj);
        }

        public ActionResult confirm_pur(int u_id, string sub_type, int identifier)
        {
            int error = CRUD.purchase(u_id, identifier, sub_type);
            if(error == -1)
            {
                ViewBag.Message = "Error purchasing";
            }
            else if (error == -2)
            {
                ViewBag.Message = "Error connecting to database!";
            }
            return RedirectToAction("Purchase", "Purchase", new { identifier = identifier });
        }
    }
}