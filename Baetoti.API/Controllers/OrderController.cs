using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.Core.Interface.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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



    }
}
