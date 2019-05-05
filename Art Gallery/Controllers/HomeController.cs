using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Art_Gallery.Models;

namespace Art_Gallery.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Art> list = CRUD.get_Art();
            if (list == null)
            {
                ViewBag.Message = "ArtWork Error!";
            }

            return View(list);
        }

        public ActionResult Team()
        {
            return View();
        }
    }
}