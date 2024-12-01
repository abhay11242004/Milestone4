using ECommerce.Api.Search.Interfaces;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService _ordersService;
        private readonly IProductsService _productsService;
        public SearchService(IOrdersService orderService, IProductsService productsService)
        {
            _ordersService = orderService;
            _productsService = productsService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var ordersResult = await _ordersService.GetOrderAsync(customerId);
            var productsResult = await _productsService.GetProductsAsync();

            if (ordersResult.IsSuccess)
            {
                foreach (var order in ordersResult.Orders)
                {
                    foreach (var item in order.Items)
                    {
                        item.ProductName = productsResult.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name;
                    }
                }
                var result = new
                {
                    Orders = ordersResult.Orders
                };
                return (true, result);
            }
            return (false, null);
        }
    }
}
