using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using advertisingAgency.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace advertisingAgency.Controllers
{
    public class HomeController : Controller
    {
        readonly IMemoryCache _memoryCache;
        readonly adAgencyContext _context;

        public HomeController(IMemoryCache memoryCache, adAgencyContext context)
        {
            _memoryCache = memoryCache;
            _context = context;
        }

        [ResponseCache(CacheProfileName = "Caching")]
        public IActionResult Index()
        {
            Advedirsments adFromMemoryCashe = (Advedirsments)_memoryCache.Get(Request.Path.Value.ToLower());

            if (Request.Cookies["coockiesAdvedirsments"] != null)
            {
                Advedirsments adCoockies = JsonConvert.DeserializeObject<Advedirsments>(Request.Cookies["coockiesAdvedirsments"].ToString());
                ViewBag.adCoockies = adCoockies;
            }
            ViewBag.adFromMemory = adFromMemoryCashe;
            ViewBag.advedirsments = _context.Advedirsments.ToList();
            return View();
        }

        [ResponseCache(CacheProfileName = "Caching")]
        public IActionResult Customers()
        {
            Customers customersFromMemoryCashe = (Customers)_memoryCache.Get(Request.Path.Value.ToLower());
            ViewData["customersFromMemoryCashe"] = customersFromMemoryCashe;

            if (HttpContext.Session.Get("CustomersSession") != null)
            {
                string[] customersSessionArray = HttpContext.Session.GetString("CustomersSession").Split(";");
                Customers customerSession = new Customers(customersSessionArray[0], customersSessionArray[1], customersSessionArray[2]);
                ViewBag.customersSession = customerSession;
            }

            ViewBag.customers = _context.Customers.ToList();
            return View();
        }

        [ResponseCache(CacheProfileName = "Caching")]
        public IActionResult Orders()
        {
            Orders ordersFromMemoryCashe = (Orders)_memoryCache.Get(Request.Path.Value.ToLower());

            if (Request.Cookies["coockiesOrders"] != null)
            {
                Orders orders = JsonConvert.DeserializeObject<Orders>(Request.Cookies["coockiesOrders"].ToString());
                ViewBag.orCoockies = orders;
            }

            ViewBag.ordFromMemory = ordersFromMemoryCashe;
            ViewBag.orders = _context.Orders.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AddType(Advedirsments advedirsment)
        {
            CookieOptions cookie = new CookieOptions();
            cookie.Expires = DateTime.Now.AddMinutes(1);

            if (Request.Cookies["coockiesAdvedirsments"] == null)
            {
                string value = JsonConvert.SerializeObject(advedirsment);
                Response.Cookies.Append("coockiesAdvedirsments", value);
            }

            _context.Advedirsments.Add(advedirsment);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddCustomer(Customers customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();

            string sess = customer.CustomerName + ";" + customer.CustomerAddress + ";" + customer.CustomerTelephone;
            HttpContext.Session.SetString("CustomersSession", sess);

            return RedirectToAction("Customers");
        }

        [HttpPost]
        public IActionResult AddOrder(Orders order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return RedirectToAction("Orders");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
