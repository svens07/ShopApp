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
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly AuthDbContext dbContext;

        private readonly IConfiguration configuration;

        public CartController(AuthDbContext ctx, IConfiguration cfg)
        {
            dbContext = ctx;
            configuration = cfg;
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetItems()
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
                cart = new Cart()
                {
                    userId = dbUser.id,
                    totalAmount = 0,
                    isActive = true,
                    createdAt = DateTime.Now
                };
                await dbContext.carts.AddAsync(cart);
                await dbContext.SaveChangesAsync();
            }

            var cartItems = await dbContext.cart_items.Where(u => u.cartId == cart.id)
                .Select(item => new CartItemMin
                {
                    productId = item.productId,
                    quantity = item.quantity
                })
                .ToListAsync();

            return Ok(new APIResponse<List<CartItemMin>>(true, "Items fetched from cart successfully.", cartItems));
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveItems([FromQuery] int productId)
        {
            var usernameClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (usernameClaim == null)
                return Unauthorized(new APIResponse(false, "Not authorized"));

            var username = usernameClaim.Value;
            var dbUser = await dbContext.users.FirstOrDefaultAsync(u => u.username == username);
            if (dbUser == null || !dbUser.isEnabled)
                return BadRequest(new APIResponse(false, "Invalid or disabled user account."));

            var cart = await dbContext.carts.FirstOrDefaultAsync(c => c.userId == dbUser.id && c.isActive == true);
            if (cart == null)
            {
                cart = new Cart()
                {
                    userId = dbUser.id,
                    totalAmount = 0,
                    isActive = true,
                    createdAt = DateTime.Now
                };
                await dbContext.carts.AddAsync(cart);
                await dbContext.SaveChangesAsync();
            }

            var itemsToRemove = await dbContext.cart_items
                .Where(ci => ci.cartId == cart.id && ci.productId == productId)
                .ToListAsync();

            dbContext.cart_items.RemoveRange(itemsToRemove);
            await dbContext.SaveChangesAsync();

            return await GetItems();
        }

        [HttpPut("modify")]
        public async Task<IActionResult> ModifyItems([FromBody] CartItemMin updatedItem)
        {
            var usernameClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (usernameClaim == null)
                return Unauthorized(new APIResponse(false, "Not authorized"));

            var username = usernameClaim.Value;
            var dbUser = await dbContext.users.FirstOrDefaultAsync(u => u.username == username);
            if (dbUser == null || !dbUser.isEnabled)
                return BadRequest(new APIResponse(false, "Invalid or disabled user account."));

            var cart = await dbContext.carts.FirstOrDefaultAsync(c => c.userId == dbUser.id && c.isActive == true);
            if (cart == null)
            {
                cart = new Cart()
                {
                    userId = dbUser.id,
                    totalAmount = 0,
                    isActive = true,
                    createdAt = DateTime.Now
                };
                await dbContext.carts.AddAsync(cart);
                await dbContext.SaveChangesAsync();
            }


            var existingItem = await dbContext.cart_items
                .FirstOrDefaultAsync(ci => ci.cartId == cart.id && ci.productId == updatedItem.productId);

            if (existingItem != null)
            {
                if (updatedItem.quantity == 0)
                {
                    dbContext.cart_items.Remove(existingItem);
                }
                else
                {
                    existingItem.quantity = updatedItem.quantity;
                    existingItem.updatedAt = DateTime.Now;
                    dbContext.cart_items.Update(existingItem);
                    await dbContext.SaveChangesAsync();
                }
            }
            else
            {
                var newCartItem = new CartItem
                {
                    cartId = cart.id,
                    productId = updatedItem.productId,
                    quantity = updatedItem.quantity,
                    createdAt = DateTime.Now,
                    updatedAt = DateTime.Now
                };
                await dbContext.cart_items.AddAsync(newCartItem);
                await dbContext.SaveChangesAsync();
            }


            var cartItems = await dbContext.cart_items.Where(u => u.cartId == cart.id).ToListAsync();

            double totalAmount = 0;
            foreach(var cartItem in cartItems)
            {
                var productInfo = await dbContext.products.FirstOrDefaultAsync(p => p.id == cartItem.productId);
                if (productInfo != null)
                {
                    totalAmount += productInfo.price * cartItem.quantity;
                }
            }

            if (cart.totalAmount != totalAmount)
            {
                cart.totalAmount = totalAmount;
                await dbContext.SaveChangesAsync();
            }

        
            return await GetItems();
        }



        [HttpPost("add")]
        public async Task<IActionResult> AddItem([FromBody] CartItemMin addItem)
        {
            var usernameClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (usernameClaim == null)
                return Unauthorized(new APIResponse(false, "Not authorized"));

            var username = usernameClaim.Value;
            var dbUser = await dbContext.users.FirstOrDefaultAsync(u => u.username == username);
            if (dbUser == null || !dbUser.isEnabled)
                return BadRequest(new APIResponse(false, "Invalid or disabled user account."));

            var cart = await dbContext.carts.FirstOrDefaultAsync(c => c.userId == dbUser.id && c.isActive == true);
            if (cart == null)
            {
                cart = new Cart()
                {
                    userId = dbUser.id,
                    totalAmount = 0,
                    isActive = true,
                    createdAt = DateTime.Now
                };
                await dbContext.carts.AddAsync(cart);
                await dbContext.SaveChangesAsync();
            }


            var existingItem = await dbContext.cart_items
                .FirstOrDefaultAsync(ci => ci.cartId == cart.id && ci.productId == addItem.productId);

            if (existingItem != null)
            {
                existingItem.quantity = existingItem.quantity + addItem.quantity;
                existingItem.updatedAt = DateTime.Now;
                dbContext.cart_items.Update(existingItem);
            }
            else
            {
                var newCartItem = new CartItem
                {
                    cartId = cart.id,
                    productId = addItem.productId,
                    quantity = addItem.quantity,
                    createdAt = DateTime.Now,
                    updatedAt = DateTime.Now
                };
                await dbContext.cart_items.AddAsync(newCartItem);
            }


            var cartItems = await dbContext.cart_items.Where(u => u.cartId == cart.id).ToListAsync();

            double totalAmount = 0;
            foreach (var cartItem in cartItems)
            {
                var productInfo = await dbContext.products.FirstOrDefaultAsync(p => p.id == cartItem.productId);
                if (productInfo != null)
                {
                    totalAmount += productInfo.price * cartItem.quantity;
                }
            }
            cart.totalAmount = totalAmount;

            await dbContext.SaveChangesAsync();
            return await GetItems();
        }

    }
}
