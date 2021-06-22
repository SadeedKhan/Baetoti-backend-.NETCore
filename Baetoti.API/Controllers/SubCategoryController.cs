using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Request.SubCategory;
using Baetoti.Shared.Response.Shared;
using Baetoti.Shared.Response.SubCategory;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baetoti.API.Controllers
{
    public class SubCategoryController : ApiBaseController
    {
        public readonly ISubCategoryRepository _subcategoryRepository;
        public readonly IMapper _mapper;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var subcategoryList = (await _subcategoryRepository.ListAllAsync()).ToList();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<SubCategoryResponse>>(subcategoryList)));
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
                var category = await _subcategoryRepository.GetByIdAsync(Id);
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<SubCategoryResponse>(category)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] SubCategoryRequest subcategoryRequest)
        {
            try
            {
                var subcategory = _mapper.Map<SubCategory>(subcategoryRequest);
                subcategory.CreatedAt = DateTime.Now;
                subcategory.CreatedBy = Convert.ToInt32(UserId);
                var result = await _subcategoryRepository.AddAsync(subcategory);
                if (result == null)
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Create SubCategory"));
                }
                return Ok(new SharedResponse(true, 200, "SubCategory Created Succesfully"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] SubCategoryRequest subcategoryRequest)
        {
            try
            {
                var cat = await _subcategoryRepository.GetByIdAsync(subcategoryRequest.ID);
                if (cat != null)
                {

                    var subcategory = _mapper.Map<SubCategory>(subcategoryRequest);
                    subcategory.LastUpdatedAt = DateTime.Now;
                    subcategory.UpdatedBy = Convert.ToInt32(UserId);
                    await _subcategoryRepository.UpdateAsync(subcategory);
                    return Ok(new SharedResponse(true, 200, "SubCategory Updated Succesfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Find SubCategory"));
                }
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] long ID)
        {
            try
            {
                var subcat = await _subcategoryRepository.GetByIdAsync(ID);
                if (subcat != null)
                {
                    await _subcategoryRepository.DeleteAsync(subcat);
                    return Ok(new SharedResponse(true, 200, "SubCategory Deleted Succesfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Find SubCategory"));
                }
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }
    }
}
