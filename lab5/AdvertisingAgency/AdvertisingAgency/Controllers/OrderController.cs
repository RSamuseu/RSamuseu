using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AdvertisingAgency.Models;
using Microsoft.Extensions.Caching.Memory;
using AdvertisingAgency.Filtres;
using Newtonsoft.Json;
using AdvertisingAgency.Data;
using AdvertisingAgency.ViewModels;
using System.Threading.Tasks;

namespace AdvertisingAgency.Controllers
{
    [SetToSession]
    public class OrderController : Controller
    {
        public enum SortState
        {
            DateCustAsc,    // по заказчикам по возрастанию
            DateCustDesc,   // по заказчикам по убыванию
            DateAdvAsc,    // по виду по возрастанию
            DateAdvDesc,   // по виду по убыванию
            DateOrderAsc,    // по дате договора по возрастанию
            DateOrderDesc,   // по дате договра по убыванию
            PeriodAsc,     // по периоду по возрастанию
            PeriodDesc,   // по периоду по убыванию
            LocationAsc, // по локации по возрастанию
            LocationDesc, // по локации по убыванию
            PriceAsc,        // по цене по убыванию
            PriceDesc       // по цене по убыванию
        }

        readonly adAgencyContext _context;

        public OrderController(adAgencyContext context)
        {
            _context = context;
        }

        public IActionResult Order(string DateOrder, SortState sortOrder = SortState.DateOrderAsc, int page = 1)
        {
            int pageSize = 5;

            IQueryable<Orders> orders = _context.Orders;

            DateTime dateorder = Convert.ToDateTime(DateOrder);
            if (DateOrder != null)
            {
                orders = orders.Where(o => o.DateOrder.Date.Equals(dateorder));
            }

            if (Request.Cookies["orderSession"] != null)
            {
                Orders orderSession = JsonConvert.DeserializeObject<Orders>(Request.Cookies["orderSession"].ToString());
                ViewBag.orderSession = orderSession;
            }

            if (HttpContext.Session.Get("orderSortSession") != null)
            {
                sortOrder = JsonConvert.DeserializeObject<SortState>(HttpContext.Session.GetString("orderSortSession"));
            }

            ViewBag.CustOrderSort = sortOrder == SortState.DateCustAsc ? SortState.DateCustDesc : SortState.DateCustAsc;
            ViewBag.AdvPeriodSort = sortOrder == SortState.DateAdvAsc ? SortState.DateAdvDesc : SortState.DateAdvAsc;
            ViewBag.DateOrderSort = sortOrder == SortState.DateOrderAsc ? SortState.DateOrderDesc : SortState.DateOrderAsc;
            ViewBag.OrderPeriodSort = sortOrder == SortState.PeriodAsc ? SortState.PeriodDesc : SortState.PeriodAsc;
            ViewBag.OrderLocationSort = sortOrder == SortState.LocationAsc ? SortState.LocationDesc : SortState.LocationAsc;
            ViewBag.OrderCostSort = sortOrder == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;

            switch (sortOrder)
            {
                case SortState.DateCustAsc:
                    orders = orders.OrderBy(o => o.Customers.CustomerName);
                    break;
                case SortState.DateCustDesc:
                    orders = orders.OrderByDescending(o => o.Customers.CustomerName);
                    break;
                case SortState.DateAdvAsc:
                    orders = orders.OrderBy(o => o.Advedirsments.AdType);
                    break;
                case SortState.DateAdvDesc:
                    orders = orders.OrderByDescending(o => o.Advedirsments.AdType);
                    break;
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

            int count = orders.Count();
            orders = orders.Skip((page - 1) * pageSize).Take(pageSize);
            ViewBag.advedirsments = _context.Advedirsments.ToList();
            ViewBag.customers = _context.Customers.ToList();
            ViewBag.orders = orders.ToList();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            OrderViewModel viewModel = new OrderViewModel
            {
                PageViewModel = pageViewModel,
                Orders = orders.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddOrder(Orders order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return RedirectToAction("Order");
        }


        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.advedirsments = _context.Advedirsments.ToList();
            ViewBag.customers = _context.Customers.ToList();
            Orders order;
            order = await _context.Orders.FindAsync(Convert.ToInt32(id));
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Orders order)
        {
            if (ModelState.IsValid)
            {
                Orders or = await _context.Orders.FindAsync(Convert.ToInt32(order.OrdersId));
                if (or != null)
                {
                    or.AdvedirsmentsId = order.AdvedirsmentsId;
                    or.CustomersId = order.CustomersId;
                    or.DateOrder = order.DateOrder;
                    or.DateBegin = order.DateBegin;
                    or.DateEnd = order.DateEnd;
                    or.Location = order.Location;
                    or.OrderCost = order.OrderCost;
                    _context.Orders.Update(or);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Order");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var or = await _context.Orders.FindAsync(Convert.ToInt32(id));

            var advedirsments = _context.Advedirsments.Where(a => a.AdvedirsmentsId == or.AdvedirsmentsId);
            var customers = _context.Customers.Where(c => c.CustomersId == or.CustomersId);

            if (or != null)
            {
                _context.Orders.Remove(or);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Order");
        }
    }
}