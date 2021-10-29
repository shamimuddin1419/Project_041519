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
    public class BuyPlanController : Controller
    {
        VmUser ObjSession = null;
        PackageDA ObjPackageDa = null;
        public BuyPlanController()
        {
            ObjPackageDa = new PackageDA();
        }
        [TypeFilter(typeof(LoginCheckActionFilter))]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> RequestForPaymentAccept([FromBody] VmUserPackageRequest objVmPackageReq)
        {
            try
            {
                VmReturnType _objReturnType = null;
                ObjSession = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");
                objVmPackageReq.UserId = ObjSession.UserId;                
                _objReturnType = await ObjPackageDa.RequestForPaymentAccept(objVmPackageReq);
                return Json(new { Message = _objReturnType.UserMsg.Trim(), Status = _objReturnType.Status });

            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, Status = false });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetPaymentMethodTypeList()
        {
            try
            {
                List<VmPaymentMethodType> paymentMethodTypes = await ObjPackageDa.GetPaymentMethodTypeList();
                var _objList = paymentMethodTypes.Select(x => new
                {
                    id = x.PaymentMethodTypeId,
                    text = x.PaymentMethodTypeName,
                }).ToList();
                return Json(new { status = true, data = _objList });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetPaymentMethodList(int id)
        {
            try
            {
                List<VmPaymentMethod> PaymentMethods = await ObjPackageDa.GetPaymentMethodList();
                var _objList = PaymentMethods.Where(x => x.PaymentMethodTypeId == id)
                    .Select(x => new
                    {
                        id = x.PaymentMethodId,
                        text = x.PaymentMethodName,
                    }).ToList();
                return Json(new { status = true, data = _objList });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }

        }
    }
}
