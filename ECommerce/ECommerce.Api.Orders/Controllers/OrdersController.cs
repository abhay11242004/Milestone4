using ECommerce.Api.Orders.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Controllers
{
/*Course: 		Web Programming 3
* Assessment: 	Milestone 4
* Created by: 	Abhay Patel - 2261385
* Date: 		30 November 2024
* Class Name: 	OrdersController.cs
* Description: 	Manages request for getting orders for a specific customer with the customerId 
* Time for Task:	4 hours
*/
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersProvider _orderProvider;
        public OrdersController(IOrdersProvider orderProvider)
        {
            _orderProvider = orderProvider;
        }
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetOrdersAsync(int customerId)
        {
            var result = await _orderProvider.GetOrdersAsync(customerId);
            if (result.IsSuccess)
            {
                return Ok(result.Orders);
            }
            return NotFound();
        }
    }
}
