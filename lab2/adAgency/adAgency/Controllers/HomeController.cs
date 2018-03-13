using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using adAgency.Models;

namespace adAgency.Controllers
{
    public class HomeController : Controller
    {
        static List<Advedirsments> advedirsments = new List<Advedirsments>();
        static List<Customers> customers = new List<Customers>();
        static List<Orders> orders = new List<Orders>();

        public IActionResult Index()
        {
            ViewData["advedirsments"] = advedirsments;

            return View();
        }

        public IActionResult Customers()
        {
            ViewData["customers"] = customers;

            return View();
        }

        public IActionResult Orders()
        {
            ViewData["orders"] = orders;

            return View();
        }

        [HttpPost]
        public IActionResult AddType(Advedirsments advedirsment)
        {
            advedirsments.Add(advedirsment);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddCustomer(Customers customer)
        {
            customers.Add(customer);
            return RedirectToAction("Customers");
        }

        [HttpPost]
        public IActionResult AddOrder(Orders order)
        {
            orders.Add(order);
            return RedirectToAction("Orders");
        }

        [HttpGet]
        public IActionResult Get()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
