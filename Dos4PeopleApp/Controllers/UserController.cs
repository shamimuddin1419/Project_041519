using Dos4PeopleApp.DA;
using Dos4PeopleApp.Models;
using Dos4PeopleApp.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Dos4PeopleApp.Controllers
{
    public class UserController : Controller
    {
        UserDA _objUserDa = null;
        private IHostingEnvironment Environment;
        public UserController(IHostingEnvironment _environment)
        {
            _objUserDa = new UserDA();
            Environment = _environment;
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
                else if (objVmUser.Mobile == null || objVmUser.Mobile == "")
                {
                    return Json(new { Message = "Provide  Mobile", Status = false });
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
                else if( objVmUser.CountryId == null || objVmUser.CountryId == "0")
                {
                    return Json(new { Message = "Provide Country", Status = false });
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
        public async Task<JsonResult> GetCountries() {
            try
            {
                List<VMCountry> countries = await _objUserDa.GetCountry();
                return Json(new { success = true, data = countries });
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, Status = false });
            }
        }
        [TypeFilter(typeof(LoginCheckActionFilter))]
        public IActionResult UpdateUser()
        {
            return View();
        }
        [TypeFilter(typeof(LoginCheckActionFilter))]
        public async Task<JsonResult> LoadLoggedInUser()
        {
            try
            {
                var user = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");
                VmUser loggedInUser = await _objUserDa.GetUserInfoByUserId(user.UserId);
                return Json(new { success = true, data = loggedInUser });
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, Status = false }); ;
            }
        }
        [TypeFilter(typeof(LoginCheckActionFilter))]
        [HttpPost]
        public async Task<JsonResult> UpdateUserInfo([FromForm] VmUser user)
        {
            try
            {

                VmUser loggedInUser = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");
                user.UserId = loggedInUser.UserId;
                if (user.Image != null)
                {
                    var extention = Path.GetExtension(user.Image.FileName);
                    var fileName = $"{loggedInUser.UserName}{extention}";
                    string folder = Path.Combine(this.Environment.WebRootPath, "Content","UserImages");
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    string route = Path.Combine(folder, fileName);
                    using (var ms = new MemoryStream())
                    {
                        await user.Image.CopyToAsync(ms);
                        var content = ms.ToArray();
                        await System.IO.File.WriteAllBytesAsync(route, content);
                        //File.WriteAllBytesAsync(route, content);
                    }
                    user.ImagePath = $"{loggedInUser.UserName}{extention}";
                }
                
                bool result = await _objUserDa.ModifyUserInfo(user);
                
                loggedInUser.Email = user.Email;
                loggedInUser.Mobile = user.Mobile;
                loggedInUser.FullName = user.FullName;
                loggedInUser.IsSendEmail = user.IsSendEmail;
                loggedInUser.ImagePath = user.ImagePath ?? loggedInUser.ImagePath;
                HttpContext.Session.SetObjectAsJson("VmUser", loggedInUser);
                return Json(new { Success = true, Message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, Success = false }); ;
            }
        }

    }
}
