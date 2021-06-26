using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Request.Category;
using Baetoti.Shared.Request.Delete;
using Baetoti.Shared.Response.Category;
using Baetoti.Shared.Response.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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
                    return Ok(new SharedResponse(false, 400, "unable to find category"));
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
                var cat = await _categoryRepository.GetByIdAsync(deleteRequest.ID);
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

        [HttpPost]
        [Route("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {

                if (file.Length > 0)
                {
                    if (CheckIfOnlyImageFile(file))
                    {
                        string fileName = null;
                        var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                        fileName = DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.
                        var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads\\Category");
                        if (!Directory.Exists(pathBuilt))
                        {
                            Directory.CreateDirectory(pathBuilt);
                        }
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads\\Category", fileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        return Ok(new SharedResponse(true, 200, "File uploaded successfully!", "Uploads/Category", fileName, path));
                    }
                    else
                    {
                        return Ok(new SharedResponse(false, 400, "File format is incorrect! (only .png,.jpg,.jpeg) is Supported"));
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

        //Get Image File Extention
        private bool CheckIfOnlyImageFile(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return (extension.ToUpper() == ".PNG" || extension.ToUpper() == ".JPG" || extension.ToUpper() == ".JPEG"); // Change the extension based on your need
        }
    }
}
