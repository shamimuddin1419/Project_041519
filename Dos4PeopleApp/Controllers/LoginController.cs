﻿using Dos4PeopleApp.DA;
using Dos4PeopleApp.Models;
using Dos4PeopleApp.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Controllers
{
    public class LoginController : Controller
    {
        UserDA _objUserDa = null;
        public LoginController()
        {
            _objUserDa = new UserDA();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> UserLogin([FromBody] VmUser objVmUser)
        {
            string TargetPath = "";
            bool status = false;
            try
            {               
                VmUser _objUser = await _objUserDa.CheckAutehtication(objVmUser);
                if (_objUser != null)
                {
                    TargetPath = "Dashboard/Index";
                    status = true;
                    HttpContext.Session.SetObjectAsJson("VmUser", _objUser); // Set user object within session               
                    return Json(new { status = status, TargetUrl = TargetPath });
                }
                else
                {
                    return Json(new { status = status, TargetUrl = TargetPath });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = status, TargetUrl = TargetPath });
            }
           

        }
        public ActionResult Logout()
        {
            try
            {
                HttpContext.Session.SetObjectAsJson("VmUser", null);
                return RedirectToAction("Index","");
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }


        }
    }
}
