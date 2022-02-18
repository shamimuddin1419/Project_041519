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
    public class AdminIndividualChatController : Controller
    {
        VmChatting _objVmChatting;
        ChattingDA _objChattingDA;
        public AdminIndividualChatController()
        {
            _objVmChatting = new VmChatting();
            _objChattingDA = new ChattingDA();
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetIndividualChatList(VmChatting objVmChatting)
        {
            try
            {
                List<VmChatting> _objList = await _objChattingDA.GetIndividualChatList(objVmChatting);                
                return Json(new { status = true, data = _objList });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }

        }

        public async Task<JsonResult> InsertIndividualChat([FromBody] VmChatting objVmChatting)
        {
            VmReturnType _objReturnType = null;
            try
            {
                Guid SenderId = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser").UserId;
                objVmChatting.SenderID = SenderId;
                _objReturnType = await _objChattingDA.InsertIndividualChat(objVmChatting);
                return Json(new { Message = _objReturnType.UserMsg.Trim(), Status = _objReturnType.Status });

            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, Status = false });
            }
        }

        public async Task<JsonResult> GetIndividualChatUserList([FromBody] VmChatting objVmChatting)
        {
            List<VmUser> UserList = new List<VmUser>();
            try
            {               
                UserList = await _objChattingDA.GetIndividualChatUserList();                
                if (!string.IsNullOrEmpty(objVmChatting.searchValue))
                {
                    UserList = UserList.Where(x => x.FullName.ToLower().Contains(objVmChatting.searchValue.ToLower())
                        || x.UserName.ToLower().Contains(objVmChatting.searchValue.ToLower())
                        || x.Email.ToLower().Contains(objVmChatting.searchValue.ToLower())
                        || x.Mobile.ToLower().Contains(objVmChatting.searchValue.ToLower())                        
                       ).ToList();
                }
              
               
                return Json(new { status = true, data = UserList });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
    }
}
