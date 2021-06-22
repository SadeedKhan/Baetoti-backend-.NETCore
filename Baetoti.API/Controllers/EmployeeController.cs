using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Response.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baetoti.API.Controllers
{
    public class EmployeeController : ApiBaseController
    {
        public readonly ICategoryRepository categoryRepository;
        public readonly IMapper _mapper;

        //[HttpGet("GetAllCategory")]
        //public async Task<IActionResult> GetAllCategory()
        //{
        //    try
        //    {
        //        var categorylist = (await categoryRepository.ListAllAsync()).ToList();
        //        return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<Category>>(categorylist)));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new SharedResponse(false, 400, ex.Message, null));
        //    }
        //}
    }
}
