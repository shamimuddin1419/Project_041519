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
    public class WithdrawalApprovalController : Controller
    {
        WithdrawalDA _objWithdrawalDa = null;
        VmUser ObjSession = null;
       

        public WithdrawalApprovalController()
        {
            _objWithdrawalDa = new WithdrawalDA();
        }
        [TypeFilter(typeof(AdminLoginCheckAttribute))]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetWithdrawalPendingList()
        {
            List<VmWithdrawal> withdrawalList = null;
            try
            {
                withdrawalList = await _objWithdrawalDa.GetWithdrawalPendingList();
                return Json(new { status = true, data = withdrawalList });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, data = ex.Message });
            }
        }
        public async Task<JsonResult> GetWithdrawalInfoById(int id) // Withdrawal  Id
        {
            List<VmWithdrawal> withdrawalList = new List<VmWithdrawal>();
            try
            {
                withdrawalList = await _objWithdrawalDa.GetWithdrawalPendingList();
                VmWithdrawal withdrawal = withdrawalList.Where(x => x.WithdrawId == id).FirstOrDefault();
                return Json(new { status = true, data = withdrawal });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, data = ex.Message });
            }
        }

        public async Task<JsonResult> GetUserWiseWithdrawBalance(int id)
        {
            VmWithdrawBalance _objWithdrawBalance = new VmWithdrawBalance();
            try
            {
                List<VmWithdrawal> withdrawalList = new List<VmWithdrawal>();
                withdrawalList = await _objWithdrawalDa.GetWithdrawalPendingList();
                VmWithdrawal withdrawal = withdrawalList.Where(x => x.WithdrawId == id).FirstOrDefault();
                _objWithdrawBalance = await _objWithdrawalDa.GetWithdrawBalanceByUserId(withdrawal.UserId);
                return Json(new { status = true, data = _objWithdrawBalance });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, data = ex.Message });
            }
        }


        public async Task<JsonResult> WithdrawalApprove([FromBody] VmWithdrawal objVmWithdrawal)
        {
            VmReturnType _objReturnType = null;
            try
            {
                ObjSession = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");
                objVmWithdrawal.UserId = ObjSession.UserId;
                _objReturnType = await _objWithdrawalDa.WithdrawalApprove(objVmWithdrawal);
                return Json(new { Message = _objReturnType.UserMsg.Trim(), Status = _objReturnType.Status });

            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, Status = false });
            }
        }
        public async Task<JsonResult> WithdrawalReject([FromBody] VmWithdrawal objVmWithdrawal)
        {
            VmReturnType _objReturnType = null;
            try
            {
                ObjSession = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");
                objVmWithdrawal.UserId = ObjSession.UserId;
                _objReturnType = await _objWithdrawalDa.WithdrawalReject(objVmWithdrawal);
                return Json(new { Message = _objReturnType.UserMsg.Trim(), Status = _objReturnType.Status });

            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, Status = false });
            }
        }
    }
}
