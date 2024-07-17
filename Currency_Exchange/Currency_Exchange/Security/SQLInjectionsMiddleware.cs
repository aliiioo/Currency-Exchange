using Microsoft.Extensions.Primitives;

namespace Currency_Exchange.Security
{
    
    public class SQLInjectionsMiddleware
    {
        private readonly RequestDelegate _next;

        public SQLInjectionsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var query = context.Request.Query.ToDictionary(
                k => k.Key,
                v => CleanInput(v.Value)
            );
            context.Request.QueryString = new QueryString(string.Join("&", query.Select(kvp => $"{kvp.Key}={kvp.Value}")));

            if (context.Request.HasFormContentType)
            {
                var form = await context.Request.ReadFormAsync();
                var formData = form.ToDictionary(
                    k => k.Key,
                    v => CleanInput(v.Value)
                );
                context.Request.Form = new FormCollection(formData);
            }

            await _next(context);
        }

        private StringValues CleanInput(string input)
        {
            return input.Replace("'", "").Replace("--", "").Replace(";", "").Replace("#","").Replace("*","");
        }
    }

}
