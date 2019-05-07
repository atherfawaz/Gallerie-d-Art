using System;
using System.Web.Mvc;
using Art_Gallery.Models;
using Microsoft.AspNetCore.Http;

namespace Art_Gallery.Controllers
{
    public class LoginRegisterController : Controller
    {

        public ActionResult LoginRegister(string err_msg = "")
        {
            ViewBag.Message = err_msg;
            return View();
        }

        public ActionResult login_User(String email, String password)
        {
            int result = CRUD.Login(email, password);

            if (result == -1)
            {
                return RedirectToAction("LoginRegister", "LoginRegister", new { err_msg = "Error connecting to the database!" });
            }
            else if (result == -2)
            {
                return RedirectToAction("LoginRegister", "LoginRegister", new { err_msg = "Error logging in!" });
            }
            else if (result == 1)
            {
                return RedirectToAction("LoginRegister", "LoginRegister", new { err_msg = "Account not found" });
            }
            else if (result == 2)
            {
                return RedirectToAction("LoginRegister", "LoginRegister", new { err_msg = "Invalid Password" });
            }

            Tuple<int, string, string> ret = CRUD.get_session_data(email);
            User u = new User(ret.Item1, ret.Item2, email, ret.Item3);

            HttpContext.Session[HttpContext.Session.SessionID.ToString()] = u;
            
            return RedirectToAction("Index", "Home", new { err_msg = "Logged in successfully"});
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
                return RedirectToAction("LoginRegister", "LoginRegister", new { err_msg = "User already exists!" });
            }
            else if (result == -2)
            {
                return RedirectToAction("LoginRegister", "LoginRegister", new { err_msg = "Error Registering!" });
            }
            else if (result == -3)
            {
                return RedirectToAction("LoginRegister", "LoginRegister", new { err_msg = "Error connecting to the database!" });
            }
            return RedirectToAction("LoginRegister", "LoginRegister");
        }

        public ActionResult logout()
        {
            HttpContext.Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }
    }
}