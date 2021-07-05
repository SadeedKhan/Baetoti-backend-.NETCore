using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.API.Helpers;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Enum;
using Baetoti.Shared.Request.Provider;
using Baetoti.Shared.Response.FileUpload;
using Baetoti.Shared.Response.Provider;
using Baetoti.Shared.Response.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Baetoti.API.Controllers
{
    public class ProviderController : ApiBaseController
    {

        public readonly IProviderRepository _providerRepository;
        public readonly IMapper _mapper;

        public ProviderController(
            IProviderRepository providerRepository,
            IMapper mapper
            )
        {
            _providerRepository = providerRepository;
            _mapper = mapper;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] ProviderRequest providerRequest)
        {
            try
            {
                var provider = _mapper.Map<Provider>(providerRequest);
                provider.ProviderStatus = (int)ProviderStatus.Pending;
                provider.MarkAsDeleted = false;
                provider.CreatedAt = DateTime.Now;
                provider.CreatedBy = Convert.ToInt32(UserId);
                var result = await _providerRepository.AddAsync(provider);
                if (result == null)
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Submit Provider Request"));
                }
                return Ok(new SharedResponse(true, 200, "Provider Request Submitted Successfully"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpGet("GetByUserID")]
        public async Task<IActionResult> GetByUserID(long Id)
        {
            try
            {
                var provider = await _providerRepository.GetByUserID(Id);
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<ProviderResponse>(provider)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("Approval")]
        public async Task<IActionResult> Approval([FromBody] ProviderApprovalRequest providerRequest)
        {
            try
            {
                var provider = await _providerRepository.GetByUserID(providerRequest.UserID);
                provider.ProviderStatus = providerRequest.IsApproved == true ? (int)ProviderStatus.Approved : (int)ProviderStatus.Rejected;
                provider.Comments = providerRequest.Comments;
                provider.LastUpdatedAt = DateTime.Now;
                provider.UpdatedBy = Convert.ToInt32(UserId);
                await _providerRepository.UpdateAsync(provider);
                return Ok(new SharedResponse(true, 200, "Provider Request Processed Successfully"));
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
                    FileUploadResponse _RESPONSE = await obj.UploadImageFile(file, "Provider");
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
