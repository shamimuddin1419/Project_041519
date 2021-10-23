using Dos4PeopleApp.DA;
using Dos4PeopleApp.Models;
using Dos4PeopleApp.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Controllers
{
    public class UserController : Controller
    {
        UserDA _objUserDa = null;
        public UserController()
        {
            _objUserDa = new UserDA();
        }
        public  IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> InsertUser([FromBody] VmUser objVmUser)
        {
            VmReturnType _objReturnType = null;
            try
            {
                if (objVmUser.UserName == null || objVmUser.UserName == "")
                {
                    return Json(new { Message = "Provide UserName", Status = false });
                }
                else if (objVmUser.Email == null || objVmUser.Email == "")
                {
                    return Json(new { Message = "Provide Email", Status = false });
                }
                else if (!Validation.EmailValidation(objVmUser.Email))
                {
                    return Json(new { Message = "Provide valid email address", Status = false });
                }
                else if (objVmUser.Password == null || objVmUser.Password == "")
                {
                    return Json(new { Message = "Provide Password", Status = false });
                }
                else if (objVmUser.ConfirmPassword == null || objVmUser.ConfirmPassword == "")
                {
                    return Json(new { Message = "Provide Confirm Password", Status = false });
                }
                else if (objVmUser.Password != objVmUser.ConfirmPassword)
                {
                    return Json(new { Message = "Password and it's confirmation doesn't match", Status = false });
                }               
                else
                {
                    _objReturnType = await _objUserDa.InsertUser(objVmUser);
                    return Json(new { Message = _objReturnType.UserMsg.Trim(), Status = _objReturnType.Status });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, Status = false });
            }
        }

       
    }
}
