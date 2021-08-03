using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.API.Helpers;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Enum;
using Baetoti.Shared.Request.Item;
using Baetoti.Shared.Response.FileUpload;
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
        public readonly IChangeItemRepository _ChangeitemRepository;
        public readonly IChangeItemTagRepository _ChangeitemTagRepository;
        public readonly IMapper _mapper;

        public ItemController(
            IItemRepository itemRepository,
            IItemTagRepository itemTagRepository,
             IChangeItemRepository changeitemRepository,
            IChangeItemTagRepository changeitemTagRepository,
            IMapper mapper
            )
        {
            _itemRepository = itemRepository;
            _itemTagRepository = itemTagRepository;
            _ChangeitemRepository = changeitemRepository;
            _ChangeitemTagRepository = changeitemTagRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var itemList = await _itemRepository.GetAll();
                return Ok(new SharedResponse(true, 200, "", itemList));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpGet("GetAllChangeRequest")]
        public async Task<IActionResult> GetAllChangeRequest()
        {
            try
            {
                var tempitemList = (await _ChangeitemRepository.ListAllAsync()
                    ).Where(x => x.IsApproved == null).ToList();
                return Ok(new SharedResponse(true, 200, "", tempitemList));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpGet("GetAllCloseRequest")]
        public async Task<IActionResult> GetAllCloseRequest()
        {
            try
            {
                var tempitemList = (await _ChangeitemRepository.ListAllAsync()
                    ).Where(x => x.IsApproved != null).ToList();
                return Ok(new SharedResponse(true, 200, "", tempitemList));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("GetFilteredData")]
        public async Task<IActionResult> GetFilteredData([FromBody] FilterRequest filterRequest)
        {
            try
            {
                //var itemsData = await _itemRepository.GetFilteredItemsDataAsync(filterRequest);
                var itemList = await _itemRepository.GetAll();
                return Ok(new SharedResponse(true, 200, "", itemList));
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
                var item = await _itemRepository.GetByID(Id);
                return Ok(new SharedResponse(true, 200, "", item));
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
                    CategoryID = itemRequest.CategoryID,
                    SubCategoryID = itemRequest.SubCategoryID,
                    ProviderID = itemRequest.ProviderID,
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
                var changeitem = new ChangeItem
                {
                    ItemId = itemRequest.ID,
                    Name = itemRequest.Name,
                    ArabicName = itemRequest.ArabicName,
                    Description = itemRequest.Description,
                    CategoryID = itemRequest.CategoryID,
                    SubCategoryID = itemRequest.SubCategoryID,
                    ProviderID = itemRequest.ProviderID,
                    UnitID = itemRequest.UnitID,
                    Price = itemRequest.Price,
                    Picture = itemRequest.Picture,
                    IsApproved = null
                };
                var addedChangeItem = await _ChangeitemRepository.AddAsync(changeitem);
                var changeitemTags = new List<ChangeItemTag>();
                foreach (var tag in itemRequest.Tags)
                {
                    var changeitemTag = new ChangeItemTag
                    {
                        ItemID = itemRequest.ID,
                        TagID = tag.ID
                    };
                    changeitemTags.Add(changeitemTag);
                }
                var addedChangeItemTags = await _ChangeitemTagRepository.AddRangeAsync(changeitemTags);
                if (addedChangeItem == null || addedChangeItemTags == null)
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Update Item"));
                }
                return Ok(new SharedResponse(true, 200, "Item Update Request Sent Successfully"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpPost("RequestApprovel")]
        public async Task<IActionResult> RequestApprovel([FromBody] ItemRequestApprovel itemRequest)
        {
            try
            {
                var changeitem = ((await _ChangeitemRepository.ListAllAsync())
                   .Where(x => x.ItemId == itemRequest.ItemID)).FirstOrDefault();
                if (changeitem != null)
                {
                    if (itemRequest.IsApproved == true)
                    {
                        var item = await _itemRepository.GetByIdAsync(changeitem.ID);
                        item.ID = changeitem.ItemId;
                        item.Name = changeitem.Name;
                        item.ArabicName = changeitem.ArabicName;
                        item.Description = changeitem.Description;
                        item.CategoryID = changeitem.CategoryID;
                        item.SubCategoryID = changeitem.SubCategoryID;
                        item.ProviderID = changeitem.ProviderID;
                        item.UnitID = changeitem.UnitID;
                        item.Price = changeitem.Price;
                        if (!string.IsNullOrEmpty(changeitem.Picture))
                        {
                            item.Picture = changeitem.Picture;
                        }
                        item.LastUpdatedAt = DateTime.Now;
                        item.UpdatedBy = Convert.ToInt32(UserId);
                        await _itemRepository.UpdateAsync(item);
                        var existingchangeItemTags = (await _ChangeitemTagRepository.ListAllAsync()).Where(x => x.ItemID == itemRequest.ItemID).ToList();
                        //Delete Existing ItemTags
                        var existingItemTags = (await _itemTagRepository.ListAllAsync()).Where(x => x.ItemID == itemRequest.ItemID).ToList();
                        await _itemTagRepository.DeleteRangeAsync(existingItemTags);

                        var itemTags = new List<ItemTag>();
                        foreach (var tag in existingchangeItemTags)
                        {
                            var itemTag = new ItemTag
                            {
                                ItemID = tag.ItemID,
                                TagID = tag.TagID
                            };
                            itemTags.Add(itemTag);
                        }
                        var addedItemTags = await _itemTagRepository.UpdateRangeAsync(itemTags);
                        if (changeitem == null || existingchangeItemTags == null)
                        {
                            return Ok(new SharedResponse(false, 400, "Unable To Update Item"));
                        }
                        changeitem.IsApproved = true;
                        await _ChangeitemRepository.UpdateAsync(changeitem);
                        return Ok(new SharedResponse(true, 200, "Item Approved Successfully"));
                    }
                    else
                    {
                        changeitem.IsApproved = false;
                        await _ChangeitemRepository.UpdateAsync(changeitem);
                        return Ok(new SharedResponse(true, 200, "Item Rejected Successfully!"));
                    }
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

        [HttpDelete("Delete/{ID}")]
        public async Task<IActionResult> Delete(long ID)
        {
            try
            {
                var item = await _itemRepository.GetByIdAsync(ID);
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
