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
    public class RegistrationController : ControllerBase
    {
        private readonly IUnitOfWork<User> _users;
        private IConfiguration _config;


        public RegistrationController(IConfiguration configuration, IUnitOfWork<User> users)
        {
            _users = users;
            _config = configuration;
        }


        [HttpPost("IsUniqueEmail")]
        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task IsUniqueEmail(EmailModel email)
        {
            string emailTobeCompared = email.Email.Trim().ToLower();

            string localEmail = await _users.Entity
                .GetOneWithOptions<string>(u => u.Email, u => u.Email == emailTobeCompared);

            bool isUnique = localEmail == null ? true : false;

            await HttpResponseJsonExtensions.WriteAsJsonAsync(Response, new { isUnique = isUnique });
        }

        [HttpGet("IsUniqueEmailServerValidation")]
        public async Task IsUniqueEmailServerValidation(string email)
        {
            string emailTobeCompared = email.Trim().ToLower();

            string localEmail = await _users.Entity
                .GetOneWithOptions<string>(u => u.Email, u => u.Email == emailTobeCompared);

            if (localEmail == null)
                await HttpResponseJsonExtensions.WriteAsJsonAsync(Response, true);

            await HttpResponseJsonExtensions.WriteAsJsonAsync(Response, false);
        }

        [HttpPost]
        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateUser(User user)
        {
            user.Email = user.Email.Trim().ToLower();

            await _users.Entity.Create(user);

            await _users.Save();

            string tokenString =
                JWTServices.GenerateJsonWebToken(user, _config);

            return Ok(new { token = tokenString, ID = user.ID });
        }
    }
}
