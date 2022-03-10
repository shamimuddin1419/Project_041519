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
    public class TransactionController : Controller
    {
        private TransactionDA _transactionDA;
        VmUser ObjSession = null;
        public TransactionController()
        {
            _transactionDA = new TransactionDA();
        }
        [TypeFilter(typeof(LoginCheckActionFilter))]
        public async Task<JsonResult> GetIncomeHist(DateTime? fromDate, DateTime? toDate)
        {
            List<VMIncomeHistory> incomeHistory = new List<VMIncomeHistory>();
            try
            {
                ObjSession = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");
                incomeHistory = await _transactionDA.GetIncomeHistoryByUser(ObjSession.UserId,fromDate,toDate);
                return Json(new { status = true, data = incomeHistory });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, data = ex.Message });
            }
        }
        [TypeFilter(typeof(LoginCheckActionFilter))]
        public IActionResult IncomeHistory()
        {
            return View();
        }
    }
}
