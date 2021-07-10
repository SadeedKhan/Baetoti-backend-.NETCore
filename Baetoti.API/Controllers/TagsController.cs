using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Request.Delete;
using Baetoti.Shared.Request.TagRequest;
using Baetoti.Shared.Response.Shared;
using Baetoti.Shared.Response.TagResponse;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baetoti.API.Controllers
{
    public class TagsController : ApiBaseController
    {
        public readonly ITagsRepository _tagsRepository;
        public readonly IMapper _mapper;

        public TagsController(
           ITagsRepository tagsRepository,
           IMapper mapper
           )
        {
            _tagsRepository = tagsRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var tagList = (await _tagsRepository.ListAllAsync())
                    .Where(x => x.MarkAsDeleted == false).ToList();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<TagResponse>>(tagList)));
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
                var tag = await _tagsRepository.GetByIdAsync(Id);
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<TagResponse>(tag)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] TagRequest tagRequest)
        {
            try
            {
                var tag = _mapper.Map<Tags>(tagRequest);
                tag.MarkAsDeleted = false;
                tag.CreatedAt = DateTime.Now;
                tag.CreatedBy = Convert.ToInt32(UserId);
                var result = await _tagsRepository.AddAsync(tag);
                if (result == null)
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Create Tag"));
                }
                return Ok(new SharedResponse(true, 200, "Tag Created Successfully"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] TagRequest tagRequest)
        {
            try
            {
                var cat = await _tagsRepository.GetByIdAsync(tagRequest.ID);
                if (cat != null)
                {

                    var tag = _mapper.Map<Tags>(tagRequest);
                    tag.LastUpdatedAt = DateTime.Now;
                    tag.UpdatedBy = Convert.ToInt32(UserId);
                    await _tagsRepository.UpdateAsync(tag);
                    return Ok(new SharedResponse(true, 200, "Tag Updated Successfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Find Tag"));
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
                var tag = await _tagsRepository.GetByIdAsync(deleteRequest.ID);
                if (tag != null)
                {
                    tag.MarkAsDeleted = true;
                    tag.CreatedAt = DateTime.Now;
                    tag.CreatedBy = Convert.ToInt32(UserId);
                    await _tagsRepository.DeleteAsync(tag);
                    return Ok(new SharedResponse(true, 200, "Tag Deleted Successfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Find Tag"));
                }
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }
    }
}
