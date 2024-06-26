﻿using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Request.StoreSchedule;
using Baetoti.Shared.Response.Shared;
using Baetoti.Shared.Response.StoreSchedule;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baetoti.API.Controllers
{
    public class StoreScheduleController : ApiBaseController
    {
        public readonly IStoreScheduleRepository _storeScheduleRepository;
        public readonly IMapper _mapper;

        public StoreScheduleController(
           IStoreScheduleRepository storeScheduleRepository,
           IMapper mapper
           )
        {
            _storeScheduleRepository = storeScheduleRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var storeScheduleList = await _storeScheduleRepository.ListAllAsync();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<StoreScheduleResponse>>(storeScheduleList)));
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
                var storeSchedule = await _storeScheduleRepository.GetByIdAsync(Id);
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<StoreScheduleResponse>(storeSchedule)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] StoreScheduleRequest storeSchRequest)
        {
            try
            {
                var storeSchedule = _mapper.Map<StoreSchedule>(storeSchRequest);
                storeSchedule.MarkAsDeleted = false;
                storeSchedule.CreatedBy = Convert.ToInt32(UserId);
                storeSchedule.CreatedAt = DateTime.Now;
                var result = await _storeScheduleRepository.AddAsync(storeSchedule);
                if (result == null)
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Create StoreSchedule"));
                }
                return Ok(new SharedResponse(true, 200, "StoreSchedule Created Successfully"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] StoreScheduleRequest storeScheduleRequest)
        {
            try
            {
                var storeSchedule = await _storeScheduleRepository.GetByIdAsync(storeScheduleRequest.ID);
                if (storeScheduleRequest != null)
                {
                    storeSchedule.StoreID = storeScheduleRequest.StoreID;
                    storeSchedule.StartTime = storeScheduleRequest.StartTime;
                    storeSchedule.Day = storeScheduleRequest.Day;
                    storeSchedule.EndTime = storeScheduleRequest.EndTime;
                    storeSchedule.UpdatedBy = Convert.ToInt32(UserId);
                    storeSchedule.LastUpdatedAt = DateTime.Now;
                    await _storeScheduleRepository.UpdateAsync(storeSchedule);
                    return Ok(new SharedResponse(true, 200, "StoreSchedule Updated Successfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "unable to find StoreSchedule"));
                }
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpDelete("Delete/{ID}")]
        public async Task<IActionResult> Delete(long ID)
        {
            try
            {
                var storeSchedule = await _storeScheduleRepository.GetByIdAsync(ID);
                if (storeSchedule != null)
                {
                    storeSchedule.MarkAsDeleted = true;
                    storeSchedule.UpdatedBy = Convert.ToInt32(UserId); 
                    storeSchedule.LastUpdatedAt = DateTime.Now;
                    await _storeScheduleRepository.UpdateAsync(storeSchedule);
                    return Ok(new SharedResponse(true, 200, "StoreSchedule Deleted Succesfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Find StoreSchedule"));
                }
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }


    }
}
