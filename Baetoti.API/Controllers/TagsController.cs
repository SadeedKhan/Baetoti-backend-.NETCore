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
    public class TagsController : ApiBaseController
    {
        public readonly ITagsRepository tagsRepository;
        public readonly IMapper _mapper;

        [HttpGet("GetAllTags")]
        public async Task<IActionResult> GetAllTags()
        {
            try
            {
                var tagslist = (await tagsRepository.ListAllAsync()).ToList();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<Tags>>(tagslist)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("AddTags")]
        public async Task<IActionResult> AddTags([FromBody] Tags tags)
        {
            try
            {
                var result = await tagsRepository.AddAsync(tags);
                if (result == null)
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Create Tags"));
                }
                return Ok(new SharedResponse(true, 200, "Tags Created Succesfully"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpPost("UpdateTags")]
        public async Task<IActionResult> UpdateTags([FromBody] Tags tags)
        {
            try
            {
                var cat = "";
                //var cat = await tagsRepository.GetByIdAsync(tags.ID); Need Confirmation
                if (cat != null)
                {
                    tagsRepository.UpdateAsync(tags);
                    return Ok(new SharedResponse(true, 200, "Tags Created Succesfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Find Tags"));
                }
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpPost("DeleteTags")]
        public async Task<IActionResult> DeleteTags([FromBody] Tags tags)
        {
            try
            {
                var cat = "";
                //var cat = await tagsRepository.GetByIdAsync(tags.ID); Need Confirmation
                if (cat != null)
                {
                    tagsRepository.DeleteAsync(tags);
                    return Ok(new SharedResponse(true, 200, "Tags Deleted Succesfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Find Tags"));
                }
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }
    }
}
