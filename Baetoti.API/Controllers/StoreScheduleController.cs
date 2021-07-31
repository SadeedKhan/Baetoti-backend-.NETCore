using AutoMapper;
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
                var storeList = await _storeScheduleRepository.ListAllAsync();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<StoreScheduleResponse>>(storeList)));
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
                var store = await _storeScheduleRepository.GetByIdAsync(Id);
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<StoreScheduleResponse>(store)));
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
                var store = _mapper.Map<StoreSchedule>(storeSchRequest);
                var result = await _storeScheduleRepository.AddAsync(store);
                if (result == null)
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Create Store"));
                }
                return Ok(new SharedResponse(true, 200, "Store Created Successfully"));
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
                if (storeScheduleRequest != null)
                {
                    var sc = _mapper.Map<StoreSchedule>(storeScheduleRequest);
                    await _storeScheduleRepository.UpdateAsync(sc);
                    return Ok(new SharedResponse(true, 200, "Store Updated Successfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "unable to find Store"));
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
                var cat = await _storeScheduleRepository.GetByIdAsync(ID);
                if (cat != null)
                {
                    await _storeScheduleRepository.DeleteAsync(cat);
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
