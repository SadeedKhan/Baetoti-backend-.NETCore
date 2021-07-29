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
        public readonly ITempItemRepository _TempitemRepository;
        public readonly ITempItemTagRepository _TempitemTagRepository;
        public readonly IMapper _mapper;

        public ItemController(
            IItemRepository itemRepository,
            IItemTagRepository itemTagRepository,
             ITempItemRepository tempitemRepository,
            ITempItemTagRepository tempitemTagRepository,
            IMapper mapper
            )
        {
            _itemRepository = itemRepository;
            _itemTagRepository = itemTagRepository;
            _TempitemRepository = tempitemRepository;
            _TempitemTagRepository = tempitemTagRepository;
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

        //[HttpPost("GetFilteredData")]
        //public async Task<IActionResult> GetFilteredData([FromBody] FilterRequest filterRequest)
        //{
        //    try
        //    {
        //        var itemsData = await _itemRepository.GetFilteredItemsDataAsync(filterRequest);
        //        return Ok(new SharedResponse(true, 200, "", itemsData));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new SharedResponse(false, 400, ex.Message, null));
        //    }
        //}

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
                var tempitem = new TempItem
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
                };
                var addedTempItem = await _TempitemRepository.AddAsync(tempitem);
                var tempitemTags = new List<TempItemTag>();
                foreach (var tag in itemRequest.Tags)
                {
                    var tempitemTag = new TempItemTag
                    {
                        ItemID = itemRequest.ID,
                        TagID = tag.ID
                    };
                    tempitemTags.Add(tempitemTag);
                }
                var addedTempItemTags = await _TempitemTagRepository.AddRangeAsync(tempitemTags);
                if (addedTempItem == null || addedTempItemTags == null)
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
                if (itemRequest.Approvel == true)
                {
                    var tempitem = ((await _TempitemRepository.ListAllAsync())
                        .Where(x=>x.ItemId== itemRequest.ItemID)).FirstOrDefault();
                    if (tempitem != null)
                    {
                        var item = await _itemRepository.GetByIdAsync(tempitem.ID);

                        item.ID = tempitem.ItemId;
                        item.Name = tempitem.Name;
                        item.ArabicName = tempitem.ArabicName;
                        item.Description = tempitem.Description;
                        item.CategoryID = tempitem.CategoryID;
                        item.SubCategoryID = tempitem.SubCategoryID;
                        item.ProviderID = tempitem.ProviderID;
                        item.UnitID = tempitem.UnitID;
                        item.Price = tempitem.Price;
                        if(!string.IsNullOrEmpty(tempitem.Picture))
                        {
                            item.Picture = tempitem.Picture;
                        }
                        item.LastUpdatedAt = DateTime.Now;
                        item.UpdatedBy = Convert.ToInt32(UserId);
                        await _itemRepository.UpdateAsync(item);
                        var existingtempItemTags = (await _TempitemTagRepository.ListAllAsync()).Where(x => x.ItemID == itemRequest.ItemID);

                        var itemTags = new List<ItemTag>();
                        foreach (var tag in existingtempItemTags)
                        {
                            var itemTag = new ItemTag
                            {
                                ItemID = tag.ItemID,
                                TagID = tag.ID
                            };
                            itemTags.Add(itemTag);
                        }
                        var addedItemTags = await _itemTagRepository.UpdateRangeAsync(itemTags);
                        if (tempitem == null || existingtempItemTags == null)
                        {
                            return Ok(new SharedResponse(false, 400, "Unable To Update Item"));
                        }
                        return Ok(new SharedResponse(true, 200, "Item Approved Successfully"));
                    }
                    else
                    {
                        return Ok(new SharedResponse(false, 400, "Unable to Find Item"));
                    }
                }
                else
                {
                    var tempitem = await _TempitemRepository.GetByIdAsync(itemRequest.ItemID);
                    if (tempitem != null)
                    {
                        await _TempitemRepository.DeleteAsync(tempitem);
                        var existingtempItemTags = (await _TempitemTagRepository.ListAllAsync()).Where(x => x.ItemID == itemRequest.ItemID);

                        var itemTags = new List<ItemTag>();
                        foreach (var tag in existingtempItemTags)
                        {
                            var itemTag = new ItemTag
                            {
                                ItemID = tag.ItemID,
                                TagID = tag.TagID
                            };
                            itemTags.Add(itemTag);
                        }
                        await _itemTagRepository.DeleteRangeAsync(itemTags);
                        return Ok(new SharedResponse(true, 200, "Item Rejected Successfully!"));
                    }
                    else
                    {
                        return Ok(new SharedResponse(false, 400, "Unable to Find Item"));
                    }
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
