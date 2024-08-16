using Entities.DataTransferObjects;
using Entities.DataTransferObjects.AdminDto;
using Entities.DataTransferObjects.PayingDto;
using Entities.Enum;
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
    public interface ITransactionService : IService
    {
        Task<string> CreatePayment(BillingDto dto);
        Task<PayStatusType> VerifyPayment(toVerifyDto dto,Transactions transaction);
        Task CancelTransactionsByTempReservation(Guid TempReservationId);
        Task<PagedData<List<TransactionInfoDto>>> GetUserTransactions(Guid UserId, UserTransactionTableRequest dto);

        Task<DashboardCardItemDto> GetWeekInCome(DateTime date);
        Task<DashboardCardItemDto> GetDayInCome(DateTime date);
    }
}
