using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.API.Helpers;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Enum;
using Baetoti.Shared.Request.Driver;
using Baetoti.Shared.Response.Driver;
using Baetoti.Shared.Response.FileUpload;
using Baetoti.Shared.Response.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Baetoti.API.Controllers
{
    public class DriverController : ApiBaseController
    {

        public readonly IDriverRepository _driverRepository;
        public readonly IMapper _mapper;

        public DriverController(
            IDriverRepository driverRepository,
            IMapper mapper
            )
        {
            _driverRepository = driverRepository;
            _mapper = mapper;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] DriverRequest driverRequest)
        {
            try
            {
                var driver = _mapper.Map<Driver>(driverRequest);
                driver.DriverStatus = (int)DriverStatus.Pending;
                driver.MarkAsDeleted = false;
                driver.CreatedAt = DateTime.Now;
                driver.CreatedBy = Convert.ToInt32(UserId);
                var result = await _driverRepository.AddAsync(driver);
                if (result == null)
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Submit Driver Request"));
                }
                return Ok(new SharedResponse(true, 200, "Driver Request Submitted Successfully"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpPost("GetByUserID")]
        public async Task<IActionResult> GetByUserID([FromBody] DriverGetByUserIDRequest request)
        {
            try
            {
                var driver = await _driverRepository.GetByUserID(request.UserID);
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<DriverResponse>(driver)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("Approval")]
        public async Task<IActionResult> Approval([FromBody] DriverApprovalRequest driverRequest)
        {
            try
            {
                var driver = await _driverRepository.GetByUserID(driverRequest.UserID);
                driver.DriverStatus = driverRequest.IsApproved == true ? (int)DriverStatus.Approved : (int)DriverStatus.Rejected;
                driver.Comments = driverRequest.Comments;
                driver.LastUpdatedAt = DateTime.Now;
                driver.UpdatedBy = Convert.ToInt32(UserId);
                await _driverRepository.UpdateAsync(driver);
                return Ok(new SharedResponse(true, 200, "Driver Request Processed Successfully"));
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
                    FileUploadResponse _RESPONSE = await obj.UploadImageFile(file, "Driver");
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
