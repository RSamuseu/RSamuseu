using System;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Newtonsoft.Json;

namespace AdvertisingAgency.Filtres
{
    public class SetToSession: Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.Count > 0)
            {
                string keyStr = "";

                foreach (var item in context.ActionArguments.Keys)
                {
                    keyStr += item;
                }

                string str = "";

                foreach (var item in context.ActionArguments.Values)
                {
                    str += JsonConvert.SerializeObject(item);
                }

                switch (keyStr)
                {
                    case "advedirsments": context.HttpContext.Session.Set("adSession", Encoding.UTF8.GetBytes(str)); break;
                    case "customers": context.HttpContext.Session.Set("customerSession", Encoding.UTF8.GetBytes(str)); break;
                    case "orders": context.HttpContext.Session.Set("orderSession", Encoding.UTF8.GetBytes(str)); break;
                    case "sortType": context.HttpContext.Session.Set("sortTypeSession", Encoding.UTF8.GetBytes(str)); break;
                    case "sortCustomer": context.HttpContext.Session.Set("customerSortSession", Encoding.UTF8.GetBytes(str)); break;
                    case "sortOrder": context.HttpContext.Session.Set("orderSortSession", Encoding.UTF8.GetBytes(str)); break;
                }
            }
        }
    }
}
