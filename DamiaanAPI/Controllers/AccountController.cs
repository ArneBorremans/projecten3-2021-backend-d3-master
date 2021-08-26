using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DamiaanAPI.Data.DTOs;
using DamiaanAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DamiaanAPI.Controllers
{

    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILoperRepository _loperRepository;
        private readonly IConfiguration _config;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
          SignInManager<IdentityUser> signInManager,
          UserManager<IdentityUser> userManager,
          ILoperRepository loperRepository,
          IConfiguration config, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _loperRepository = loperRepository;
            _roleManager = roleManager;
            _config = config;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model">the login details</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<String>> CreateToken(LoginDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            var loper = _loperRepository.GetByEmail(model.Email);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var loperId = loper != null ? loper.ID : 0;
                    var loginResponse = new LoginResponse { Token = GetToken(user, roles), Role = roles, LoperId = loperId };
                    return Created("", loginResponse);                    
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// Register a user
        /// </summary>
        /// <param name="model">the user details</param>
        /// <returns></returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<String>> Register(RegisterDTO model)
        {

            IdentityUser user = new IdentityUser { UserName = model.Email, Email = model.Email };
            Loper loper = new Loper { FirstName = model.FirstName, LastName = model.LastName, Gemeente = model.Gemeente, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, "Loper");

            if (result.Succeeded)
            {
                _loperRepository.Add(loper);
                _loperRepository.SaveChanges();
                var roles = await _userManager.GetRolesAsync(user);
                var loginResponse = new LoginResponse { Token = GetToken(user, roles), Role = roles, LoperId = loper.ID};
                Mail.RegisterMail(loper);
                return Created("", loginResponse);
            }
            return BadRequest();
        }

        /// <summary>
        /// Checks if an email is available as username
        /// </summary>
        /// <returns>true if the email is not registered yet</returns>
        /// <param name="email">email</param>/
        [HttpGet("checkusername")]
        [AllowAnonymous]
        public async Task<ActionResult<Boolean>> CheckAvailableUserName(string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            return user == null;
        }

        private String GetToken(IdentityUser user, IList<String> roles)
        {
            // Create the token
            List<Claim> claims = new List<Claim>()
            {
              new Claim(JwtRegisteredClaimNames.Sub, user.Email),
              new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, (string) role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              null, null,
              claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    class LoginResponse
    {
        public String Token { get; set; }
        public IList<String> Role { get; set; }
        public int LoperId { get; set; }
    }
}
