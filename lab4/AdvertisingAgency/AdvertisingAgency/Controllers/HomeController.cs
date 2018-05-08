using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AdvertisingAgency.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using AdvertisingAgency.Filtres;

namespace AdvertisingAgency.Controllers
{
    [SetToSession]
    public class HomeController : Controller
    {
        public enum SortState
        {
            TypeAsc,    // по типу по возрастанию
            TypeDesc,   // по типу по убыванию
            DescriptAsc, // по описанию по возрастанию
            DescriptDesc   // по описанию по убыванию
        }

        readonly IMemoryCache _memoryCache;
        readonly adAgencyContext _context;

        public HomeController(IMemoryCache memoryCache, adAgencyContext context)
        {
            _memoryCache = memoryCache;
            _context = context;
        }

        [ResponseCache(CacheProfileName = "Caching")]
        public IActionResult Index(string Adtype, SortState sortType = SortState.TypeAsc)
        {
            Advedirsments adFromMemoryCashe = (Advedirsments)_memoryCache.Get(Request.Path.Value.ToLower());
            ViewBag.adFromMemory = adFromMemoryCashe;

            if (HttpContext.Session.Get("adSession") != null)
            {
                Advedirsments adSession = JsonConvert.DeserializeObject<Advedirsments>(HttpContext.Session.GetString("adSession"));
                ViewBag.adSession = adSession;
            }

            if (HttpContext.Session.Get("sortTypeSession") != null)
            {
                sortType = JsonConvert.DeserializeObject<SortState>(HttpContext.Session.GetString("sortTypeSession"));
            }

            IQueryable<Advedirsments> advedirsments = _context.Advedirsments;

            if (!String.IsNullOrEmpty(Adtype))
            {
                advedirsments = advedirsments.Where(a => a.AdType.Contains(Adtype));
            }

            ViewBag.AdTypeSort = sortType == SortState.TypeAsc ? SortState.TypeDesc : SortState.TypeAsc;
            ViewBag.DesriptionSort = sortType == SortState.DescriptAsc ? SortState.DescriptDesc : SortState.DescriptAsc;

            switch (sortType)
            {
                case SortState.TypeAsc:
                    advedirsments = advedirsments.OrderBy(t => t.AdType);
                    break;
                case SortState.TypeDesc:
                    advedirsments = advedirsments.OrderByDescending(t => t.AdType);
                    break;
                case SortState.DescriptAsc:
                    advedirsments = advedirsments.OrderBy(d => d.AdDescription);
                    break;
                case SortState.DescriptDesc:
                    advedirsments = advedirsments.OrderByDescending(d => d.AdDescription);
                    break;
                default:
                    advedirsments = advedirsments.OrderBy(t => t.AdType);
                    break;
            }

            ViewBag.advedirsments = advedirsments.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AddType(Advedirsments advedirsment)
        {
            _context.Advedirsments.Add(advedirsment);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
