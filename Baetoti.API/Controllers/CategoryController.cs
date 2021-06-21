using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Request.Category;
using Baetoti.Shared.Response.Category;
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

        public readonly ICategoryRepository _categoryRepository;
        public readonly IMapper _mapper;

        public CategoryController(
            ICategoryRepository categoryRepository,
            IMapper mapper
            )
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categoryList = (await _categoryRepository.ListAllAsync()).ToList();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<CategoryResponse>>(categoryList)));
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
                var category = await _categoryRepository.GetByIdAsync(Id);
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<CategoryResponse>(category)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] CategoryRequest categoryRequest)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryRequest);
                category.CreatedAt = DateTime.Now;
                category.CreatedBy = Convert.ToInt32(UserId);
                var result = await _categoryRepository.AddAsync(category);
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

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] CategoryRequest categoryRequest)
        {
            try
            {
                var cat = await _categoryRepository.GetByIdAsync(categoryRequest.ID);
                if (cat != null)
                {

                    var category = _mapper.Map<Category>(categoryRequest);
                    category.LastUpdatedAt = DateTime.Now;
                    category.UpdatedBy = Convert.ToInt32(UserId);
                    await _categoryRepository.UpdateAsync(category);
                    return Ok(new SharedResponse(true, 200, "Category Updated Succesfully"));
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

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] long ID)
        {
            try
            {
                var cat = await _categoryRepository.GetByIdAsync(ID);
                if (cat != null)
                {
                    await _categoryRepository.DeleteAsync(cat);
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
