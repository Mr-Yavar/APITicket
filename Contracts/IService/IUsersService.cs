using Contracts.IService;
using Entities.DataTransferObjects;
using Features.CustomRequest;
using Features.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IUsersService : IService
    {
        Task<PagedData<List<UserUpdateDto>>> GetUsers(UsersTableRequest dto);
        Task<Response> UpdateUser(UserUpdateDto dto);
        Task<UserUpdateDto> GetUser(Guid UserId);
    }
}
