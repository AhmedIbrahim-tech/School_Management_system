using Microsoft.AspNetCore.Http;

namespace Core.Middleware;

public class CustomJwtMiddleware
{
    private readonly RequestDelegate _next;

    public CustomJwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (!string.IsNullOrEmpty(token))
        {
            context.Items["Token"] = token;
        }

        await _next(context);
    }
}
