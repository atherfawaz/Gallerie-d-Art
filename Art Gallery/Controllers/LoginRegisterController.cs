using System;
using System.Web.Mvc;
using Art_Gallery.Models;
using Microsoft.AspNetCore.Http;

namespace Art_Gallery.Controllers
{
    public class LoginRegisterController : Controller
    {

        public ActionResult LoginRegister()
        {
            return View();
        }
        public ActionResult login_User(String email, String password)
        {
            int result = CRUD.Login(email, password);
            if (result == -1)
            {
                ViewBag.Message = "Error connecting to the database!";
                return RedirectToAction("Index", "Home");
            }
            else if (result == -2)
            {
                ViewBag.Message = "Error logging in!";
                return RedirectToAction("LoginRegister", "LoginRegister");
            }
            else if (result == 1)
            {
                ViewBag.Message = "Account not found";
                return RedirectToAction("LoginRegister", "LoginRegister");
            }
            else if (result == 2)
            {
                ViewBag.Message = "Invalid Password";
                return RedirectToAction("LoginRegister", "LoginRegister");
            }

            Tuple<int, string, string> ret = CRUD.get_session_data(email);
            User u = new User(ret.Item1, ret.Item2, email, ret.Item3);

            HttpContext.Session[HttpContext.Session.SessionID.ToString()] = u;
            ViewBag.Message = "Login Successfull!";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Add_User()
        {
            int result = CRUD.Register(Request["fname"].ToString(),
                                       Request["lname"].ToString(),
                                       Request["password"].ToString(),
                                       Request["gender"].ToString() == "Male" ? 'M' : 'F',
                                       Request["email"].ToString(),
                                       Request["dob"].ToString(),
                                       Request["acc_type"].ToString(),
                                       Request["address"].ToString());

            if (result == -1)
            {
                ViewBag.Message = "Error connecting to the database!";
                return RedirectToAction("Index", "Home");
            }
            else if (result == -2)
            {
                ViewBag.Message = "Error Resgistering!";
                return RedirectToAction("LoginRegister", "LoginRegister");
            }
            return RedirectToAction("LoginRegister", "LoginRegister");
        }
    }
}