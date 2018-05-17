using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AdvertisingAgency.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using AdvertisingAgency.Filtres;
using AdvertisingAgency.Data;
using AdvertisingAgency.ViewModels;
using System.Threading.Tasks;

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

        readonly adAgencyContext _context;

        public CustomerController(adAgencyContext context)
        {
            _context = context;
        }

        public IActionResult Customer(string CustomerName, SortState sortCustomer = SortState.NameAsc, int page = 1)
        {
            int pageSize = 6;

            IQueryable<Customers> customers = _context.Customers;

            if (!String.IsNullOrEmpty(CustomerName))
            {
                customers = _context.Customers.Where(c => c.CustomerName.Contains(CustomerName));
            }

            if (HttpContext.Session.Get("customerSession") != null)
            {
                Customers customerSession = JsonConvert.DeserializeObject<Customers>(HttpContext.Session.GetString("customerSession"));
                ViewBag.customerSession = customerSession;
            }

            if (HttpContext.Session.Get("customerSortSession") != null)
            {
                sortCustomer = JsonConvert.DeserializeObject<SortState>(HttpContext.Session.GetString("customerSortSession"));
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

            int count = customers.Count();
            customers = customers.Skip((page - 1) * pageSize).Take(pageSize);
            ViewBag.customers = customers.ToList();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            CustomerViewModel viewModel = new CustomerViewModel
            {
                PageViewModel = pageViewModel,
                Customers = customers.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddCustomer(Customers customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();

            return RedirectToAction("Customer");
        }


        public async Task<IActionResult> Edit(string id)
        {
            Customers customer;
            customer = await _context.Customers.FindAsync(Convert.ToInt32(id));
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Customers customer)
        {
            if (ModelState.IsValid)
            {
                Customers cust = await _context.Customers.FindAsync(Convert.ToInt32(customer.CustomersId));
                if (cust != null)
                {
                    cust.CustomerName = customer.CustomerName;
                    cust.CustomerAddress = customer.CustomerAddress;
                    cust.CustomerTelephone = customer.CustomerTelephone;
                    _context.Customers.Update(cust);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Customer");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var cust = await _context.Customers.FindAsync(Convert.ToInt32(id));

            if (cust != null)
            {

                _context.Customers.Remove(cust);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Customer");
        }
    }
}