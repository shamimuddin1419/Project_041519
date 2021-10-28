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
        [HttpGet]
        public async Task<JsonResult> GetPackageCategoryList()
        {
            try
            {
                List<VmPackageCategory> packageCategories = await _objPackageDa.GetPackageCategoryList();
                var _objList = packageCategories.Select(x => new
                {
                    id = x.PackageCategoryId,
                    text = x.PackageCategory,
                }).ToList();
                return Json(new { status = true, data = _objList });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }

        }
        [HttpPost]
        public async Task<JsonResult> GetPackageCateMaxLevel([FromBody] VmPackageCategory objPackageCate)
        {
            try
            {
                List<VmPackageCategory> packageCategories = await _objPackageDa.GetPackageCategoryList();
                VmPackageCategory objPackageCateMaxLevel = packageCategories.Where(x => x.PackageCategoryId == objPackageCate.PackageCategoryId).FirstOrDefault();
                return Json(new { status = true, data = objPackageCateMaxLevel });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }

        }
        public async Task<JsonResult> GetPackageList()
        {
            List<VmPackage> PackageList = null;
            try
            {
                PackageList = await _objPackageDa.GetPackageList();
                return Json(new { status = true, data = PackageList });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, data = ex.Message });
            }
        }
        public async Task<JsonResult> GetPublishedPackageList()
        {
            List<VmPackage> PackageList = null;
            try
            {
                PackageList = (await _objPackageDa.GetPackageList()).Where(x=>x.IsActive && x.IsPublished).ToList();
                return Json(new { status = true, data = PackageList });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, data = ex.Message });
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

        public async Task<JsonResult> GetPackageInfoById(int id)
        {
            List<VmPackage> PackageList = null;
            try
            {
                PackageList = await _objPackageDa.GetPackageList();
                VmPackage packageInfo = PackageList.Where(x => x.PackageId == id).FirstOrDefault();
                return Json(new { status = true, data = packageInfo });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, data = ex.Message });
            }
        }

    }
}
