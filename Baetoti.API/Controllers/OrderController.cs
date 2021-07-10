using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Enum;
using Baetoti.Shared.Request.Order;
using Baetoti.Shared.Response.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baetoti.API.Controllers
{
    public class OrderController : ApiBaseController
    {
        public readonly ICartRepository _cartRepository;
        public readonly IOrderRepository _orderRepository;
        public readonly IMapper _mapper;

        public OrderController(
            ICartRepository cartRepository,
            IOrderRepository orderRepository,
            IMapper mapper
            )
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] OrderRequest orderRequest)
        {
            try
            {
                var cart = new Cart
                {
                    UserIID = orderRequest.UserIID,
                    NotesForDriver = orderRequest.NotesForDriver,
                    DeliveryAddress = orderRequest.DeliveryAddress,
                    ExpectedDeliveryTime = orderRequest.ExpectedDeliveryTime,
                    Status = (int)OrderStatus.Pending,
                    CreatedAt = DateTime.Now,
                    CreatedBy = Convert.ToInt32(UserId)
                };
                var addedCart = await _cartRepository.AddAsync(cart);
                var orders = new List<Order>();
                foreach (var item in orderRequest.Items)
                {
                    var order = new Order
                    {
                        CartID = addedCart.ID,
                        ItemID = item.ItemID,
                        Quantity = item.Quantity,
                        Comments = item.Comments
                    };
                    orders.Add(order);
                }
                var addedOrders = await _orderRepository.AddRangeAsync(orders);
                if (addedCart == null || addedOrders == null)
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Submit Order"));
                }
                return Ok(new SharedResponse(true, 200, "Order Submitted Successfully"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        //[HttpGet("GetAll")]
        //public async Task<IActionResult> GetAll()
        //{
        //    try
        //    {
        //        var itemList = await _orderRepository.GetAll();
        //        return Ok(new SharedResponse(true, 200, "", itemList));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new SharedResponse(false, 400, ex.Message, null));
        //    }
        //}

    }
}
