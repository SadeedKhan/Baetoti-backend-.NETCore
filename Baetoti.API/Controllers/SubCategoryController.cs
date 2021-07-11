using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.API.Helpers;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Request.SubCategory;
using Baetoti.Shared.Response.FileUpload;
using Baetoti.Shared.Response.Shared;
using Baetoti.Shared.Response.SubCategory;
using Microsoft.AspNetCore.Http;
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

        public SubCategoryController(
         ISubCategoryRepository subcategoryRepository,
         IMapper mapper
         )
        {
            _subcategoryRepository = subcategoryRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var subcategoryList = (await _subcategoryRepository.GetByCategoryAsync(0)).ToList();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<SubCategoryResponse>>(subcategoryList)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpGet("GetByCategory")]
        public async Task<IActionResult> GetByCategory(long CategoryId)
        {
            try
            {
                var subcategoryList = (await _subcategoryRepository.GetByCategoryAsync(CategoryId)).ToList();
                return Ok(new SharedResponse(true, 200, "", subcategoryList));
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
                subcategory.MarkAsDeleted = false;
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
                var subcat = await _subcategoryRepository.GetByIdAsync(subcategoryRequest.ID);
                if (subcat != null)
                {
                    subcat.SubCategoryName = subcategoryRequest.SubCategoryName;
                    subcat.SubCategoryArabicName = subcategoryRequest.SubCategoryArabicName;
                    subcat.CategoryId = Convert.ToInt32(subcategoryRequest.CategoryID);
                    subcat.LastUpdatedAt = DateTime.Now;
                    subcat.UpdatedBy = Convert.ToInt32(UserId);
                    await _subcategoryRepository.UpdateAsync(subcat);
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

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] CustomRequest deleteRequest)
        {
            try
            {
                var subcat = await _subcategoryRepository.GetByIdAsync(deleteRequest.ID);
                if (subcat != null)
                {
                    subcat.MarkAsDeleted = true;
                    subcat.LastUpdatedAt = DateTime.Now;
                    subcat.UpdatedBy = Convert.ToInt32(UserId);
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

        [HttpPost]
        [Route("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    UploadImage obj = new UploadImage();
                    FileUploadResponse _RESPONSE = await obj.UploadImageFile(file, "SubCategory");
                    if (string.IsNullOrEmpty(_RESPONSE.Message))
                    {
                        return Ok(new SharedResponse(true, 200, "File uploaded successfully!", _RESPONSE));
                    }
                    else
                    {
                        return Ok(new SharedResponse(true, 400, _RESPONSE.Message));
                    }
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "File is required!"));
                }
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }
    }
}
