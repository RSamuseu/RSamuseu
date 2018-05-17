using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using AdvertisingAgency.Data;

namespace AdvertisingAgency.DbCacheMiddleware
{
    public class CasheLast
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _memoryCache;

        public CasheLast(RequestDelegate next, IMemoryCache memCache)
        {
            _next = next;
            _memoryCache = memCache;
        }

        public async Task Invoke(HttpContext context, adAgencyContext dbContext)
        {
            string path = context.Request.Path.Value.ToLower();
            object data = null;
            if (path == "/")
                data = dbContext.Advedirsments.Last();
            if (path == "/home/index")
                data = dbContext.Advedirsments.Last();
            if (path == "/customers/customers")
                data = dbContext.Customers.Last();
            if (path == "/orders/orders")
                data = dbContext.Orders.Last();

            var dataTmp = data;
            if (!_memoryCache.TryGetValue(path, out dataTmp))
            {
                _memoryCache.Set(path, data, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(272)));
            }

            await _next.Invoke(context);
        }
    }

    public static class DbCacheExtensions
    {
        public static IApplicationBuilder UseLastElementCache(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CasheLast>();
        }
    }
}
