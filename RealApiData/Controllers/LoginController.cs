using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RealApiData.Models;
using RealApiData.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealApiData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public class LoginResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public TokenData Data { get; set; }

            public bool IsAdmin { get; set; } = false;
        }

        public class TokenData
        {
            public string Token { get; set; }
        }
        public GenericRepository<User> userrepository;
        public LoginController(GenericRepository<User> _userrepository)
        {
            userrepository = _userrepository;
        }

        [HttpPost]
        public IActionResult Login([FromBody] User user)
        {
            var loggedInUser = userrepository.getall().FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);

            if (loggedInUser != null)
            {
                var claims = new[]
                {
           new Claim(ClaimTypes.Name, user.Email),
           new Claim(ClaimTypes.Role, loggedInUser.IsAdmin ? "Admin" : "User")
        };

                // Generate JWT token
                string key = "Welcome To Our Funi Website, By Ola";
                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: credentials
                );

                //return data when he logged in
                return Ok(new LoginResponse
                {
                    Success = true,
                    Message = $"Logged in as {(loggedInUser.IsAdmin ? "Admin" : "User")}",
                    IsAdmin = loggedInUser.IsAdmin,
                    Data = new TokenData
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token)
                    },
                });
            }
            else
            {
                return BadRequest(new LoginResponse { Success = false, Message = "Login failed" });
            }
        }
    }
}
