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
    public class VATController : ApiBaseController
    {
        public readonly IVATRepository _vatRepository;
        public readonly IMapper _mapper;

        public VATController(
            IVATRepository vatRepository,
            IMapper mapper
            )
        {
            _vatRepository = vatRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var vatList = (await _vatRepository.ListAllAsync()).
                    Where(x => x.MarkAsDeleted == false).ToList();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<CategoryResponse>>(vatList)));
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
                var vat = await _vatRepository.GetByIdAsync(Id);
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<CategoryResponse>(vat)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] VAT vatRequest)
        {
            try
            {
                var vat = _mapper.Map<VAT>(vatRequest);
                vat.MarkAsDeleted = false;
                vat.CreatedAt = DateTime.Now;
                vat.CreatedBy = Convert.ToInt32(UserId);
                var result = await _vatRepository.AddAsync(vat);
                if (result == null)
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Create VAT Tax"));
                }
                return Ok(new SharedResponse(true, 200, "VAT Created Succesfully"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }
    }
}
