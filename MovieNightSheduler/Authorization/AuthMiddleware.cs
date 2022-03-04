using Dapper.Contrib.Extensions;
using MovieNightScheduler.Models;
using MovieNightScheduler.Helpers;
using Microsoft.Extensions.Options;
using MovieNightScheduler.Services;
namespace MovieNightScheduler.Authorization
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public AuthMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, AppDb db, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtUtils.ValidateJwtToken(token);
            if (userId != null)
            {
                // attach account to context on successful jwt validation
                context.Items["User"] = await db.Connection.GetAsync<User>(userId.Value);
            }

            await _next(context);
        }
    }
}
