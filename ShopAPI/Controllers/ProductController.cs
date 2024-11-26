using AuthApi.Models.Requests;
using AuthApi.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAPI.Models;
using System.Security.Claims;

namespace ShopAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly AuthDbContext dbContext;

        private readonly IConfiguration configuration;

        public ProductController(AuthDbContext ctx, IConfiguration cfg)
        {
            dbContext = ctx;
            configuration = cfg;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var usernameClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (usernameClaim == null)
                return Unauthorized(new APIResponse(false, "Not authorized"));

            var username = usernameClaim.Value;
            var dbUser = await dbContext.users.FirstOrDefaultAsync(u => u.username == username);
            if (dbUser == null || !dbUser.isEnabled)
                return BadRequest(new APIResponse(false, "Invalid or disabled user account."));


            var result = await dbContext.products.ToListAsync();
            return Ok(new APIResponse<List<Product>>(true, "Product list successfully fetched.", result));
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetInfo([FromQuery]int id)
        {
            var usernameClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (usernameClaim == null)
                return Unauthorized(new APIResponse(false, "Not authorized"));

            var username = usernameClaim.Value;
            var dbUser = await dbContext.users.FirstOrDefaultAsync(u => u.username == username);
            if (dbUser == null || !dbUser.isEnabled)
                return BadRequest(new APIResponse(false, "Invalid or disabled user account."));


            var result = await dbContext.products.FirstOrDefaultAsync(p => p.id == id);
            if (result == null)
            {
                return BadRequest(new APIResponse(false, "Product does not exist."));
            }

            return Ok(new APIResponse<Product>(true, "Product info successfully fetched.", result));
        }
    }
}
