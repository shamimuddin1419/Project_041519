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
    public class PackageApprovalController : Controller
    {
        PackageDA _objPackageDa = null;
        VmUser ObjSession = null;

        public PackageApprovalController()
        {
            _objPackageDa = new PackageDA();
        }
        [TypeFilter(typeof(LoginCheckActionFilter))]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetUserPackageRequestPendingList()
        {
            List<VmUserPackageRequest> PackageRequestList = null;
            try
            {
                PackageRequestList = await _objPackageDa.GetUserPackageRequestPendingList();
                return Json(new { status = true, data = PackageRequestList });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, data = ex.Message });
            }
        }
        public async Task<JsonResult> GetUserPackageRequestById(int id) // Approval Request Id
        {
            VmUserPackageRequest PackageRequest = null;
            try
            {
                PackageRequest = await _objPackageDa.GetUserPackageRequestById(id);
                return Json(new { status = true, data = PackageRequest });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, data = ex.Message });
            }
        }

        public async Task<JsonResult> UserPackageRequestApprove([FromBody] VmUserPackageRequest objPackageReq)
        {
            VmReturnType _objReturnType = null;
            try
            {
                ObjSession = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");
                objPackageReq.ApprovedBy = ObjSession.UserId;
                _objReturnType = await _objPackageDa.UserPackageRequestApprove(objPackageReq);
                return Json(new { Message = _objReturnType.UserMsg.Trim(), Status = _objReturnType.Status });

            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, Status = false });
            }
        }
        public async Task<JsonResult> UserPackageRequestReject([FromBody] VmUserPackageRequest objPackageReq)
        {
            VmReturnType _objReturnType = null;
            try
            {
                ObjSession = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");
                objPackageReq.RejectBy = ObjSession.UserId;
                _objReturnType = await _objPackageDa.UserPackageRequestReject(objPackageReq);
                return Json(new { Message = _objReturnType.UserMsg.Trim(), Status = _objReturnType.Status });

            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, Status = false });
            }
        }
    }
}
