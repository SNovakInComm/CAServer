using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CAApi.Models;
using CAApi.Utilities;

namespace CAApi.Controllers
{
    [Route("/[controller]")]
    [ApiVersion("1.0")]
    public class UserController : Controller
    {
        private readonly CADBContext _context;

        private Crypto cipher;

        public UserController(CADBContext context)
        {
            _context = context;
            cipher = new Crypto(_context);
        }

        [HttpGet(Name = nameof(GetUser))]
        public IActionResult GetUser()
        {
            if (!Crypto.Validated)
                return Unauthorized();

            return Ok();
        }

        [HttpGet("{userID}", Name = nameof(GetUserByIdAsync))]
        public async Task<IActionResult> GetUserByIdAsync(Guid userID, CancellationToken ct)
        {
            if (!Crypto.Validated)
                return Unauthorized();

            var entity = await _context.Users.SingleOrDefaultAsync(r => r.Id == userID.ToString(), ct);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        [HttpGet("{firstName},{lastName}", Name = nameof(GetUserByNameAsync))]
        public async Task<IActionResult> GetUserByNameAsync(String firstName, String lastName, CancellationToken ct)
        {
            if (!Crypto.Validated)
                return Unauthorized();

            var entity = await _context.Users.SingleOrDefaultAsync(r => r.FirstName == firstName && r.LastName == lastName, ct);

            if (entity == null)
                return NotFound();
           
            return Ok(entity);
        }


        [HttpPut("{firstName},{lastName}", Name = nameof(CreateUserWithNameAsync))]
        public async Task<IActionResult> CreateUserWithNameAsync(String firstName, String lastName, CancellationToken ct)
        {
            if (!Crypto.Validated)
                return Unauthorized();

            var entity = await _context.Users.SingleOrDefaultAsync(r => r.FirstName == firstName && r.LastName == lastName, ct);

            if (entity != null)
                return BadRequest("The User Already Exists");

            string id = Guid.NewGuid().ToString();
            await _context.Users.AddAsync(new UserEntity()
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName
            });

            var resource = new
            {
                ID = id
            };

            var result = await _context.SaveChangesAsync();

            if (result == 1)
                return Ok(resource);
            else
                return NotFound();

        }

        [HttpDelete("{userID}", Name = nameof(DeleteUserWithIDAsync))]
        public async Task<IActionResult> DeleteUserWithIDAsync(Guid userID, CancellationToken ct )
        {
            if (!Crypto.Validated)
                return Unauthorized();

            var entity = await _context.Users.SingleOrDefaultAsync(r => r.Id == userID.ToString(), ct);
            if (entity == null)
                return NotFound();

            _context.Users.Remove(entity);
            _context.SaveChanges();

            return Ok();
        }
    }
}