using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Zählerstände.Models;

namespace Zählerstände.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase {
    private readonly IConfiguration _config;
    private readonly UserManager<IdentityUser> _userMgr;

    public AuthController(
        IConfiguration config,
        UserManager<IdentityUser> userMgr) {
        _config = config;
        _userMgr = userMgr;
    }
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginModel model) {
        //check user credentials
        var user = await _userMgr.FindByNameAsync(model.Username);
        if (user is null || !await _userMgr.CheckPasswordAsync(user, model.Password))
        { return Unauthorized(); }
        //create list of claims
        var roles = await _userMgr.GetRolesAsync(user);
        var claims = new List<Claim>() {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!)
        };
        claims.AddRange(roles.Select(role=>new Claim(ClaimTypes.Role,role)));
        // generate key
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //generate Token
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);
        // return ok with Token
        return Ok(new {
            token =new JwtSecurityTokenHandler().WriteToken(token)
        });
    }
}