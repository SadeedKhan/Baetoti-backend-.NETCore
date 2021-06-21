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
    public class UnitController : ApiBaseController
    {
        public readonly IUnitRepository unitRepository;
        public readonly IMapper _mapper;

        [HttpGet("GetAllUnit")]
        public async Task<IActionResult> GetAllUnit()
        {
            try
            {
                var unitlist = (await unitRepository.ListAllAsync()).ToList();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<Unit>>(unitlist)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("AddUnit")]
        public async Task<IActionResult> AddUnit([FromBody] Unit unit)
        {
            try
            {
                var result = await unitRepository.AddAsync(unit);
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

        [HttpPost("UpdateUnit")]
        public async Task<IActionResult> UpdateUnit([FromBody] Unit unit)
        {
            try
            {
                var cat = "";
                //var cat = await unitRepository.GetByIdAsync(unit.ID); Need Confirmation
                if (cat != null)
                {
                    unitRepository.UpdateAsync(unit);
                    return Ok(new SharedResponse(true, 200, "Unit Created Succesfully"));
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

        [HttpPost("DeleteUnit")]
        public async Task<IActionResult> DeleteUnit([FromBody] Unit unit)
        {
            try
            {
                var cat = "";
                //var cat = await unitRepository.GetByIdAsync(unit.ID); Need Confirmation
                if (cat != null)
                {
                    unitRepository.DeleteAsync(unit);
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
