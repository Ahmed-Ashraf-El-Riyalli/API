using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork<User> _users;

        public UserController(IUnitOfWork<User> users)
        {
            _users = users;
        }


        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetCurrentUser()
        {
            var currentUserClaims = HttpContext.User.Claims;

            User user = new User()
            {
                ID = int.Parse(currentUserClaims.FirstOrDefault(c => c.Type == "ID").Value),
                Name = currentUserClaims.FirstOrDefault(c => c.Type == "Name").Value,
                Age = int.Parse(currentUserClaims.FirstOrDefault(c => c.Type == "Age").Value),
                Address = currentUserClaims.FirstOrDefault(c => c.Type == "Address").Value,
                Email = currentUserClaims.FirstOrDefault(c => c.Type == "Email").Value
            };

            return Ok(user);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetUserById(int id)
        {
            User user = await _users.Entity.GetByID(id);

            if (user != null)
                return Ok(user);

            return NotFound();
        }

        [Authorize]
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(int id, User user)
        {
            if (id != user.ID)
                return BadRequest();

            string password = await _users.Entity
                .GetOneWithOptions(u => u.Password, u => u.ID == id);

            user.Password = password;

            _users.Entity.UpdateByID(id, user);

            try
            {
                await _users.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
