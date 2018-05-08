using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace AdvertisingAgency.Filtres
{
    public class LogFilter : Attribute, IResourceFilter
    {
        string fileName;

        ILogger _logger;

        public LogFilter(ILoggerFactory loggerFactory)
        {
            fileName = "logger";
            _logger = loggerFactory.CreateLogger("TimingResourceFilter");
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            _logger.LogInformation($"Path - {context.HttpContext.Request.Path}");
            _logger.LogInformation($"OnResourceExecuted - {DateTime.Now}");
            File.AppendAllText(fileName, $"Path - {context.HttpContext.Request.Path}\n");
            File.AppendAllText(fileName, $"OnResourceExecuted - {DateTime.Now}\n=================\n");
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            _logger.LogInformation($"OnResourceExecuting - {DateTime.Now}");
            File.AppendAllText(fileName, $"OnResourceExecuting - {DateTime.Now}\n");
        }
    }
}
