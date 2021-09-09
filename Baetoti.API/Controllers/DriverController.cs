using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.API.Helpers;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Enum;
using Baetoti.Shared.Request.Commission;
using Baetoti.Shared.Request.Driver;
using Baetoti.Shared.Request.VAT;
using Baetoti.Shared.Response.Commission;
using Baetoti.Shared.Response.Driver;
using Baetoti.Shared.Response.FileUpload;
using Baetoti.Shared.Response.Invoice;
using Baetoti.Shared.Response.Shared;
using Baetoti.Shared.Response.VAT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baetoti.API.Controllers
{
    public class DriverController : ApiBaseController
    {

        public readonly IDriverRepository _driverRepository;
        public readonly ICommissionRepository _commissionRepository;
        public readonly IVATRepository _vatRepository;
        public readonly IMapper _mapper;

        public DriverController(
            IDriverRepository driverRepository,
            ICommissionRepository commissionRepository,
            IVATRepository vatRepository,
            IMapper mapper
            )
        {
            _driverRepository = driverRepository;
            _commissionRepository = commissionRepository;
            _vatRepository = vatRepository;
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

        [HttpGet("GetByUserID")]
        public async Task<IActionResult> GetByUserID(long Id)
        {
            try
            {
                var driver = await _driverRepository.GetByUserID(Id);
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

        //Commissions
        [HttpGet("GetAllCommissions")]
        public async Task<IActionResult> GetAllCommissions()
        {
            try
            {
                var commissionList = (await _commissionRepository.ListAllAsync()).
                    Where(x => x.MarkAsDeleted == false && x.UserTypeID == (int)UserType.Driver).ToList();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<CommissionResponse>>(commissionList)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpGet("GetCommissionsById")]
        public async Task<IActionResult> GetCommissionsById(int Id)
        {
            try
            {
                var commission = await _commissionRepository.GetByIdAsync(Id);
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<CommissionResponse>(commission)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("AddCommissions")]
        public async Task<IActionResult> AddCommissions([FromBody] CommissionRequest commissionRequest)
        {
            try
            {
                var commission = _mapper.Map<Commissions>(commissionRequest);
                commission.UserTypeID = (int)UserType.Driver;
                commission.MarkAsDeleted = false;
                commission.CreatedAt = DateTime.Now;
                commission.CreatedBy = Convert.ToInt32(UserId);
                var result = await _commissionRepository.AddAsync(commission);
                if (result == null)
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Create Driver Commissions"));
                }
                return Ok(new SharedResponse(true, 200, "Driver Commission Created Succesfully"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        //VAT Tax

        [HttpGet("GetAllVAT")]
        public async Task<IActionResult> GetAllVAT()
        {
            try
            {
                var vatList = (await _vatRepository.ListAllAsync()).
                    Where(x => x.MarkAsDeleted == false && x.UserTypeID == (int)UserType.Driver).ToList();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<VATResponse>>(vatList)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpGet("GetVATById")]
        public async Task<IActionResult> GetVATById(int Id)
        {
            try
            {
                var vat = await _vatRepository.GetByIdAsync(Id);
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<VATResponse>(vat)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("AddVAT")]
        public async Task<IActionResult> AddVAT([FromBody] VATRequest vatRequest)
        {
            try
            {
                var vat = _mapper.Map<VAT>(vatRequest);
                vat.UserTypeID = (int)UserType.Driver;
                vat.MarkAsDeleted = false;
                vat.CreatedAt = DateTime.Now;
                vat.CreatedBy = Convert.ToInt32(UserId);
                var result = await _vatRepository.AddAsync(vat);
                if (result == null)
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Create Driver VAT Tax"));
                }
                return Ok(new SharedResponse(true, 200, "Driver VAT Created Succesfully"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpGet("GetDriverInvoice")]
        public async Task<IActionResult> GetDriverInvoice(long OrderID)
        {
            try
            {
                int UserTypeID = (int)UserType.Buyer;
                var DriverInvoice = await _driverRepository.GetDriverInvoice(OrderID, UserTypeID);
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<InvoiceResponse>(DriverInvoice)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

    }
}
