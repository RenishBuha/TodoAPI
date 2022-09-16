using Bethubs.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace TodoAPI.AuthMiddleware
{
    public class CustomAuthHandler : AuthenticationHandler<CustomAuthOptions>
    {
        private readonly IConfiguration _configuration;

        public CustomAuthHandler(IOptionsMonitor<CustomAuthOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock, IConfiguration configuration) : base(options, logger, encoder, clock)
        {
             _configuration = configuration;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Options.AuthTypes.Contains("JWT") && ValidateToken(Options.IsHostOrigin, out var jwtIdentity))
            {
                var ticket = new AuthenticationTicket(new ClaimsPrincipal(jwtIdentity), Options.Scheme);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail($"All auth schemes failed. Path:{Request.Path}"));
        }

        public bool ValidateToken(bool isHostOrigin, out ClaimsIdentity identity)
        {
            identity = null;

            string token = null;
            string cookieToken = null;

            if (Request.Cookies.ContainsKey("JwtToken"))
            {
                cookieToken = Request.Cookies["JwtToken"];
            }
            
            if (Request.Headers.ContainsKey("authorization"))
            {
                token = ((string)Request.Headers["authorization"]).Replace("Bearer ", "");
            }

            if (token == null || cookieToken == null)
            {
                Logger.LogTrace("JWT AUTH FAILED. Invalid or expire token");
                return false;
            }

            var response = AuthUtil.DecodeJwtToken(token, _configuration["Jwt:Key"]);
            if (!response.IsSuccess)
            {
                Logger.LogTrace("JWT AUTH FAILED. {token} not authorized", token);
                return false;
            }
            var tokenData = response.Data;

            identity = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, tokenData.UserId),
                    new Claim(ClaimTypes.Name, tokenData.Username),
                    new Claim(ClaimTypes.Role, tokenData.Role),
                    new Claim(ClaimTypes.Email, tokenData.Email)
                }, "JWT");


            return true;
        }
    }
}
