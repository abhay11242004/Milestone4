﻿using ECommerce.Api.Search.Interfaces;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService _ordersService;
        private readonly IProductsService _productsService;
        private readonly ICustomersService _customersService;
        public SearchService(IOrdersService orderService, IProductsService productsService, ICustomersService customersService)
        {
            _ordersService = orderService;
            _productsService = productsService;
            _customersService = customersService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var customersResult = await _customersService.GetCustomerAsync(customerId);
            var ordersResult = await _ordersService.GetOrderAsync(customerId);
            var productsResult = await _productsService.GetProductsAsync();

            if (ordersResult.IsSuccess)
            {
                foreach (var order in ordersResult.Orders)
                {
                    foreach (var item in order.Items)
                    {
                        item.ProductName = productsResult.IsSuccess ?  productsResult.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name : "Product information is not available";
                    }
                }
                var result = new
                {
                    Customer = customersResult.IsSuccess ? customersResult.Customer : new { Name = "Customer information is not available"},
                    Orders = ordersResult.Orders
                };
                return (true, result);
            }
            return (false, null);
        }
    }
}
