using ModelVerifyDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ModelVerifyDemo.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetData(Person p)
        {
            if (!ModelState.IsValid)
            {
                string str = "";

                foreach (var value in ModelState.Values)
                {
                    foreach (ModelError error in value.Errors)
                    {
                        str += error.ErrorMessage;
                    }
                }


                return Json(str);
            }
            return Json(p);
        }
    }
}