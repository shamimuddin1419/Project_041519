using Dos4PeopleApp.DA;
using Dos4PeopleApp.Models;
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
    }
}
