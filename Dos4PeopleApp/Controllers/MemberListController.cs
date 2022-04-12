using Dos4PeopleApp.DA;
using Dos4PeopleApp.Models;
using Dos4PeopleApp.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Controllers
{
    public class MemberListController : Controller
    {
        UserDA _objUserDa = null;
        public MemberListController()
        {
            _objUserDa = new UserDA();
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetTotalNumberOfUsers()
        {
            List<VmUser> UserList = new List<VmUser>();
            try
            {
                var user = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");
                UserList = await _objUserDa.GetUserList(user.UserId);
                int totalRows = UserList.Count;               
                return Json(new { success = true, data = totalRows });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, data = ex.Message });
            }
        }
        

        public async Task<JsonResult> GetMemberList()
        {
            List<VmUser> UserList = new List<VmUser>();
            try
            {
                var user = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");
                UserList = await _objUserDa.GetUserList(user.UserId);
                int totalRows = UserList.Count;
                int start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
                int length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
                string searchValue = Request.Form["search[value]"].FirstOrDefault();              
               // var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                int  selectedSortTblHeaderIndex = int.Parse(Request.Form["order[0][column]"]);             
                string sortDirection = Request.Form["order[0][dir]"];
                if (!string.IsNullOrEmpty(searchValue))
                {
                    UserList = UserList.Where(x => x.FullName.ToLower().Contains(searchValue.ToLower())
                        || x.UserName.ToLower().Contains(searchValue.ToLower())
                        || x.Email.ToLower().Contains(searchValue.ToLower())
                        || x.Mobile.ToLower().Contains(searchValue.ToLower())
                        || x.Package.ToLower().Contains(searchValue.ToLower())
                       ).ToList();
                }                
                UserList = UserList.Skip(start).Take(length).ToList();
                if (selectedSortTblHeaderIndex >= 0)
                {
                    string sortColumn = GetTableHeaderByIndex(selectedSortTblHeaderIndex);
                    UserList = UserList.AsQueryable().OrderBy(sortColumn + " " + sortDirection).ToList();
                }
                return Json(new { success = true, data = UserList, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalRows, recordsFiltered = totalRows });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, data = ex.Message });
            }
        }

        private string GetTableHeaderByIndex(int index)
        {
            string[] TableHeader = { "FullName","UserName","Email","Mobile","Sponsored","Package","JoinDate","Duration", "Expire", "Status" };
            return TableHeader[index];
        }
        [TypeFilter(typeof(LoginCheckActionFilter))]
        public IActionResult MyTeam()
        {
            return View();
        }
        [TypeFilter(typeof(LoginCheckActionFilter))]
        public async Task<JsonResult> GetUserTeam()
        {
            List<VMUserTeam> UserList = new List<VMUserTeam>();
            try
            {
                var user = HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");
                UserList = await _objUserDa.GetUserTeam(user.UserId);
                return Json(new { success = true, data = UserList });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, data = ex.Message });
            }
        }
    }
}
