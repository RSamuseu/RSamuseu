using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AdvertisingAgency.Models;
using Microsoft.Extensions.Caching.Memory;
using AdvertisingAgency.Filtres;
using Newtonsoft.Json;

namespace AdvertisingAgency.Controllers
{
    [SetToSession]
    public class OrderController : Controller
    {
        public enum SortState
        {
            DateOrderAsc,    // по дате договора по возрастанию
            DateOrderDesc,   // по дате договра по убыванию
            PeriodAsc,     // по периоду по возрастанию
            PeriodDesc,   // по периоду по убыванию
            LocationAsc, // по локации по возрастанию
            LocationDesc, // по локации по убыванию
            PriceAsc,        // по цене по убыванию
            PriceDesc       // по цене по убыванию
        }

        readonly IMemoryCache _memoryCache;
        readonly adAgencyContext _context;

        public OrderController(IMemoryCache memoryCache, adAgencyContext context)
        {
            _memoryCache = memoryCache;
            _context = context;
        }

        [ResponseCache(CacheProfileName = "Caching")]
        public IActionResult Order(string DateOrder, SortState sortOrder = SortState.DateOrderAsc )
        {
            Orders ordersFromMemoryCashe = (Orders)_memoryCache.Get(Request.Path.Value.ToLower());
            ViewBag.ordFromMemory = ordersFromMemoryCashe;

            if (Request.Cookies["orderSession"] != null)
            {
                Orders orderSession = JsonConvert.DeserializeObject<Orders>(Request.Cookies["orderSession"].ToString());
                ViewBag.orderSession = orderSession;
            }

            if (HttpContext.Session.Get("orderSortSession") != null)
            {
                sortOrder = JsonConvert.DeserializeObject<SortState>(HttpContext.Session.GetString("orderSortSession"));
            }

            IQueryable<Orders> orders = _context.Orders;
            DateTime dateorder = Convert.ToDateTime(DateOrder);
            if (DateOrder != null)
            {
                orders = orders.Where(o => o.DateOrder.Date.Equals(dateorder));
            }

            ViewBag.DateOrderSort = sortOrder == SortState.DateOrderAsc ? SortState.DateOrderDesc : SortState.DateOrderAsc;
            ViewBag.OrderPeriodSort = sortOrder == SortState.PeriodAsc ? SortState.PeriodDesc : SortState.PeriodAsc;
            ViewBag.OrderLocationSort = sortOrder == SortState.LocationAsc ? SortState.LocationDesc : SortState.LocationAsc;
            ViewBag.OrderCostSort = sortOrder == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;

            switch (sortOrder)
            {
                case SortState.DateOrderAsc:
                    orders = orders.OrderBy(o => o.DateOrder);
                    break;
                case SortState.DateOrderDesc:
                    orders = orders.OrderByDescending(o => o.DateOrder);
                    break;
                case SortState.PeriodAsc:
                    orders = orders.OrderBy(o => o.DateBegin);
                    break;
                case SortState.PeriodDesc:
                    orders = orders.OrderByDescending(o => o.DateEnd);
                    break;
                case SortState.LocationAsc:
                    orders = orders.OrderBy(o => o.Location);
                    break;
                case SortState.LocationDesc:
                    orders = orders.OrderByDescending(o => o.Location);
                    break;
                case SortState.PriceAsc:
                    orders = orders.OrderBy(o => o.OrderCost);
                    break;
                case SortState.PriceDesc:
                    orders = orders.OrderByDescending(o => o.OrderCost);
                    break;
                default:
                    orders = orders.OrderBy(o => o.DateOrder);
                    break;
            }

            ViewBag.orders = orders.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AddOrder(Orders order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return RedirectToAction("Order");
        }
    }
}