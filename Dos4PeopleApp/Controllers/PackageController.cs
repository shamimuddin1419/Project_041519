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
    public class PackageController : Controller
    {
        PackageDA _objPackageDa = null;
        VmUser ObjSession = null;
        public PackageController()
        {
            _objPackageDa = new PackageDA();
        }
        [TypeFilter(typeof(LoginCheckActionFilter))]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetPackageCategoryList()
        {
            List<VmPackageCategory> PackageCategoryList = null;
            try
            {
                PackageCategoryList = await _objPackageDa.GetPackageCategoryList();
                return Json(new { status = true, packagecategoryList = PackageCategoryList });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, packagecategoryList = PackageCategoryList });
            }
        }
        public async Task<JsonResult> GetPackageList()
        {
            List<VmPackage> PackageList = null;
            try
            {
                PackageList = await _objPackageDa.GetPackageList();
                return Json(new { status = true, PackageList = PackageList });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, PackageList = PackageList });
            }
        }

        public async Task<JsonResult> InsertPackage([FromBody] VmPackage objPackage)
        {
            VmReturnType _objReturnType = null;
            try
            {
                ObjSession = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");
                objPackage.CreatedBy = ObjSession.UserId;
                _objReturnType = await _objPackageDa.InsertPackage(objPackage);
                return Json(new { Message = _objReturnType.UserMsg.Trim(), Status = _objReturnType.Status });

            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, Status = false });
            }
        }
        public async Task<JsonResult> UpdatePackage([FromBody] VmPackage objPackage)
        {
            VmReturnType _objReturnType = null;
            try
            {
                ObjSession = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");
                objPackage.CreatedBy = ObjSession.UserId;
                _objReturnType = await _objPackageDa.UpdatePackage(objPackage);
                return Json(new { Message = _objReturnType.UserMsg.Trim(), Status = _objReturnType.Status });

            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, Status = false });
            }
        }
        public async Task<JsonResult> DeletePackage([FromBody] VmPackage objPackage)
        {
            VmReturnType _objReturnType = null;
            try
            {
                ObjSession = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");
                objPackage.CreatedBy = ObjSession.UserId;
                _objReturnType = await _objPackageDa.DeletePackage(objPackage);
                return Json(new { Message = _objReturnType.UserMsg.Trim(), Status = _objReturnType.Status });

            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, Status = false });
            }
        }

    }
}
