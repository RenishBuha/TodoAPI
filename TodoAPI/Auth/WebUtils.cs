using TodoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace TodoAPI.Web
{
    public static class WebUtils
    {
        public static string GetRemoteIp(HttpContext context)
        {
            return context.Connection.RemoteIpAddress.ToString();
        }

        //public static DeviceType GetDeviceType(HttpContext context)
        //{
        //    var strToReturn = $"User agent: {context.Request.Headers["User-Agent"]}";
        //    return strToReturn.ToLower().Contains("mobi") ? DeviceType.MOBILE : DeviceType.DESKTOP;
        //}

        public static string GetHost(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("Host", out StringValues host))
            {
                return null;
            }

            var hostname = host.ToString();

            if (hostname.StartsWith("api.") || hostname.StartsWith("s2s."))
            {
                return hostname[4..];
            }

            var domainArtifcates = hostname.Split(".");

            if (domainArtifcates.Length >= 3)
            {
                var domainData = domainArtifcates.Skip(1).ToArray();
                return string.Join(".", domainData);
            }

            return hostname;
        }

        public static string GetOrigin(HttpContext context)
        {
            var origin = context.Request.Headers["Origin"].ToString();

            var domainArtifcates = origin.Split(".");

            if (domainArtifcates.Length >= 3)
            {
                var domainData = domainArtifcates.Skip(1).ToArray();
                return string.Join(".", domainData);
            }

            return origin;
        }
    }
}
