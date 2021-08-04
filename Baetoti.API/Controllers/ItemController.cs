using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.API.Helpers;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Enum;
using Baetoti.Shared.Request.ChangeItem;
using Baetoti.Shared.Request.Item;
using Baetoti.Shared.Response.ChangeItem;
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
      
        [HttpPost("GetFilteredData")]
        public async Task<IActionResult> GetFilteredData([FromBody] ItemFilterRequest filterRequest)
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

        //Change Item  Requests
        [HttpGet("GetAllChangeRequest")]
        public async Task<IActionResult> GetAllChangeRequest()
        {
            try
            {
                var changeitemList = (await _ChangeitemRepository.ListAllAsync()
                    ).Where(x => x.IsApproved == null).ToList();
                return Ok(new SharedResponse(true, 200, "", changeitemList));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }


        [HttpGet("GetChangeRequestByID")]
        public async Task<IActionResult> GetChangeRequestByID(long Id)
        {
            try
            {
                var changeitem = await _ChangeitemRepository.GetByItemID(Id);
                return Ok(new SharedResponse(true, 200, "", changeitem));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpGet("ViewItemChangeRequest")]
        public async Task<IActionResult> ViewItemChangeRequest(long Id)
        {
            try
            {
                var item = await _itemRepository.GetByID(Id);
                var changeitem = await _ChangeitemRepository.GetByItemID(Id);
                List<Dictionary<String, String>> DictList = new List<Dictionary<String, String>>();
                Dictionary<String,String> Dict;
                if (changeitem != null && item !=null)
                {
                    if (item.AveragePreparationTime != changeitem.AveragePreparationTime)
                    {
                        Dict = new Dictionary<String, String>()
                                            {
                                                {"OldAveragePreparationTime",item.AveragePreparationTime.ToString() },
                                                {"NewAveragePreparationTime",changeitem.AveragePreparationTime.ToString() }
                                            };
                        DictList.Add(Dict);
                    }
                    if (item.AverageRating != changeitem.AverageRating)
                    {
                        Dict = new Dictionary<String, String>()
                                            {
                                                {"OldAverageRating",item.AverageRating.ToString() },
                                                {"NewAverageRating",changeitem.AverageRating.ToString() }
                                            };
                        DictList.Add(Dict);
                    }
                    if (item.AvailableNow != changeitem.AvailableNow)
                    {
                        Dict = new Dictionary<String, String>()
                                            {
                                                {"OldAvailableNow",item.AvailableNow.ToString() },
                                                {"NewAvailableNow",changeitem.AvailableNow.ToString() }
                                            };
                        DictList.Add(Dict);
                    }
                    if (item.Description != changeitem.Description)
                    {
                        Dict = new Dictionary<String, String>()
                                            {
                                                {"OldDescription",item.Description },
                                                {"NewDescription",changeitem.Description }
                                            };
                        DictList.Add(Dict);
                    }
                    if (item.Category != changeitem.Category)
                    {
                        Dict = new Dictionary<String, String>()
                                            {
                                                {"OldCategory",item.Category },
                                                {"NewCategory",changeitem.Category }
                                            };
                        DictList.Add(Dict);
                    }
                    if (item.Location != changeitem.Location)
                    {
                        Dict = new Dictionary<String, String>()
                                            {
                                                {"OldLocation",item.Location },
                                                {"NewLocation",changeitem.Location }
                                            };
                        DictList.Add(Dict);
                    }

                    if (item.Price != changeitem.Price)
                    {
                        Dict = new Dictionary<String, String>()
                                            {
                                                {"OldPrice",item.Price },
                                                {"NewPrice",changeitem.Price }
                                            };
                        DictList.Add(Dict);
                    }

                    if (item.Quantity != changeitem.Quantity)
                    {
                        Dict = new Dictionary<String, String>()
                                            {
                                                {"OldQuantity",item.Quantity.ToString() },
                                                {"NewQuantity",changeitem.Quantity.ToString() }
                                            };
                        DictList.Add(Dict);
                    }
                    if (item.SubCategory != changeitem.SubCategory)
                    {
                        Dict = new Dictionary<String, String>()
                                            {
                                                {"OldSubCategory",item.SubCategory },
                                                {"NewSubCategory",changeitem.SubCategory }
                                            };
                        DictList.Add(Dict);
                    }
                    if (item.Sold != changeitem.Sold)
                    {
                        Dict = new Dictionary<String, String>()
                                            {
                                                {"OldSold",item.Sold.ToString() },
                                                {"NewSold",changeitem.Sold.ToString() }
                                            };
                        DictList.Add(Dict);
                    }
                    if (item.StoreName != changeitem.StoreName)
                    {
                        Dict = new Dictionary<String, String>()
                                            {
                                                {"OldStoreName",item.StoreName },
                                                {"NewStoreName",changeitem.StoreName }
                                            };
                        DictList.Add(Dict);
                    }
                    if (item.Title != changeitem.Title)
                    {
                        Dict = new Dictionary<String, String>()
                                            {
                                                {"OldTitle",item.Title },
                                                {"NewTitle",changeitem.Title }
                                            };
                        DictList.Add(Dict);
                    }

                    if (item.TotalRevenue != changeitem.TotalRevenue)
                    {
                        Dict = new Dictionary<String, String>()
                                            {
                                                {"OldTotalRevenue",item.TotalRevenue.ToString() },
                                                {"NewTotalRevenue",changeitem.TotalRevenue.ToString() }
                                            };
                        DictList.Add(Dict);
                    }
                    if (item.Unit != changeitem.Unit)
                    {
                        Dict = new Dictionary<String, String>()
                                            {
                                                {"OldUnit",item.Unit },
                                                {"NewUnit",changeitem.Unit }
                                            };
                        DictList.Add(Dict);
                    }
                    return Ok(new SharedResponse(true, 200, "", DictList));
                }
                else
                {
                    return Ok(new SharedResponse(true, 400, "Unable to Find Item"));

                }

            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }


        [HttpPost("UpdateItemChangeRequestByID")]
        public async Task<IActionResult> UpdateItemChangeRequestByID([FromBody] ChangeItemRequest changeitemRequest)
        {
            try
            {
                var changeitem = await _ChangeitemRepository.GetByIdAsync(changeitemRequest.ID);
                if(changeitem != null)
                {
                    var updateitem = _mapper.Map<ChangeItem>(changeitemRequest);
                    changeitem.Name = updateitem.Name;
                    changeitem.ArabicName = updateitem.ArabicName;
                    changeitem.Description = updateitem.Description;
                    changeitem.CategoryID = updateitem.CategoryID;
                    changeitem.SubCategoryID = updateitem.SubCategoryID;
                    changeitem.Price = updateitem.Price;
                    changeitem.Rating = updateitem.Rating;
                    await _ChangeitemRepository.UpdateAsync(changeitem);

                    //Delete Existing ChangeItemTags
                    var existingItemTags = (await _ChangeitemTagRepository.ListAllAsync()).Where(x => x.ItemID == changeitemRequest.ItemID).ToList();
                    await _ChangeitemTagRepository.DeleteRangeAsync(existingItemTags);

                    var changeitemTags = new List<ChangeItemTag>();
                    foreach (var tag in changeitemRequest.Tags)
                    {
                        var changeitemTag = new ChangeItemTag
                        {
                            ItemID = changeitemRequest.ID,
                            TagID = tag.ID
                        };
                        changeitemTags.Add(changeitemTag);
                    }
                    var addedChangeItemTags = await _ChangeitemTagRepository.AddRangeAsync(changeitemTags);
                    if (addedChangeItemTags == null)
                    {
                        return Ok(new SharedResponse(false, 400, "Unable To Update Item"));
                    }
                    return Ok(new SharedResponse(true, 200, "Item Update Request Sent Successfully"));
                }
                else
                {
                    return Ok(new SharedResponse(true, 400, "Unable to Find Item"));

                }

            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        //Completion of Changing or Updating Item  Requests

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


        [HttpGet("GetAllCloseRequest")]
        public async Task<IActionResult> GetAllCloseRequest()
        {
            try
            {
                var changeitemList = (await _ChangeitemRepository.ListAllAsync()
                    ).Where(x => x.IsApproved != null).ToList();
                return Ok(new SharedResponse(true, 200, "", changeitemList));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpGet("GetCloseRequestByItemID")]
        public async Task<IActionResult> GetCloseRequestByID(long Id)
        {
            try
            {
                var changeitemList = await _ChangeitemRepository.GetByItemID(Id);
                return Ok(new SharedResponse(true, 200, "", changeitemList));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        //Request For ReOpen Rejected Item Request
        [HttpGet("ReOpenRequest")]
        public async Task<IActionResult> ReOpenRequest(long ItemId)
        {
            try
            {
                var changeitem = (await _ChangeitemRepository.ListAllAsync())
                  .Where(x => x.ItemId == ItemId).FirstOrDefault();
                if (changeitem != null)
                {
                    changeitem.IsApproved = null;
                    await _ChangeitemRepository.UpdateAsync(changeitem);
                }
                else
                {
                    return Ok(new SharedResponse(true, 400, "Unable To find Record!"));
                }
                return Ok(new SharedResponse(true, 200, "Item Reopened Successfully!"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }


    }
}
