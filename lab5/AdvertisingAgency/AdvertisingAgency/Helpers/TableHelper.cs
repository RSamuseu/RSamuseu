using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using AdvertisingAgency.Models;
using System.Collections.Generic;

namespace AdvertisingAgency.Helpers
{
    public static class TableHelper
    {
        public static HtmlString CreateCustomerTable(this IHtmlHelper html, IEnumerable<Customers> customers)
        {
            string result = "";
            foreach (Customers item in customers)
            {
                result += "<tr>";
                result += $"<td>{item.CustomerName}</td>";
                result += $"<td>{item.CustomerAddress}</td>";
                result += $"<td>{item.CustomerTelephone}</td>";
                result += $"<td class=\"act\"><form action=\"/Customer/Delete/ " + item.CustomersId + 
                    "\" method=\"post\"><a class=\"btn btn-sm btn-primary\" href=\"/Customer/Edit/" + item.CustomersId + 
                    "\">Изменить</a><button type = \"submit\" class=\"btn btn-sm btn-danger\">Удалить</button></form></td>";
                result += "</tr>";
            }
            return new HtmlString(result);
        }
    }
}
