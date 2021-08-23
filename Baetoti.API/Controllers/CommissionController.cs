using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Response.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baetoti.API.Controllers
{
    public class CommissionController : ApiBaseController
    {
        public readonly ICommissionRepository _commissionRepository;
        public readonly IMapper _mapper;

        public CommissionController(
            ICommissionRepository commissionRepository,
            IMapper mapper
            )
        {
            _commissionRepository = commissionRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var commissionList = (await _commissionRepository.ListAllAsync()).
                    Where(x => x.MarkAsDeleted == false).ToList();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<CategoryResponse>>(commissionList)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var commission = await _commissionRepository.GetByIdAsync(Id);
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<CategoryResponse>(commission)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] CategoryRequest commissionRequest)
        {
            try
            {
                var commission = _mapper.Map<Category>(commissionRequest);
                commission.MarkAsDeleted = false;
                commission.CreatedAt = DateTime.Now;
                commission.CreatedBy = Convert.ToInt32(UserId);
                var result = await _commissionRepository.AddAsync(commission);
                if (result == null)
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Create Category"));
                }
                return Ok(new SharedResponse(true, 200, "Category Created Succesfully"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }
    }
}
