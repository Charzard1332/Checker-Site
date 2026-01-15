using Microsoft.AspNetCore.Http; // including http services lib

namespace Checker_Site.Middleware;

// middleware to add security headers and enforce security policies
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SecurityHeadersMiddleware> _loggerFactory;

    public SecurityHeadersMiddlware(RequestDelegate next, ILogger<SecurityHeadersMiddleware> iLogger)
    {
        _next = next;
        _loggerFactory = iLogger;
    }
}