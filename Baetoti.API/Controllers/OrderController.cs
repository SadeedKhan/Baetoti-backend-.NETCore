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
        public readonly IOrderRepository _orderRepository;
        public readonly IOrderItemRepository _orderItemRepository;
        public readonly IMapper _mapper;

        public OrderController(
            IOrderRepository cartRepository,
            IOrderItemRepository orderRepository,
            IMapper mapper
            )
        {
            _orderRepository = cartRepository;
            _orderItemRepository = orderRepository;
            _mapper = mapper;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] OrderRequest orderRequest)
        {
            try
            {
                var cart = new Order
                {
                    UserID = orderRequest.UserIID,
                    NotesForDriver = orderRequest.NotesForDriver,
                    DeliveryAddress = orderRequest.DeliveryAddress,
                    ExpectedDeliveryTime = orderRequest.ExpectedDeliveryTime,
                    Status = (int)OrderStatus.Pending,
                    Type = (int)OrderType.Delivery,
                    CreatedAt = DateTime.Now,
                    CreatedBy = Convert.ToInt32(UserId)
                };
                var addedCart = await _orderRepository.AddAsync(cart);
                var orders = new List<OrderItem>();
                foreach (var item in orderRequest.Items)
                {
                    var order = new OrderItem
                    {
                        OrderID = addedCart.ID,
                        ItemID = item.ItemID,
                        Quantity = item.Quantity,
                        Comments = item.Comments
                    };
                    orders.Add(order);
                }
                var addedOrders = await _orderItemRepository.AddRangeAsync(orders);
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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var orderList = await _orderItemRepository.GetAll();
                return Ok(new SharedResponse(true, 200, "", orderList));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpGet("View")]
        public async Task<IActionResult> View(int Id)
        {
            try
            {
                var item = await _orderItemRepository.GetByID(Id);
                return Ok(new SharedResponse(true, 200, "", item));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

    }
}
