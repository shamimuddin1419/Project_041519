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
    public class WithdrawalController : Controller
    {
        VmUser ObjSession = null;
        PackageDA ObjPackageDa = null;
        UserDA _objUserDa = null;
        WithdrawalDA _objWithdrawalDA = null;
        public WithdrawalController()
        {
            ObjPackageDa = new PackageDA();
            _objUserDa = new UserDA();
            _objWithdrawalDA = new WithdrawalDA();
        }
        [TypeFilter(typeof(LoginCheckActionFilter))]
        public IActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> WithdrawalRequest([FromBody] VmWithdrawal objVmWithdrawal)
        {
            try
            {
                VmReturnType _objReturnType = null;
                ObjSession = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");
                objVmWithdrawal.UserId = ObjSession.UserId;
                VmUser _objAssignuser = new VmUser();
                _objAssignuser.UserName = ObjSession.UserName.Trim();
                _objAssignuser.Password = objVmWithdrawal.Password.Trim();
                VmUser _objUser = await _objUserDa.CheckAutehtication(_objAssignuser);
                if (_objUser != null)
                {
                    if (objVmWithdrawal.isCommission == true)
                    {
                        objVmWithdrawal.WithdrawBalanceType = "C";
                    }
                    else
                    {
                        objVmWithdrawal.WithdrawBalanceType = "E";
                    }
                    _objReturnType = await _objWithdrawalDA.WithdrawalRequest(objVmWithdrawal);
                    return Json(new { Message = _objReturnType.UserMsg.Trim(), Status = _objReturnType.Status });
                }
                else
                {
                    return Json(new { Message ="Please Insert Valid Password", Status = false });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, Status = false });
            }
        }

        public async Task<JsonResult> GetWithdrawalListByUserId()
        {
            List<VmWithdrawal> List = new List<VmWithdrawal>();
            try
            {
                ObjSession = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");
                Guid UserId =ObjSession.UserId;
                List = await _objWithdrawalDA.GetWithdrawalListByUserId(UserId);
                return Json(new { status = true, data = List });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, data = ex.Message });
            }
        }

        public async Task<JsonResult> GetWithdrawBalanceByUserId()
        {
            VmWithdrawBalance _objWithdrawBalance = new VmWithdrawBalance();
            try
            {
                ObjSession = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");
                Guid UserId = ObjSession.UserId;
                _objWithdrawBalance = await _objWithdrawalDA.GetWithdrawBalanceByUserId(UserId);
                return Json(new { status = true, data = _objWithdrawBalance });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, data = ex.Message });
            }
        }


    }
}
