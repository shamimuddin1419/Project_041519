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
    public class GenerateDailyTaskIncomeController : Controller
    {
        PackageDA _objPackageDa;
        VmUser ObjSession;     

        public GenerateDailyTaskIncomeController()
        {
            _objPackageDa = new PackageDA();            
        }
        [TypeFilter(typeof(AdminLoginCheckAttribute))]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GenerateDailyTaskIncome()
        {
            try
            {
                VmReturnType _objReturnType = new VmReturnType();
                ObjSession = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");
                Guid GeneratedBy = ObjSession.UserId;
                _objReturnType = await _objPackageDa.GenerateDailyTaskIncome(GeneratedBy);
                return Json(new { Message = _objReturnType.UserMsg.Trim(), Status = _objReturnType.Status });
                
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, Status = false });
            }
        }
    }
}
