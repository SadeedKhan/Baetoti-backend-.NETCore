using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.API.Helpers;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Request.Store;
using Baetoti.Shared.Response.FileUpload;
using Baetoti.Shared.Response.Shared;
using Baetoti.Shared.Response.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baetoti.API.Controllers
{
    public class StoreController : ApiBaseController
    {
        public readonly IStoreRepository _storeRepository;
        public readonly IStoreTagRepository _storeTagRepository;
        public readonly IMapper _mapper;

        public StoreController(
            IStoreRepository storeRepository,
            IStoreTagRepository storeTagRepository,
            IMapper mapper
            )
        {
            _storeRepository = storeRepository;
            _storeTagRepository = storeTagRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var storeList = (await _storeRepository.ListAllAsync()).
                    Where(x => x.MarkAsDeleted == false).ToList();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<StoreResponse>>(storeList)));
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
                var store = await _storeRepository.GetByIdAsync(Id);
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<StoreResponse>(store)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] StoreRequest storeRequest)
        {
            try
            {
                var store = _mapper.Map<Store>(storeRequest);
                store.MarkAsDeleted = false;
                store.CreatedAt = DateTime.Now;
                store.CreatedBy = Convert.ToInt32(UserId);
                var result = await _storeRepository.AddAsync(store);
                var storeTags = new List<StoreTag>();
                foreach (var tag in storeRequest.Tags)
                {
                    var storeTag = new StoreTag
                    {
                        StoreID = result.ID,
                        TagID = tag.ID
                    };
                    storeTags.Add(storeTag);
                }
                var addedStoreTags = await _storeTagRepository.AddRangeAsync(storeTags);
                if (result == null || addedStoreTags == null)
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
        public async Task<IActionResult> Update([FromBody] StoreRequest storeRequest)
        {
            try
            {
                var cat = await _storeRepository.GetByIdAsync(storeRequest.ID);
                if (cat != null)
                {
                    cat.Name = storeRequest.Name;
                    cat.Description = storeRequest.Description;
                    cat.Location = storeRequest.Location;
                    cat.IsAddressHidden = storeRequest.IsAddressHidden;
                    if(!string.IsNullOrEmpty(storeRequest.BusinessLogo))
                    {
                        cat.BusinessLogo = storeRequest.BusinessLogo;
                    }
                    if (!string.IsNullOrEmpty(storeRequest.CoverImage))
                    {
                        cat.CoverImage = storeRequest.CoverImage;
                    }
                    if (!string.IsNullOrEmpty(storeRequest.InstagramGallery))
                    {
                        cat.InstagramGallery = storeRequest.InstagramGallery;
                    }
                    cat.LastUpdatedAt = DateTime.Now;
                    cat.UpdatedBy = Convert.ToInt32(UserId);
                    await _storeRepository.UpdateAsync(cat);
                    var existingstoreTags = (await _storeTagRepository.ListAllAsync()).Where(x => x.StoreID == storeRequest.ID);
                    var storeTags = new List<StoreTag>();
                    foreach (var tag in existingstoreTags)
                    {
                        var storeTag = new StoreTag
                        {
                            StoreID = storeRequest.ID,
                            TagID = tag.ID
                        };
                        storeTags.Add(storeTag);
                    }
                    var addedItemTags = await _storeTagRepository.UpdateRangeAsync(storeTags);
                    if (cat == null || existingstoreTags == null)
                    {
                        return Ok(new SharedResponse(false, 400, "Unable To Update Store"));
                    }
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
                var cat = await _storeRepository.GetByIdAsync(ID);
                if (cat != null)
                {
                    cat.MarkAsDeleted = true;
                    cat.LastUpdatedAt = DateTime.Now;
                    cat.UpdatedBy = Convert.ToInt32(UserId);
                    await _storeRepository.DeleteAsync(cat);
                    return Ok(new SharedResponse(true, 200, "Store Deleted Succesfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Find Store"));
                }
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpPost]
        [Route("UploadCoverImageFile")]
        public async Task<IActionResult> UploadCoverImageFile(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    UploadImage obj = new UploadImage();
                    FileUploadResponse _RESPONSE = await obj.UploadImageFile(file, "CoverImage");
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

        [HttpPost]
        [Route("UploadInstagramGalleryFile")]
        public async Task<IActionResult> UploadInstagramGalleryFile(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    UploadImage obj = new UploadImage();
                    FileUploadResponse _RESPONSE = await obj.UploadImageFile(file, "InstagramGallery");
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

        [HttpPost]
        [Route("UploadBusinessLogoFile")]
        public async Task<IActionResult> UploadBusinessLogoFile(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    UploadImage obj = new UploadImage();
                    FileUploadResponse _RESPONSE = await obj.UploadImageFile(file, "BusinessLogo");
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
