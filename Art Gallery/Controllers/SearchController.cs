using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Art_Gallery.Models;

namespace Art_Gallery.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Search(string err_msg = "")
        {
            ViewBag.Message = err_msg;
            return View();
        }

        public ActionResult ExecuteNameSearch()
        {
            List<Art> result = CRUD.get_Search_Name(Request["name"].ToString());
            if (result == null)
            {
                return RedirectToAction("Search", "Search", new { err_msg = "Failed to connect to database!" });
            }
            else if (!result.Any())
            {
                return RedirectToAction("Search", "Search", new { err_msg = "No Result found!" });
            }
            ViewBag.Message = "";
            return View(result);
        }

        public ActionResult ExecuteOriginSearch()
        {
            List<Art> result = CRUD.get_Search_Origin(Request["origin"].ToString());
            if (result == null)
            {
                return RedirectToAction("Search", "Search", new { err_msg = "Failed to connect to database!" });
            }
            else if (!result.Any())
            {
                return RedirectToAction("Search", "Search", new { err_msg = "No Result found!" });
            }
            ViewBag.Message = "";
            return View(result);
        }
    }
}