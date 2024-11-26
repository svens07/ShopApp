using AuthApi.Models.Requests;
using AuthApi.Models.Responses;
using DevOne.Security.Cryptography.BCrypt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthDbContext dbContext;

        private readonly IConfiguration configuration;

        public AuthController(AuthDbContext ctx, IConfiguration cfg)
        {
            dbContext = ctx;
            configuration = cfg;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var dbUser = await dbContext.users.FirstOrDefaultAsync(u => u.username == req.username || u.email == req.username);
            if (dbUser == null || !dbUser.isEnabled)
                return BadRequest(new APIResponse(false, "Invalid or disabled user account."));

            var hashedPass = BCryptHelper.HashPassword(req.password, configuration.GetSection("BCrypt")["Salt"]);
            if (dbUser.passwordHash != hashedPass) 
            {
                return BadRequest(new APIResponse(false, "Username or password is wrong."));
            }

            var jwtToken = GenerateJWTToken(dbUser.username);

            dbUser.lastLogin = DateTime.Now;
            await dbContext.SaveChangesAsync();

            return Ok(new APIResponse<LoginResponse>(true, "Successfully logged in.", new LoginResponse() { jwtToken = jwtToken }));
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            if (await dbContext.users.AnyAsync(u => u.username == req.username || u.email == req.email))
                return BadRequest(new APIResponse(false, "Username or email was already used."));

            User newUser = new()
            {
                username = req.username,
                email = req.email,
                passwordHash = BCryptHelper.HashPassword(req.password, configuration.GetSection("BCrypt")["Salt"]),
                role = "User",
                createdAt = DateTime.Now,
                lastLogin = DateTime.Now,
                isEnabled = true
            };

            dbContext.users.Add(newUser);
            await dbContext.SaveChangesAsync();

            var jwtToken = GenerateJWTToken(newUser.username);
            return Ok(new APIResponse<LoginResponse>(true, "Account was successfully registered.", new LoginResponse() { jwtToken = jwtToken }));
        }

        [HttpPost("change-pass")]
        [Authorize]
        public async Task<IActionResult> ChangePass([FromBody] ChangePassRequest req)
        {
            var usernameClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (usernameClaim == null)
                return Unauthorized(new APIResponse(false, "Not authorized"));

            var username = usernameClaim.Value;
            var dbUser = await dbContext.users.FirstOrDefaultAsync(u => u.username == username);
            if (dbUser == null || !dbUser.isEnabled)
                return BadRequest(new APIResponse(false, "Invalid or disabled user account."));

            var oldHash = BCryptHelper.HashPassword(req.oldPassword, configuration.GetSection("BCrypt")["Salt"]);
            if (oldHash != dbUser.passwordHash)
            {
                return BadRequest(new APIResponse(false, "Password does not match."));
            }

            var newHash = BCryptHelper.HashPassword(req.newPassword, configuration.GetSection("BCrypt")["Salt"]);

            dbUser.passwordHash = newHash;
            await dbContext.SaveChangesAsync();

            return Ok(new APIResponse(true, "Password was successfully changed."));
        }


        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var usernameClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (usernameClaim == null)
                return Unauthorized(new APIResponse(false, "Not authorized"));

            var username = usernameClaim.Value;
            var dbUser = await dbContext.users.FirstOrDefaultAsync(u => u.username == username);
            if (dbUser == null || !dbUser.isEnabled)
                return BadRequest(new APIResponse(false, "Invalid or disabled user account."));

            MeResponse result = new()
            {
                id = dbUser.id,
                username = dbUser.username,
                email = dbUser.email,
                role = dbUser.role
            };

            return Ok(new APIResponse<MeResponse>(true, "User information successfully fetched.", result));
        }


        private string GenerateJWTToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSettings = configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                     new Claim(ClaimTypes.Name, username) 
                ]),
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(Convert.ToInt16(jwtSettings["TokenTime"])), 
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
