using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Mime;
using System.Threading.Tasks;
using WebAPI.JwtAthentication.Models;
using WebAPI.JwtAthentication.Services;

namespace WebAPI.JwtAthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private readonly IUnitOfWork<User> _users;

        public LoginController(IConfiguration configuration, IUnitOfWork<User> users)
        {
            _config = configuration;
            _users = users;
        }


        [HttpPost]
        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Login(UserModel login)
        {
            User user = await AuthenticateUser(login);

            if (user != null)
            {
                string tokenString = 
                    JWTServices.GenerateJsonWebToken(user, _config);

                return Ok(new { token = tokenString, ID = user.ID });
            }

            return Unauthorized();
        }

        private async Task<User> AuthenticateUser(UserModel login)
        {
            return await _users.Entity
                               .GetOneWithOptions<User>(u =>
                               new User()
                               {
                                   ID = u.ID,
                                   Name = u.Name,
                                   Age = u.Age,
                                   Address = u.Address,
                                   Email = u.Email
                               },
                               // conditions
                               u => u.Email == login.Email && u.Password == login.Password
                               );
        }

        //private string GenerateJsonWebToken(User userInfo)
        //{
        //    var securityKey = new 
        //        SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

        //    var credentials =
        //        new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    var claims = new Claim[]
        //    {
        //        new Claim("ID", userInfo.ID.ToString()),
        //        new Claim("Name", userInfo.Name),
        //        new Claim("Age", userInfo.Age.ToString()),
        //        new Claim("Address", userInfo.Address),
        //        new Claim("Email", userInfo.Email)
        //    };

        //    var token = new JwtSecurityToken(_config["Jwt:Issuer"],
        //                                     _config["Jwt:Issuer"],
        //                                     claims,
        //                                     expires: DateTime.Now.AddMinutes(120),
        //                                     signingCredentials: credentials);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }
}
