using IdentityManagement.Infrastructure.Persistance;
using IdentityManagement.Server.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityManagement.Server.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("api/user/register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestViewModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new AppUser
            {
                UserName = request.Username,
                Name = request.Name,
                Surname = request.Surname,
                DateOfBirth = request.BirthDate,
                Email = request.Email,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddClaimAsync(user, new Claim("username", user.UserName));
            await _userManager.AddClaimAsync(user, new Claim("name", user.Name));
            await _userManager.AddClaimAsync(user, new Claim("surname", user.Surname));
            await _userManager.AddClaimAsync(user, new Claim("email", user.Email));
            await _userManager.AddClaimAsync(user, new Claim("birthdate", user.DateOfBirth.ToShortDateString()));
            await _userManager.AddClaimAsync(user, new Claim("role", "customer"));

            return Ok();
        }
    }
}
