using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Controllers
{
    public class HowItWorksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
