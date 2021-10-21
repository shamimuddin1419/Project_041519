using Dos4PeopleApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Utility
{
    public class LoginCheckActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Controller controller = context.Controller as Controller;
            var user = context.HttpContext.Session.GetObjectFromJson<VmUser>("VmUser");
            if (user == null)
            {
                context.Result = new RedirectToRouteResult
                (
                new RouteValueDictionary(new
                {
                    action = "Index",
                    controller = "Login"
                }));
                return;
            }
            ActionExecutedContext resultContext = await next();
            //else
            //{
            //    context.Result = new 
            //    return;
            //}
            // ActionExecutedContext resultContext = await next();
        }
    }
}
