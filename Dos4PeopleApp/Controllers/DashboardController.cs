using Dos4PeopleApp.Models;
using Dos4PeopleApp.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            VmUser _objSession = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");
            if (_objSession != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
          
        }
    }
}
