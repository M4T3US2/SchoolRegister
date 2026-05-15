using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolRegister.Api.Controllers;

public class AccountApiController : BaseApiController
{
    private readonly UserManager<User> _userManager;
    private readonly JwtOptionsVm _jwtOptions;
    private readonly ILogger<AccountApiController> _logger;

    public AccountApiController(
        UserManager<User> userManager,
        IOptions<JwtOptionsVm> jwtOptions,
        ILogger<AccountApiController> logger)
    {
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
        _logger = logger;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromForm] LoginUserVm loginVm)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(loginVm.Login!);
            if (user == null)
                return Unauthorized("Nieprawidłowy login lub hasło.");

            var passwordValid = await _userManager.CheckPasswordAsync(user, loginVm.Password!);
            if (!passwordValid)
                return Unauthorized("Nieprawidłowy login lub hasło.");

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.TokenExpirationMinutes),
                signingCredentials: creds
            );

            return Ok(new
            {
                access_token = new JwtSecurityTokenHandler().WriteToken(token),
                expires_in = DateTime.UtcNow.AddMinutes(_jwtOptions.TokenExpirationMinutes)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, "Wewnętrzny błąd serwera.");
        }
    }
}