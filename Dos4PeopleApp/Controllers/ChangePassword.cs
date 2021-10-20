using Dos4PeopleApp.DA;
using Dos4PeopleApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dos4PeopleApp.Utility;

namespace Dos4PeopleApp.Controllers
{
    public class ChangePassword : Controller
    {
        UserDA _objUserDa = null;
        VmUser ObjSession = null;
        public IActionResult Index()
        {           
            return View();
        }
        public ChangePassword()
        {
            _objUserDa = new UserDA();

        }
        public async Task<JsonResult> CheckExistingPassword([FromBody] VmUser objVmUser)
        {
            try
            {
                ObjSession = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");
                if (objVmUser.Password == null || objVmUser.Password == "")
                {
                    return Json(new { Message = "Provide Password", Status = false });
                }
                else
                {
                    VmUser _objUser = await _objUserDa.CheckExistingPassword(objVmUser.Password.Trim(), ObjSession.UserName);
                    return Json(new { Message = "", Status = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, Status = false });
            }
        }
        public async Task<JsonResult> ChangeCurrentPassword([FromBody] VmUser objVmUser)
        {
            try
            {
                VmReturnType _objReturnType = null;
                ObjSession = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");               
                if (objVmUser.CurrentPassword == null || objVmUser.CurrentPassword == "")
                {
                    return Json(new { Message = "Provide Current Password", Status = false });
                }               
                else if (objVmUser.Password == null || objVmUser.Password == "")
                {
                    return Json(new { Message = "Provide New Password", Status = false });
                }
                else if (objVmUser.ConfirmPassword == null || objVmUser.ConfirmPassword == "")
                {
                    return Json(new { Message = "Provide Confirm Password", Status = false });
                }
                else if (objVmUser.Password != objVmUser.ConfirmPassword)
                {
                    return Json(new { Message = "New Password and it's confirmation doesn't match", Status = false });
                }
                else
                {
                    objVmUser.UserName = ObjSession.UserName;
                    _objReturnType = await _objUserDa.ChangeCurrentPassword(objVmUser);
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
