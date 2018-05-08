using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AdvertisingAgency.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using AdvertisingAgency.Filtres;

namespace AdvertisingAgency.Controllers
{
    [SetToSession]
    public class CustomerController : Controller
    {
        public enum SortState
        {
            NameAsc,    // по имени по возрастанию
            NameDesc,   // по имени по убыванию
            AddressAsc, // по адресу по возрастанию
            AddressDesc,   // по адресу по убыванию
            TelephoneAsc, // по телефону по возрастанию
            TelephoneDesc // по телефону по убыванию
        }

        readonly IMemoryCache _memoryCache;
        readonly adAgencyContext _context;

        public CustomerController(IMemoryCache memoryCache, adAgencyContext context)
        {
            _memoryCache = memoryCache;
            _context = context;
        }

        [ResponseCache(CacheProfileName = "Caching")]
        public IActionResult Customer(string CustomerName, SortState sortCustomer = SortState.NameAsc)
        {
            Customers customersFromMemoryCashe = (Customers)_memoryCache.Get(Request.Path.Value.ToLower());
            ViewData["customersFromMemoryCashe"] = customersFromMemoryCashe;

            if (HttpContext.Session.Get("customerSession") != null)
            {
                Customers customerSession = JsonConvert.DeserializeObject<Customers>(HttpContext.Session.GetString("customerSession"));
                ViewBag.customerSession = customerSession;
            }

            if (HttpContext.Session.Get("customerSortSession") != null)
            {
                sortCustomer = JsonConvert.DeserializeObject<SortState>(HttpContext.Session.GetString("customerSortSession"));
            }

            IQueryable<Customers> customers = _context.Customers;

            if (!String.IsNullOrEmpty(CustomerName))
            {
                customers = customers.Where(c => c.CustomerName.Contains(CustomerName));
            }

            ViewBag.CustomerNameSort = sortCustomer == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewBag.CustomerAddressSort = sortCustomer == SortState.AddressAsc ? SortState.AddressDesc : SortState.AddressAsc;
            ViewBag.CustomerTelephoneSort = sortCustomer == SortState.TelephoneAsc ? SortState.TelephoneDesc : SortState.TelephoneAsc;

            switch (sortCustomer)
            {
                case SortState.NameAsc:
                    customers = customers.OrderBy(c => c.CustomerName);
                    break;
                case SortState.NameDesc:
                    customers = customers.OrderByDescending(c => c.CustomerName);
                    break;
                case SortState.AddressAsc:
                    customers = customers.OrderBy(c => c.CustomerAddress);
                    break;
                case SortState.AddressDesc:
                    customers = customers.OrderByDescending(c => c.CustomerAddress);
                    break;
                case SortState.TelephoneAsc:
                    customers = customers.OrderBy(c => c.CustomerTelephone);
                    break;
                case SortState.TelephoneDesc:
                    customers = customers.OrderByDescending(c => c.CustomerTelephone);
                    break;
                default:
                    customers = customers.OrderBy(c => c.CustomerName);
                    break;
            }

            ViewBag.customers = customers.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AddCustomer(Customers customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();

            return RedirectToAction("Customer");
        }
    }
}