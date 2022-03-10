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
        [TypeFilter(typeof(LoginCheckActionFilter))]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetIndividualChatList([FromBody] VmChatting objVmChatting)
        {
            try
            {
                Guid ReceiverID = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser").UserId;
                objVmChatting.ReceiverID = ReceiverID;
                List<VmChatting> _objList = await _objChattingDA.GetIndividualChatList(objVmChatting);                
                return Json(new { status = true, data = _objList });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }

        }

        public async Task<JsonResult> InsertIndividualChat([FromBody] VmChatting objChatting)
        {
            VmReturnType _objReturnType = null;
            try
            {
                Guid SenderId = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser").UserId;
                objChatting.SenderID = SenderId;
                _objReturnType = await _objChattingDA.InsertIndividualChat(objChatting);
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
                Guid UserId = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser").UserId;  
                UserList = await _objChattingDA.GetIndividualChatUserList(UserId);   
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

        
        public async Task<JsonResult> GetIndividualUnseenChatListByReceiverId()
        {
            try
            {
                Guid ReceiverID = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser").UserId;               
                List<VmChatting> _objList = await _objChattingDA.GetIndividualUnseenChatListByReceiverId(ReceiverID);
                return Json(new { status = true, data = _objList });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }

        }
        public async Task<JsonResult> IndividualUnseenChatListForAdmin()
        {
            try
            {
                Guid ReceiverID = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser").UserId;
                List<VmChatting> _objList = await _objChattingDA.IndividualUnseenChatListForAdmin(ReceiverID);
                return Json(new { status = true, data = _objList });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }

        }
        public async Task<JsonResult> UpdateIndividualUnseenChatStatus()
        {
            VmReturnType _objReturnType = null;
            try
            {
                VmChatting objChatting = new VmChatting();
                objChatting.ReceiverID = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser").UserId;
                objChatting.SenderID = new Guid("908387e0-7e6d-4308-9e9d-7d2443eff722");
                _objReturnType = await _objChattingDA.UpdateIndividualUnseenChatStatus(objChatting);
                return Json(new { Message = _objReturnType.UserMsg.Trim(), Status = _objReturnType.Status });

            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, Status = false });
            }
        }
        [HttpPost]
        public async Task<JsonResult> UpdateIndividualUnseenChatStatus([FromBody] VmChatting objChatting)
        {
            VmReturnType _objReturnType = null;
            try
            {               
                objChatting.ReceiverID= HttpContext.Session.GetObjectFromJson<VmUser>("VmUser").UserId;
                _objReturnType = await _objChattingDA.UpdateIndividualUnseenChatStatus(objChatting);
                return Json(new { Message = _objReturnType.UserMsg.Trim(), Status = _objReturnType.Status });

            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, Status = false });
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetIndividualChatListForReceiver([FromBody] VmChatting objVmChatting)       
        {
            try
            {
                objVmChatting = new VmChatting();
                Guid ReceiverID = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser").UserId;
                objVmChatting.ReceiverID = ReceiverID;
                objVmChatting.SenderID = new Guid("908387e0-7e6d-4308-9e9d-7d2443eff722");
                List<VmChatting> _objList = await _objChattingDA.GetIndividualChatListForReceiver(objVmChatting);
                return Json(new { status = true, data = _objList });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }

        }
    
        public async Task<JsonResult> InsertIndividualChatForuser([FromBody] VmChatting objChatting)
        {
            VmReturnType _objReturnType = null;
            try
            {
                Guid SenderId = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser").UserId;
                objChatting.SenderID = SenderId;
                objChatting.ReceiverID = new Guid("908387e0-7e6d-4308-9e9d-7d2443eff722");
                _objReturnType = await _objChattingDA.InsertIndividualChat(objChatting);
                return Json(new { Message = _objReturnType.UserMsg.Trim(), Status = _objReturnType.Status });

            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, Status = false });
            }
        }



    }
}
