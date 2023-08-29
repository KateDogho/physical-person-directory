using System.Globalization;

namespace PhysicalPersonDirectory.Api.Middlewares;
public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public LocalizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var cultureQuery = context.Request.Headers["Accept-Language"];
        var culture = cultureQuery.ToString();

        var supportedCultures = new[] { "en-US", "ka-GE" };

        if (supportedCultures.Contains(culture))
        {
            CultureInfo.CurrentCulture = new CultureInfo(culture);
            CultureInfo.CurrentUICulture = new CultureInfo(culture);
        }

        await _next(context);
    }
}