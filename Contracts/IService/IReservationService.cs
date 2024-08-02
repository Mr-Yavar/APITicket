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
        Task<bool> AddTemporaryReservation(Guid UserId, TempReservation dto);

        Task<bool> RemoveTemporaryReservation(Guid UserId, TempReservation dto);

        Task<TempReservationInfo> GetUserReservation(Guid UserId);
    }
}
