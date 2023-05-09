using Microsoft.AspNetCore.Http;

namespace Util.Common
{
    public class Utilities : IUtil
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Utilities(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetHeaderRequest(EHeaders header)
        {
            _httpContextAccessor.HttpContext.Request.Headers.TryGetValue(header.ToString(), out var headerOut);
            return headerOut;
        }
    }
}

