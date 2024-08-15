using AutoMapper;
using Contracts.IRepository;
using Entities.DataTransferObjects;
using Entities.Models;
using Features.CustomRequest;
using Features.RequestFeatures;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class UsersService : ServiceBase, IUsersService
    {
        private UserManager<User> _userManager;
        RoleManager<IdentityRole> _roleManager;
        public UsersService(IMapper mapper, IRepositoryManager repositoryManager, UserManager<User> userManager,RoleManager<IdentityRole> roleManager) : base(repositoryManager, mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }


        public async Task<PagedData<List<UserUpdateDto>>> GetUsers(UsersTableRequest dto)
        {

            var query = _userManager.Users.Where(x => (x.FirstName + " " + x.LastName).Contains(dto.FullName ?? ""));


            var models = query.Skip((dto.PageNumber-1)*dto.PageSize).Take(dto.PageSize).Select(userInfo => new UserUpdateDto
            {
                Id = Guid.Parse(userInfo.Id),
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                PhoneNumber = userInfo.PhoneNumber,
                UserName = userInfo.UserName
            }).ToList();


            PagedList<UserUpdateDto> res = new(models, query.Count(), dto.PageNumber, dto.PageSize);



            return new(res.MetaData, res);



        }

        public async Task<UserUpdateDto> GetUser(Guid UserId)
        {

            if (UserId == null || UserId == Guid.Empty)
                throw new ArgumentException(nameof(UserId));




            var userInfo = await _userManager.FindByIdAsync(UserId.ToString());

            var models = new UserUpdateDto
            {
                Id = Guid.Parse(userInfo.Id),
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                PhoneNumber = userInfo.PhoneNumber,
                UserName = userInfo.UserName,
                Role = (await _userManager.GetRolesAsync(userInfo)).First()
            };


  


            return models;



        }
        public async Task<Response> UpdateUser(UserUpdateDto dto)
        {

            if (dto.Id == null || dto.Id == Guid.Empty)
                throw new ArgumentException(nameof(dto.Id));

        


            var userExists = await _userManager.FindByIdAsync(dto.Id.ToString());


            if (userExists == null)
             return new Response { Status = "Error", Message = "User was not found!" };


            userExists.UserName = dto.UserName;
            userExists.FirstName = dto.FirstName;
            userExists.LastName = dto.LastName;
            userExists.PhoneNumber = dto.PhoneNumber;
            var result = await _userManager.UpdateAsync(userExists);

           
            var roles =await _userManager.GetRolesAsync(userExists);
            var role = await _roleManager.FindByNameAsync(dto.Role);
            if (!roles.Contains(role.Name))
            {
                result = await _userManager.RemoveFromRolesAsync(userExists, roles);
                result = await _userManager.AddToRoleAsync(userExists, role.Name);


            }

            if (result.Succeeded && dto.Password != null && dto.Password != "")
            {


                    result = await _userManager.RemovePasswordAsync(userExists);
                    if(result.Succeeded)
                        result = await _userManager.AddPasswordAsync(userExists, dto.Password);
            }


            if (!result.Succeeded)
                return new Response { Status = "Error", Message = "User creation failed! Please check user details and try again.", Errors = result.Errors.Select(x => x.Description).ToList() };

            return new Response { Status = "Success", Message = "User updated successfully!" };


        }




    }
}
