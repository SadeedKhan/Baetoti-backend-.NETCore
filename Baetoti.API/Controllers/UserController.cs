using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Core.Interface.Services;
using Baetoti.Shared.Request.Delete;
using Baetoti.Shared.Request.User;
using Baetoti.Shared.Request.UserRole;
using Baetoti.Shared.Response.Shared;
using Baetoti.Shared.Response.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Baetoti.API.Controllers
{
    public class UserController : ApiBaseController
    {
        public readonly IUserRepository _userRepository;
        public readonly IUserRoleRepository _userroleRepository;
        private readonly IArgon2Service _hashingService;
        public readonly IMapper _mapper;

        public UserController(
         IUserRepository userRepository,
         IUserRoleRepository userroleRepository,
         IArgon2Service hashingService,
         IMapper mapper
         )
        {
            _userRepository = userRepository;
            _userroleRepository = userroleRepository;
            _hashingService = hashingService;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userList = (await _userRepository.ListAllAsync()).ToList();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<UserResponse>>(userList)));
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
                var user = await _userRepository.GetByIdAsync(Id);
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<UserResponse>(user)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] UserRequest userRequest)
        {
            try
            {

                var user = _mapper.Map<User>(userRequest);
                user.Password = _hashingService.GenerateHash(userRequest.Password);
                user.CreatedAt = DateTime.Now;
                user.CreatedBy = Convert.ToInt32(UserId);
                user.UserStatus = 1; //Change it after Email or mobile verifications
                var result = await _userRepository.AddAsync(user);
                if (result == null)
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Create User"));
                }
                else
                {
                    AssignRoleRequest assignRole = new AssignRoleRequest();
                    assignRole.RoleId = userRequest.RoleID;
                    assignRole.UserId = Convert.ToInt32(result.ID);
                    var role = _mapper.Map<UserRoles>(assignRole);
                    role.CreatedAt = DateTime.Now;
                    role.CreatedBy = Convert.ToInt32(UserId);
                    var res = await _userroleRepository.AddAsync(role);
                }
                return Ok(new SharedResponse(true, 200, "User Created Succesfully"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] UserRequest userRequest)
        {
            try
            {
                var cat = await _userRepository.GetByIdAsync(userRequest.ID);
                if (cat != null)
                {
                    var user = _mapper.Map<User>(userRequest);
                    user.LastUpdatedAt = DateTime.Now;
                    user.LastUpdatedBy = Convert.ToInt32(UserId);
                     await _userRepository.UpdateAsync(user);
                    return Ok(new SharedResponse(true, 200, "User Updated Succesfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Find User"));
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
                var un = await _userRepository.GetByIdAsync(deleteRequest.ID);
                if (un != null)
                {
                    await _userRepository.DeleteAsync(un);
                    return Ok(new SharedResponse(true, 200, "User Deleted Succesfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Find User"));
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
                    if (CheckIfOnlyImageFile(file))
                    {
                        string fileName = null;
                        var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                        fileName = DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.
                        var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads\\Employee");
                        if (!Directory.Exists(pathBuilt))
                        {
                            Directory.CreateDirectory(pathBuilt);
                        }
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads\\Employee", fileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        return Ok(new SharedResponse(true, 200, "File uploaded successfully!", "Uploads/Employee", fileName, path));
                    }
                    else
                    {
                        return Ok(new SharedResponse(false, 400, "File format is incorrect! (only .png,.jpg,.jpeg) is Supported"));
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

        //Get Image File Extention
        private bool CheckIfOnlyImageFile(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return (extension.ToUpper() == ".PNG" || extension.ToUpper() == ".JPG" || extension.ToUpper() == ".JPEG"); // Change the extension based on your need
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
