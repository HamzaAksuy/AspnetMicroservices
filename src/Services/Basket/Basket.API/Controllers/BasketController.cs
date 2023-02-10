using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepo;
        private readonly DiscountGrpcService _discountGrpc;
        public BasketController(IBasketRepository basketRepo,DiscountGrpcService discountGrpc)
        {
            _basketRepo = basketRepo ?? throw new ArgumentNullException(nameof(basketRepo));
            _discountGrpc= discountGrpc ?? throw new ArgumentNullException(nameof(discountGrpc));
        }
        [HttpGet("{userName}",Name ="GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket =await _basketRepo.GetBasket(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }
        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody]ShoppingCart basket)
        {
            foreach (var item in basket.Items)
            {
                var coupon = await _discountGrpc.GetDiscount(item.ProductName.ToString());
                item.Price -= coupon.Amount;
            }

            return Ok(await _basketRepo.UpdateBasket(basket));
        }
        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _basketRepo.DeleteBasket(userName);
            return Ok();
        }
    }
}
