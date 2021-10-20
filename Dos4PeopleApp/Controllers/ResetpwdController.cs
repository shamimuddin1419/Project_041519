using Dos4PeopleApp.DA;
using Dos4PeopleApp.Models;
using Dos4PeopleApp.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Controllers
{
    public class ResetpwdController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        UserDA _objUserDa = null;
        EmailService _objEmailService = null;
        public ResetpwdController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _objUserDa = new UserDA();
            _objEmailService = new EmailService();
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> SendEmail([FromBody] VmUser objVmUser)
        {
            
            try
            {
                int result = 0;
               if (objVmUser.Email == null || objVmUser.Email == "")
                {
                    return Json(new { Message = "Provide Email", Status = false });
                }
                else if (!Validation.EmailValidation(objVmUser.Email))
                {
                    return Json(new { Message = "Provide valid email address", Status = false });
                }              
                else
                {
                    VmUser _objUser = await _objUserDa.GetUserInfoByEmail(objVmUser.Email.Trim());
                    if (_objUser != null)
                    {
                        result = await _objEmailService.PasswordRecovery(_objUser, _webHostEnvironment);                        
                    } 
                }

                if (result > 0)
                {
                    return Json(new { Message = "Mail send successfully. Please check your email", Status = true });
                }
                else
                {
                    return Json(new { Message = "Mail send Failed", Status = false });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, Status = false });
            }
        }

    }
}
