using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services;

namespace Demo.Pl.Helper
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;

        public CachedAttribute(int timeToLiveInSeconds)
        {
           _timeToLiveInSeconds = timeToLiveInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cachedService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cachedReponse = await cachedService.GetCachedResponseAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedReponse))
            {
                var contentResult = new ContentResult()
                {
                    Content = cachedReponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
          
            var executedEndPointContext = await next();//will Execute the Endpoint
             
            if (executedEndPointContext.Result is OkObjectResult okObjectResult)
            {
                await cachedService.cacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }

        }
         
        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append(request.Path);//Api/Product/

            foreach (var (key, value) in request.Query.OrderBy(X=>X.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
           return keyBuilder.ToString();
        }
    }
}
