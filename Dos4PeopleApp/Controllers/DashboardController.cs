using Dos4PeopleApp.DA;
using Dos4PeopleApp.Models;
using Dos4PeopleApp.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private DashboardDA dashboardDA;
        public DashboardController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            dashboardDA = new DashboardDA();
        }

        [TypeFilter(typeof(LoginCheckActionFilter))]
        public IActionResult Index()
        {
            return View();

        }
        public async Task<JsonResult> GetDashboardFirstCardData()
        {
            try
            {
                string userId = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser").UserId.ToString();
                VMDashboardFirstCardData dashboardFirstCardData = await dashboardDA.GetDashboardFirstCardData(userId);
                return Json(new { success = true, data = dashboardFirstCardData });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public async Task<JsonResult> GetDashboardGraphData()
        {
            try
            {
                string userId = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser").UserId.ToString();
                List<VMDashboardGraphData> dashboardGraphData = await dashboardDA.GetDashboardGraphData(userId);
                return Json(new { success = true, data = dashboardGraphData });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
