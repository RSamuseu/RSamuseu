using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AdvertisingAgency.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using AdvertisingAgency.Filtres;
using AdvertisingAgency.Data;
using AdvertisingAgency.ViewModels;
using System.Threading.Tasks;

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

        readonly adAgencyContext _context;

        public HomeController(adAgencyContext context)
        {
            _context = context;
        }

        public IActionResult Index(string Adtype, SortState sortType = SortState.TypeAsc, int page = 1)
        {
            int pageSize = 3;

            IQueryable<Advedirsments> advedirsments = _context.Advedirsments;

            if (!String.IsNullOrEmpty(Adtype))
            {
                advedirsments = advedirsments.Where(a => a.AdType.Contains(Adtype));
            }

            if (HttpContext.Session.Get("adSession") != null)
            {
                Advedirsments adSession = JsonConvert.DeserializeObject<Advedirsments>(HttpContext.Session.GetString("adSession"));
                ViewBag.adSession = adSession;
            }

            if (HttpContext.Session.Get("sortTypeSession") != null)
            {
                sortType = JsonConvert.DeserializeObject<SortState>(HttpContext.Session.GetString("sortTypeSession"));
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

            int count = advedirsments.Count();
            advedirsments = advedirsments.Skip((page - 1) * pageSize).Take(pageSize);
            ViewBag.advedirsments = advedirsments.ToList();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Advedirsments = advedirsments.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddType(Advedirsments advedirsment)
        {
            _context.Advedirsments.Add(advedirsment);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var adv = await _context.Advedirsments.FindAsync(Convert.ToInt32(id));

            if (adv != null)
            {

                _context.Advedirsments.Remove(adv);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            Advedirsments advedirsment;
            advedirsment = await _context.Advedirsments.FindAsync(Convert.ToInt32(id));
            if (advedirsment == null)
            {
                return NotFound();
            }
            return View(advedirsment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Advedirsments advedirsment)
        {
            if (ModelState.IsValid)
            {
                Advedirsments adv = await _context.Advedirsments.FindAsync(Convert.ToInt32(advedirsment.AdvedirsmentsId));
                if (adv != null)
                {
                    adv.AdType = advedirsment.AdType;
                    adv.AdDescription = advedirsment.AdDescription;

                    _context.Advedirsments.Update(adv);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
