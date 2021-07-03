using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.API.Helpers;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Enum;
using Baetoti.Shared.Request.Delete;
using Baetoti.Shared.Request.Item;
using Baetoti.Shared.Response.FileUpload;
using Baetoti.Shared.Response.Item;
using Baetoti.Shared.Response.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baetoti.API.Controllers
{
    public class ItemController : ApiBaseController
    {

        public readonly IItemRepository _itemRepository;
        public readonly IItemTagRepository _itemTagRepository;
        public readonly IMapper _mapper;

        public ItemController(
            IItemRepository itemRepository,
            IItemTagRepository itemTagRepository,
            IMapper mapper
            )
        {
            _itemRepository = itemRepository;
            _itemTagRepository = itemTagRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var itemList = (await _itemRepository.ListAllAsync()).ToList();
                return Ok(new SharedResponse(true, 200, "", new List<ItemResponse>()));
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
                var item = await _itemRepository.GetByIdAsync(Id);
                return Ok(new SharedResponse(true, 200, "", new ItemResponse()));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] ItemRequest itemRequest)
        {
            try
            {
                var item = new Item
                {
                    Name = itemRequest.Name,
                    ArabicName = itemRequest.ArabicName,
                    Description = itemRequest.Description,
                    Rating = itemRequest.Rating,
                    CategoryID = itemRequest.CategoryID,
                    SubCategoryID = itemRequest.SubCategoryID,
                    UnitID = itemRequest.UnitID,
                    Price = itemRequest.Price,
                    Picture = itemRequest.Picture,
                    Status = (int)ItemStatus.Active,
                    MarkAsDeleted = false,
                    CreatedAt = DateTime.Now,
                    CreatedBy = Convert.ToInt32(UserId)
                };
                var addedItem = await _itemRepository.AddAsync(item);
                var itemTags = new List<ItemTag>();
                foreach (var tag in itemRequest.Tags)
                {
                    var itemTag = new ItemTag
                    {
                        ItemID = addedItem.ID,
                        TagID = tag.ID
                    };
                    itemTags.Add(itemTag);
                }
                var addedItemTags = await _itemTagRepository.AddRangeAsync(itemTags);
                if (addedItem == null || addedItemTags == null)
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Create Item"));
                }
                return Ok(new SharedResponse(true, 200, "Item Created Successfully"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] ItemRequest itemRequest)
        {
            try
            {
                var item = await _itemRepository.GetByIdAsync(itemRequest.ID);
                if (item != null)
                {

                    item.Name = itemRequest.Name;
                    item.ArabicName = itemRequest.ArabicName;
                    item.CategoryID = itemRequest.CategoryID;
                    item.Description = itemRequest.Description;
                    item.Rating = itemRequest.Rating;
                    item.SubCategoryID = itemRequest.SubCategoryID;
                    item.UnitID = itemRequest.UnitID;
                    item.Price = itemRequest.Price;
                    item.Picture = itemRequest.Picture;
                    item.Status = (int)ItemStatus.Active;
                    item.MarkAsDeleted = false;
                    item.LastUpdatedAt = DateTime.Now;
                    item.UpdatedBy = Convert.ToInt32(UserId);
                    await _itemRepository.UpdateAsync(item);
                    var existingItemTags = (await _itemTagRepository.ListAllAsync()).Where(x => x.ItemID == item.ID);
                    await _itemTagRepository.DeleteRangeAsync(existingItemTags.ToList());
                    var itemTags = new List<ItemTag>();
                    foreach (var tag in itemRequest.Tags)
                    {
                        var itemTag = new ItemTag
                        {
                            ItemID = item.ID,
                            TagID = tag.ID
                        };
                        itemTags.Add(itemTag);
                    }
                    var addedItemTags = await _itemTagRepository.AddRangeAsync(itemTags);
                    if (item == null || addedItemTags == null)
                    {
                        return Ok(new SharedResponse(false, 400, "Unable to Update Item"));
                    }
                    return Ok(new SharedResponse(true, 200, "Item Updated Successfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable to Find Item"));
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
                var item = await _itemRepository.GetByIdAsync(deleteRequest.ID);
                if (item != null)
                {
                    item.MarkAsDeleted = true;
                    item.LastUpdatedAt = DateTime.Now;
                    item.UpdatedBy = Convert.ToInt32(UserId);
                    await _itemRepository.UpdateAsync(item);
                    return Ok(new SharedResponse(true, 200, "Item Deleted Successfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Find Item"));
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
                    FileUploadResponse _RESPONSE = await obj.UploadImageFile(file, "Item");
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
