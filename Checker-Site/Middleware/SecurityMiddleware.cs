using Microsoft.AspNetCore.Http; // including http services lib

namespace Checker_Site.Middleware;

// middleware to add security headers and enforce security policies
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SecurityHeadersMiddleware> _loggerFactory;

    public SecurityHeadersMiddleware(RequestDelegate next, ILogger<SecurityHeadersMiddleware> iLogger)
    {
        _next = next;
        _loggerFactory = iLogger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // add security headers here
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Append("X-Frame-Options", "DENY");
        context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
        context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload");
        // add more here idc im not adding everything for you skids

        if (IsSuspiciousRequest(context.Request))
        {
            _loggerFactory.LogWarning("Suspicious request detected");
            context.Response.StatusCode = 403;
        }
        await _next(context);
    }

    private bool IsSuspiciousRequest(HttpRequest request)
    {
        // check for common attack patterns
        var path = request.Path.Value?.ToLower() ?? "";
        var query = request.QueryString.Value?.ToLower() ?? "";
        var fullPath = path + query;

        var suspiciousPatterns = new[]
        {
            "admin", "wp-admin", "xmlrpc", "shell", "cmd", "exec"
        };
        // you fucks can write the rest of the patterns im all good gng
        return suspiciousPatterns.Any(pattern => fullPath.Contains(pattern));
    }
}

// rate limiting middleware to prevent brute force attacks
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitingMiddleware> _loggerFactory;
    private static readonly Dictionary<string, List<DateTime>> RequestLog = new();
    private static readonly object lockObject = new();

    public RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> iLogger)
    {
        _next = next;
        _loggerFactory = iLogger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress.ToString() ?? "unknown";
        var now = DateTime.UtcNow;
        var windowStart = now.AddSeconds(-60); // 1 min window
        
        // clean and check rate limit
        bool rateLimited = false;
        lock (lockObject)
        {
            if (!RequestLog.ContainsKey(ip))
            {
                RequestLog[ip] = new List<DateTime>();
            }
            // clean old requests
            RequestLog[ip].RemoveAll(t => t < windowStart);
            
            // check rate limit (100 request per min per ip
            if (RequestLog[ip].Count >= 100 && context.Request.Path.StartsWithSegments("/api"))
            {
                rateLimited = true;
                _loggerFactory.LogWarning("Rate limit exceeded for IP {ip}", ip);
            }
            else
            {
                RequestLog[ip].Add(now);
            }
        }

        if (rateLimited)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.Response.WriteAsJsonAsync(new { error = "Rate limit exceeded" });
            return;
        }
        
        // log all api requests
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            _loggerFactory.LogInformation("API request from IP {ip}", ip);
        }

        await _next(context);
    }
}