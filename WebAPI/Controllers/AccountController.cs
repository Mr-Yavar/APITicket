using Asp.Versioning;
using AutoMapper;

using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(IMapper mapper,
                                 IHttpContextAccessor httpContextAccessor,
                                 UserManager<User> userManager,
                                 RoleManager<IdentityRole> roleManager) : base()
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        #region Users
  

        /// <summary>
        /// گرفتن همه کابران
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = _userManager.Users.Select(x =>new
            UserDto
            {
                Id = Guid.Parse(x.Id),
                PhoneNumber = x.PhoneNumber,
                FirstName = x.FirstName,
                LastName = x.LastName,
                UserName = x.UserName,
      
            }).ToList();
            return Ok(result);


        }

        /// <summary>
        /// بروزرسانی کاربران
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto model)
        {

            var userExists = await _userManager.FindByIdAsync(model.Id.ToString());


            if (userExists == null)
                return NotFound(new Response { Status = "Error", Message = "User was not found!" });

            userExists.PhoneNumber = model.PhoneNumber;

            userExists.UserName = model.UserName;
            userExists.FirstName = model.FirstName;
            userExists.LastName = model.LastName;
            userExists.Gender = model.Gender;
            var result = await _userManager.UpdateAsync(userExists);

            await _userManager.RemoveFromRoleAsync(userExists, (await _userManager.GetRolesAsync(userExists)).First());
            await _userManager.AddToRoleAsync(userExists,model.Role);
         
            if (result.Succeeded && model.Password != null && model.Password != "")
            {

                await _userManager.RemovePasswordAsync(userExists);
                await _userManager.AddPasswordAsync(userExists, model.Password);
            }


            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again.", Errors = result.Errors.Select(x => x.Description).ToList() });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        #endregion


        #region Roles
        /// <summary>
        /// ایجاد نقش جدید
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateRole([FromBody] RoleDto dto)
        {
            //if (ModelState.IsValid)
            //{
            IdentityRole Role = new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
               // Description = dto.Description,
                //Locked = false
            };
            IdentityResult result = await _roleManager.CreateAsync(Role);
            if (result.Errors.Count() > 0)
                return Ok(new
                {
                    success = false,
                    message = result.Errors.First().Description,
                });

            //foreach (IdentityError error in result.Errors)
            //    ModelState.AddModelError("", error.Description);
            //};

            return Ok(new
            {
                success = true,
                message = "Role created successfully.",
            });
        }
        /// <summary>
        /// گرفتن همه نقش های
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> ListRoles()
        {
            return Ok(_roleManager.Roles.Select(x => new RoleDto() { Id = Guid.Parse(x.Id), Name = x.Name,
                
                //Description = x.Description, Locked = x.Locked 
            
            }).ToList());
        }
        /// <summary>
        /// گرفتن اطلاعات یک نقش برای ویرایش کاربران و سطح دسترسی ها
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       /* [HttpGet("[action]")]
        public async Task<IActionResult> GetRole(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return NotFound("نقش مورد نظر یافت نشد");
            }

            List<User> RoleUsers = (await _userManager.GetUsersInRoleAsync(role.Name)).ToList<User>();
            List<User> Users = _userManager.Users.ToList();

            var res = new RoleDto() { Id = id, Name = role.Name, Users = Users.Select(x => new UserRoleDto() { Id = Guid.Parse(x.Id), UserName = x.UserName, FirstName = x.FirstName, LastName = x.LastName, Selected = RoleUsers.Any(user => user.Id == x.Id) }).ToList() };


            List<RoleClaimDto> Permissions = typeof(SMPermissions).GetFields(BindingFlags.Static | BindingFlags.Public).Select(x => new RoleClaimDto() { Type = ServiceClaimTypes.SM_Permission, Value = x.GetValue(null).ToString(), Selected = false }).ToList();
            Permissions.AddRange(typeof(PMSPermission).GetFields(BindingFlags.Static | BindingFlags.Public).Select(x => new RoleClaimDto() { Type = ServiceClaimTypes.PMS_Permission, Value = x.GetValue(null).ToString(), Selected = false }).ToList());

            (await _roleManager.GetClaimsAsync(role)).ToList().ForEach(claim =>
            {
                var Permission = Permissions.FirstOrDefault(x => x.Type == claim.Type && x.Value == claim.Value);
                Permission.Selected = true;
            });

            res.Claims = Permissions;
            return Ok(res);
        }*/

        /// <summary>
        /// بروزرسانی نقش و کاربران و سطح دسترسی های آن
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
     /*   [HttpPatch("[action]")]
        public async Task<IActionResult> UpdateRole([FromBody] RoleDto dto)
        {
            var role = await _roleManager.FindByIdAsync(dto.Id.ToString());
            if (role == null)
            {
                return NotFound("نقش مورد نظر یافت نشد");
            }


            var AllRoleClaims = await _roleManager.GetClaimsAsync(role);

            for (int i = 0; i < dto.Claims.Count; i++)
            {
                var claim = dto.Claims[i];
                if (AllRoleClaims.Any(x => x.Type == claim.Type && x.Value == claim.Value))
                {
                    if (!claim.Selected)
                        await _roleManager.RemoveClaimAsync(role, AllRoleClaims.FirstOrDefault(x => x.Type == claim.Type && x.Value == claim.Value));

                }
                else
                {
                    if (claim.Selected)
                        await _roleManager.AddClaimAsync(role, new System.Security.Claims.Claim(claim.Type, claim.Value));
                }
            }










            for (int i = 0; i < dto.Users.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(dto.Users[i].Id.ToString());
                IdentityResult result = null;
                if (dto.Users[i].Selected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    // اضافه کردن رول به یک یوزر
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!dto.Users[i].Selected && await _userManager.IsInRoleAsync(user, role.Name))
                {

                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);

                }
                else
                    continue;


            }
            if (!role.Locked)
            {
                role.Name = dto.Name;
                role.Description = dto.Description;
            }
            await _roleManager.UpdateAsync(role);

            return Ok(new Response() { Message = "has updated!" });
        }*/

        //[HttpDelete("[action]")]
        //public async Task<IActionResult> RemoveRole(Guid RoleId)
        //{
        //    var role = await _roleManager.FindByIdAsync(RoleId.ToString());
        //    if (role == null)
        //    {
        //        return Ok(new
        //        {
        //            success = false,
        //            message = $"Role cannot be found!",
        //        });
        //    }

        //    var result = await _roleManager.DeleteAsync(role);
        //    if (result.Errors.Count() > 0)
        //        return Ok(new
        //        {
        //            success = false,
        //            message = result.Errors.First().Description,
        //        });

        //    return Ok(new
        //    {
        //        success = true,
        //        message = "Role deleted successfully.",
        //    });
        //}







        #endregion
    }
}
