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
        public ActionResult Search()
        {
            return View();
        }

        public ActionResult ExecuteNameSearch()
        {
            List<Art> result = CRUD.get_Search_Name(Request["name"].ToString());
            return View(result);
        }

        public ActionResult ExecuteOriginSearch()
        {
            List<Art> result = CRUD.get_Search_Origin(Request["origin"].ToString());
            return View(result);
        }
    }
}