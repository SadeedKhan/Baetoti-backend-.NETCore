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
    public class CategoryController : ApiBaseController
    {

        public readonly ICategoryRepository categoryRepository;
        public readonly IMapper _mapper;

        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            try
            {
                var categorylist = (await categoryRepository.ListAllAsync()).ToList();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<Category>>(categorylist)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            try
            {
                var result = await categoryRepository.AddAsync(category);
                if(result==null)
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

        [HttpPost("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] Category category)
        {
            try
            {
                var cat = await categoryRepository.GetByIdAsync(category.CategoryId);
                if(cat!=null)
                {
                    categoryRepository.UpdateAsync(category);
                    return Ok(new SharedResponse(true, 200, "Category Created Succesfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Find Category"));
                }
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpPost("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory([FromBody] Category category)
        {
            try
            {
                var cat = await categoryRepository.GetByIdAsync(category.CategoryId);
                if (cat != null)
                {
                    categoryRepository.DeleteAsync(category);
                    return Ok(new SharedResponse(true, 200, "Category Deleted Succesfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Find Category"));
                }
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }
    }
}
