﻿using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Request.Delete;
using Baetoti.Shared.Request.UnitRequest;
using Baetoti.Shared.Response.Shared;
using Baetoti.Shared.Response.Unit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baetoti.API.Controllers
{
    public class UnitController : ApiBaseController
    {
        public readonly IUnitRepository _unitRepository;
        public readonly IMapper _mapper;

        public UnitController(
         IUnitRepository unitRepository,
         IMapper mapper
         )
        {
            _unitRepository = unitRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var unitList = (await _unitRepository.ListAllAsync()).ToList();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<UnitResponse>>(unitList)));
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
                var unit = await _unitRepository.GetByIdAsync(Id);
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<UnitResponse>(unit)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] UnitRequest unitRequest)
        {
            try
            {
                var unit = _mapper.Map<Unit>(unitRequest);
                unit.CreatedAt = DateTime.Now;
                unit.CreatedBy = Convert.ToInt32(UserId);
                var result = await _unitRepository.AddAsync(unit);
                if (result == null)
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Create Unit"));
                }
                return Ok(new SharedResponse(true, 200, "Unit Created Succesfully"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] UnitRequest unitRequest)
        {
            try
            {
                var cat = await _unitRepository.GetByIdAsync(unitRequest.ID);
                if (cat != null)
                {

                    var unit = _mapper.Map<Unit>(unitRequest);
                    unit.LastUpdatedAt = DateTime.Now;
                    unit.UpdatedBy = Convert.ToInt32(UserId);
                    await _unitRepository.UpdateAsync(unit);
                    return Ok(new SharedResponse(true, 200, "Unit Updated Succesfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Find Unit"));
                }
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteRequest deleteRequest)
        {
            try
            {
                var un = await _unitRepository.GetByIdAsync(deleteRequest.ID);
                if (un != null)
                {
                    await _unitRepository.DeleteAsync(un);
                    return Ok(new SharedResponse(true, 200, "Unit Deleted Succesfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Find Unit"));
                }
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }
    }
}
