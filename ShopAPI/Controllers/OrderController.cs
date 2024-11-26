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
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly AuthDbContext dbContext;

        private readonly IConfiguration configuration;

        public OrderController(AuthDbContext ctx, IConfiguration cfg)
        {
            dbContext = ctx;
            configuration = cfg;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder()
        {
            var usernameClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (usernameClaim == null) return Unauthorized(new APIResponse(false, "Not authorized"));
            var username = usernameClaim.Value;

            var dbUser = await dbContext.users.FirstOrDefaultAsync(u => u.username == username);
            if (dbUser == null || !dbUser.isEnabled)
            {
                return BadRequest(new APIResponse(false, "Invalid or disabled user account."));
            }

            var cart = await dbContext.carts.FirstOrDefaultAsync(c => c.userId == dbUser.id && c.isActive == true);
            if (cart == null)
            {
                return BadRequest(new APIResponse(false, "An active cart was not found."));
            }

            cart.isActive = false;
            await dbContext.SaveChangesAsync();

            Order newOrder = new Order()
            {
                cartId = cart.id,
                userId = dbUser.id,
                createdAt = DateTime.Now,
                totalAmount = cart.totalAmount
            };

            await dbContext.orders.AddAsync(newOrder);
            await dbContext.SaveChangesAsync();

            return Ok(new APIResponse<object>(true, "Order successfully created.", null));
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetOrders()
        {
            var usernameClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (usernameClaim == null) return Unauthorized(new APIResponse(false, "Not authorized"));
            var username = usernameClaim.Value;

            var dbUser = await dbContext.users.FirstOrDefaultAsync(u => u.username == username);
            if (dbUser == null || !dbUser.isEnabled)
            {
                return BadRequest(new APIResponse(false, "Invalid or disabled user account."));
            }

            var orders = await dbContext.orders.Where(o => o.userId == dbUser.id).ToListAsync();

            List<OrderInfo> orderInfos = [];
            foreach (var order in orders)
            {
                OrderInfo orderInfoResponse = new OrderInfo();
                var items = await dbContext.cart_items.Where(c => c.cartId == order.cartId).Select(c => new CartItemMin() { productId = c.productId, quantity = c.quantity}).ToListAsync();
                orderInfoResponse.cartItems = items;
                orderInfoResponse.id = order.id;
                orderInfoResponse.createdAt = order.createdAt;
                orderInfoResponse.totalAmount = order.totalAmount;
                orderInfos.Add(orderInfoResponse);
            }

            return Ok(new APIResponse<List<OrderInfo>>(true, "Order list successfully fetched.", orderInfos));
        }


    }
}
