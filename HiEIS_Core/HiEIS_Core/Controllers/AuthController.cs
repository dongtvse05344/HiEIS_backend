using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HiEIS.Model;
using HiEIS_Core.Models;
using HiEIS_Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HiEIS_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<MyUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<MyUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("token")]
        public async Task<ActionResult> GetToken([FromBody]LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return BadRequest("Invalid Username");
            }
            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
            {
                return BadRequest("Invalid Password");
            }
            return new OkObjectResult(GenerateToken(user).Result); 
        }

        private async Task<Token> GenerateToken(MyUser user)
        {
            //security key
            string securityKey = "qazedcVFRtgbNHYujmKIolp";
            //symmectric security key
            var symmectricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            //signing credentials
            var signingCredentials = new SigningCredentials(symmectricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            //add Claims
            var claims = new List<Claim>();
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            //create token
            var token = new JwtSecurityToken(
                    issuer: "dongtv",
                    audience: user.Name,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: signingCredentials,
                    claims: claims
                );
            //return token
            return new Token
            {
                roles = _userManager.GetRolesAsync(user).Result.ToArray(),
                access_token = new JwtSecurityTokenHandler().WriteToken(token),
                expires_in = (int)TimeSpan.FromHours(1).TotalSeconds
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]

        public ActionResult AdminArea()
        {
            return Ok("Welcome admin");
        }

        [HttpGet("AccountingManager")]
        [Authorize]
        public ActionResult AccountingManagerArea()
        {
            return Ok("Welcome AccountingManager");
        }
    }
}