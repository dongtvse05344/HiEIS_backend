using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiEIS.Model;
using HiEIS.Service;
using HiEIS_Core.Paging;
using HiEIS_Core.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HiEIS_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<MyUser> _userManager;
        private readonly IMyUserService _userService;
        private readonly IStaffService _staffService;

        public AccountController(UserManager<MyUser> userManager, IMyUserService userService, IStaffService staffService)
        {
            _userManager = userManager;
            _userService = userService;
            _staffService = staffService;
        }

        [HttpGet("Admin")]
        public ActionResult GetAdmins(int index = 1, int pageSize = 5)
        {
            var list = _userService.GetMyUsers(_ => _.Name.Contains("admin"));
            var result = list.ToPageList<AccountVM, MyUser>(index, pageSize);
            return Ok(result);
        }

        [HttpGet("Company/{companyId}/Staffs")]
        public ActionResult GetStaff(Guid companyId , int index = 1, int pageSize = 5 )
        {
            var list = _staffService.GetStaffs(_ => _.CompanyId.Equals(companyId)); 
            var result  = list.ToPageList<StaffVM, Staff>(index, pageSize);
            foreach (var item in result.List)
            {
                item.Roles = _userManager.GetRolesAsync(
                            _userManager.FindByIdAsync(item.Id).Result
                            ).Result.ToArray();
            }
            return Ok(result);
        }

        [HttpGet("Staffs/{staffId}")]
        public ActionResult GetStaffById(string staffId)
        {
            var staff = _staffService.GetStaff(staffId);
            var result = staff.Adapt<StaffVM>();
            result.Roles = _userManager.GetRolesAsync(
                            staff.MyUser
                            ).Result.ToArray();
            return Ok(result);
        }

        [HttpPost("Staff")]
        public async Task<ActionResult> CreateStaff(StaffCM model)
        {
            MyUser user = null;
            try
            {
                user = new MyUser { UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    IsActive = true
                };
                var createUserResult = await this._userManager.CreateAsync(user, model.Password);
                if (createUserResult.Succeeded)
                {
                    foreach (var role in model.Roles)
                    {
                        await this._userManager.AddToRoleAsync(user, role);
                    }
                    var staff = model.Adapt<Staff>();
                    staff.Id = user.Id;
                    _staffService.CreateStaff(staff);
                    _staffService.SaveChanges();
                    return StatusCode(201);
                }
                else
                {
                    return BadRequest("Dữ liệu không phù hợp");
                }

            }
            catch (Exception e)
            {
                if (user != null) _userManager.DeleteAsync(user);
                return BadRequest(e.Message);
            }
        }
        [HttpPut("Staff")]
        public async Task< ActionResult> UpdateStaff(StaffUM model)
        {
            try
            {
                var staff = _staffService.GetStaff(model.Id);
                if (staff == null) return NotFound();
                staff = model.Adapt(staff);
                staff.MyUser.PhoneNumber = model.PhoneNumber;
                staff.MyUser.Email = model.Email;

                _staffService.SaveChanges();
                if (model.Roles.Count > 0)
                {
                    var user = _userManager.FindByIdAsync(model.Id).Result;
                    var roles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                    foreach (var role in model.Roles)
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    }

}