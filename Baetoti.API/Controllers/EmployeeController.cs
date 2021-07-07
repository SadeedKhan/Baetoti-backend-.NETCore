using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.API.Helpers;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Core.Interface.Services;
using Baetoti.Shared.Request.Delete;
using Baetoti.Shared.Request.Employee;
using Baetoti.Shared.Response.FileUpload;
using Baetoti.Shared.Response.Shared;
using Baetoti.Shared.Response.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Baetoti.Shared.Request.EmployeeRole;

namespace Baetoti.API.Controllers
{
    public class EmployeeController : ApiBaseController
    {
        public readonly IEmployeeRepository _employeeRepository;
        public readonly IEmployeeRoleRepository _employeeroleRepository;
        private readonly IArgon2Service _hashingService;
        public readonly IMapper _mapper;

        public EmployeeController(
         IEmployeeRepository employeeRepository,
         IEmployeeRoleRepository employeeroleRepository,
         IArgon2Service hashingService,
         IMapper mapper
         )
        {
            _employeeRepository = employeeRepository;
            _employeeroleRepository = employeeroleRepository;
            _hashingService = hashingService;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var employeeList = (await _employeeRepository.GetAll(0)).ToList();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<EmployeeResponse>>(employeeList)));
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
                var employee = (await _employeeRepository.GetAll(Id)).FirstOrDefault();
                if (employee != null)
                {
                    return Ok(new SharedResponse(true, 200, "", _mapper.Map<EmployeeResponse>(employee)));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Find Employee"));
                }
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] EmployeeRequest employeeRequest)
        {
            try
            {
                var employee = _mapper.Map<Employee>(employeeRequest);
                employee.Password = _hashingService.GenerateHash(employeeRequest.Password);
                employee.MarkAsDeleted = false;
                employee.CreatedAt = DateTime.Now;
                employee.CreatedBy = Convert.ToInt32(UserId);
                employee.EmployeeStatus = 1; //Change it after Email or mobile verifications
                var result = await _employeeRepository.AddAsync(employee);
                if (result == null)
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Create Employee"));
                }
                else
                {
                    AssignRoleRequest assignRole = new AssignRoleRequest();
                    assignRole.RoleId = employeeRequest.RoleID;
                    assignRole.EmployeeId = Convert.ToInt32(result.ID);
                    var role = _mapper.Map<EmployeeRole>(assignRole);
                    role.MarkAsDeleted = false;
                    role.CreatedAt = DateTime.Now;
                    role.CreatedBy = Convert.ToInt32(UserId);
                    var res = await _employeeroleRepository.AddAsync(role);
                }
                return Ok(new SharedResponse(true, 200, "Employee Created Succesfully"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] EmployeeRequest employeeRequest)
        {
            try
            {
                var cat = await _employeeRepository.GetByIdAsync(employeeRequest.ID);
                if (cat != null)
                {
                    var employee = _mapper.Map<Employee>(employeeRequest);
                    employee.LastUpdatedAt = DateTime.Now;
                    employee.LastUpdatedBy = Convert.ToInt32(UserId);
                     await _employeeRepository.UpdateAsync(employee);
                    return Ok(new SharedResponse(true, 200, "Employee Updated Succesfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Find Employee"));
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
                var un = await _employeeRepository.GetByIdAsync(deleteRequest.ID);
                if (un != null)
                {
                    un.MarkAsDeleted = true;
                    un.LastUpdatedAt = DateTime.Now;
                    un.LastUpdatedBy = Convert.ToInt32(UserId);
                    await _employeeRepository.DeleteAsync(un);
                    return Ok(new SharedResponse(true, 200, "Employee Deleted Succesfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Find Employee"));
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
                    FileUploadResponse _RESPONSE = await obj.UploadImageFile(file, "Employee");
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

        //Save Sase64 Image In Directory
        //private FileUploadOutput WriteBase64ImageFile(byte[] bytess, string extension)
        //{
        //    FileUploadOutput _result = new FileUploadOutput();
        //    bool WriteFileWithRoot = true;
        //    string fileName = null;
        //    try
        //    {
        //        if (WriteFileWithRoot == true)
        //        {
        //            fileName = DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.
        //            var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads\\Visitors");
        //            if (!Directory.Exists(pathBuilt))
        //            {
        //                Directory.CreateDirectory(pathBuilt);
        //            }
        //            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads\\Visitors", fileName);
        //            using (var imageFile = new FileStream(path, FileMode.Create))
        //            {
        //                imageFile.Write(bytess, 0, bytess.Length);
        //                imageFile.Flush();
        //            }
        //        }
        //        _result.Message = "File uploaded successfully!";
        //        _result.FileName = fileName;
        //        _result.Path = "Uploads/Visitors";
        //        _result.PathwithFileName = "Uploads/Visitors/" + fileName;
        //        _result.StatusCode = "200";
        //    }
        //    catch (Exception e)
        //    {
        //        _result.Message = "failed to Upload File!";
        //        _result.StatusCode = "500";
        //        //isSaveSuccess = false;
        //    }
        //    return _result;
        //}

        //private string GetExtenstionFromBase64(string base64String)
        //{
        //    String[] strings = base64String.Split(",");
        //    string extension;
        //    switch (strings[0])
        //    {
        //        //check image's extension
        //        case "data:image/jpg;base64":
        //            extension = ".jpg";
        //            break;
        //        case "data:image/png;base64":
        //            extension = ".png";
        //            break;
        //        default:
        //            //should write cases for more images types
        //            extension = ".jpeg";
        //            break;
        //    }
        //    return extension;
        //}
    }
}
