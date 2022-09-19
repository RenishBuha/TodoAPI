using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;
using Bethubs.Auth;
using TodoAPI.Auth;
using Microsoft.AspNetCore.Authorization;
using TodoAPI.Web;

namespace TodoAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IConfiguration _configuration;

        public TokenController(TodoContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [AllowAnonymous]
        public async Task<IActionResult> Post (UserInfo _userData)
        {
            if(_userData != null && _userData.Email != null && _userData.Password != null)
            {
                var user = await GetUser(_userData.Email, _userData.Password);

                if (user != null)
                {
                    var tokenData = new TokenData
                    {
                        UserId = user.UserId.ToString(),
                        Username = user.UserName,
                        Role = user.Role,
                        Email = user.Email,
                        Expiration = DateTime.Now.AddMinutes(2),
                        IpAddress = "192.168.1.8",
                    };

                    var token = AuthUtil.EncodeToken(tokenData, _configuration["Jwt:Key"]);

                    CookieOptions cookieexpire = new CookieOptions();
                    cookieexpire.Expires = DateTime.Now.AddMinutes(2);
                    Response.Cookies.Append("JwtToken", token, cookieexpire);
                    
                    return Ok(token);

                    //create claims details based on the user information
                    //var claims = new[] {
                    //    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    //    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    //    new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                    //    new Claim(ClaimTypes.Name, user.UserName),
                    //    new Claim(ClaimTypes.Email, user.Email),
                    //    new Claim(ClaimTypes.Expiration, )
                    //};

                    //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    //var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    //var token = new JwtSecurityToken(
                    //    _configuration["Jwt:Issuer"],
                    //    _configuration["Jwt:Audience"],
                    //    claims,
                    //    expires: DateTime.UtcNow.AddHours(2),
                    //    signingCredentials: signIn);

                    //var handler = new JwtSecurityTokenHandler();

                    //var jwtSecurityToken = handler.CreateJwtSecurityToken(
                    //   _configuration["Jwt:Issuer"],
                    //    _configuration["Jwt:Audience"],
                    //    new ClaimsIdentity(claims),
                    //    expires: DateTime.UtcNow.AddHours(2),
                    //    signingCredentials: signIn);

                    //string tokenString = handler.WriteToken(jwtSecurityToken);

                    //CookieOptions cookieexpire = new CookieOptions();
                    //cookieexpire.Expires = DateTime.Now.AddMinutes(20);
                    //Response.Cookies.Append("JwtToken", "Bearer " + tokenString, cookieexpire);

                    //return Ok(new JwtSecurityTokenHandler().WriteToken(token););
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }   
            }
            else
            {
                return BadRequest("Invalid Data");
            }

        }

        private async Task<UserInfo> GetUser(string email, string password)
        {
            return await _context.UserInfos.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
    }
}
