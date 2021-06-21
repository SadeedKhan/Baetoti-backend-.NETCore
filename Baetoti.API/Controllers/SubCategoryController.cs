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
    public class SubCategoryController : ApiBaseController
    {
        public readonly ISubCategoryRepository subcategoryRepository;
        public readonly IMapper _mapper;

        [HttpGet("GetAllSubCategory")]
        public async Task<IActionResult> GetAllSubCategory()
        {
            try
            {
                var subcategorylist = (await subcategoryRepository.ListAllAsync()).ToList();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<SubCategory>>(subcategorylist)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("AddSubCategory")]
        public async Task<IActionResult> AddSubCategory([FromBody] SubCategory subcategory)
        {
            try
            {
                var result = await subcategoryRepository.AddAsync(subcategory);
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

        [HttpPost("UpdateSubCategory")]
        public async Task<IActionResult> UpdateSubCategory([FromBody] SubCategory subcategory)
        {
            try
            {
                var cat="";
                //var cat = await subcategoryRepository.GetByIdAsync(subcategory.ID); Need Confirmation
                if (cat != null)
                {
                    subcategoryRepository.UpdateAsync(subcategory);
                    return Ok(new SharedResponse(true, 200, "SubCategory Created Succesfully"));
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

        [HttpPost("DeleteSubCategory")]
        public async Task<IActionResult> DeleteSubCategory([FromBody] SubCategory subcategory)
        {
            try
            {
                var cat = "";
                //var cat = await subcategoryRepository.GetByIdAsync(subcategory.ID); Need Confirmation
                if (cat != null)
                {
                    subcategoryRepository.DeleteAsync(subcategory);
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
