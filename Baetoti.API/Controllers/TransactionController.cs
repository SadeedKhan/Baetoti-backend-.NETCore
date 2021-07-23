﻿using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Response.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Baetoti.API.Controllers
{
    public class TransactionController : ApiBaseController
    {

        public readonly ITransactionRepository _transactionRepository;
        public readonly IMapper _mapper;

        public TransactionController(
            ITransactionRepository transactionRepository,
            IMapper mapper
            )
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var usersData = await _transactionRepository.GetAll();
                return Ok(new SharedResponse(true, 200, "", usersData));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

    }
}