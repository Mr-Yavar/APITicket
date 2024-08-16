using Entities.DataTransferObjects;
using Entities.DataTransferObjects.AdminDto;
using Entities.DataTransferObjects.PayingDto;
using Entities.Models;
using Features.CustomRequest;
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
        Task<DashboardCardItemDto> GetDailyReservedTicket(DateTime date);
        Task<Guid?> AddTemporaryReservation(Guid UserId, TempReservationDto dto);

        Task<bool> RemoveTemporaryReservation(Guid UserId);

        Task<TempReservationInfo> GetUserReservation(Guid UserId);
        Task<toVerifyResponse> VerifyPaidBilling(Guid UserId, toVerifyDto dto);
        Task<toPayResponse> ReservationToPay(Guid UserId, toPayDto dto);
        Task<ReservationResultDto> GetUserReservationDetailsById(Guid UserId, Guid ReservationId);

        /// <summary>
        /// گرفتن صفحه بندی شده رزروهای یوزر
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ReservationId"></param>
        /// <returns></returns>
        Task<PagedData<List<ReservationResultDto>>> GetUserReservations(Guid UserId, UserReservationTableRequest dto);
        Task<PagedData<List<ReservationResultDto>>> GetConcertReservations(Guid ConcertId, UserReservationTableRequest dto);

    }
}
