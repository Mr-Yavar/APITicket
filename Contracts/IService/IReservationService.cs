using Entities.DataTransferObjects;
using Entities.Models;
using Features.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.IService
{
    public interface IReservationService : IService
    {
        Task<Guid> AddTemporaryReservation(Guid UserId, TempReservation dto);

        Task<bool> RemoveTemporaryReservation(Guid UserId, Guid TempId);

        Task<TempReservationInfo> GetUserReservation(Guid UserId);
    }
}
