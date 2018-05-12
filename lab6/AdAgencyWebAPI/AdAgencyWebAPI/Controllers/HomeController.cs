using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AdAgencyWebAPI.Data;
using AdAgencyWebAPI.Models;
using AdAgencyWebAPI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AdAgencyWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly adAgencyContext _context;

        public HomeController(adAgencyContext context)
        {
            _context = context;
        }

        //GET api/values
        [HttpGet]
        [Produces("application/json")]
        public List<OrderViewModels> Get()
        {
            var item = _context.Orders.Include(t => t.Advedirsments).Select(o =>
             new OrderViewModels
             {
                 OrdersId = o.OrdersId,
                 DateOrder = o.DateOrder,
                 DateBegin = o.DateBegin,
                 DateEnd = o.DateEnd,
                 Location = o.Location,
                 OrderCost = o.OrderCost,
                 AdvedirsmentsId = o.AdvedirsmentsId,
                 AdType = o.Advedirsments.AdType,
                 CustomersId = o.CustomersId,
                 CustomerName = o.Customers.CustomerName
             });
            return item.OrderBy(o => o.OrdersId).ToList();
        }

        // GET api/values
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = _context.Orders.FirstOrDefault(o => o.OrdersId == id);
            if (item == null)
                return NotFound();
            return new ObjectResult(item);
        }

        // GET api/values
        [HttpGet("advedirsments")]
        [Produces("application/json")]
        public IEnumerable<Advedirsments> GetAdvedirsments()
        {
            return _context.Advedirsments.ToList();
        }

        // GET api/values
        [HttpGet("customers")]
        [Produces("application/json")]
        public IEnumerable<Customers> GetCustomers()
        {
            return _context.Customers.ToList();
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Orders order)
        {
            if (order == null)
            {
                return BadRequest();
            }

            _context.Orders.Add(order);
            _context.SaveChanges();
            return Ok(order);
        }

        // PUT api/values
        public IActionResult Put([FromBody]Orders order)
        {
            if (order == null)
            {
                return BadRequest();
            }

            if (!_context.Orders.Any(o => o.OrdersId == order.OrdersId))
            {
                return NotFound();
            }

            _context.Update(order);
            _context.SaveChanges();

            return Ok(order);
        }

        // DELETE api/values
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Orders order = _context.Orders.FirstOrDefault(o => o.OrdersId == id);
            if (order == null)
            {
                return NotFound();
            }
            _context.Orders.Remove(order);
            _context.SaveChanges();
            return Ok(order);
        }
    }
}
