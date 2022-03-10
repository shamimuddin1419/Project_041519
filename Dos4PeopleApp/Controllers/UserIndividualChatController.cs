using Dos4PeopleApp.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Controllers
{
    public class UserIndividualChatController : Controller
    {
        [TypeFilter(typeof(LoginCheckActionFilter))]
        public IActionResult Index()
        {
            return View();
        }
    }
}
