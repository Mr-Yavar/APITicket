using Entities.DataTransferObjects;
using Entities.DataTransferObjects.PayingDto;
using Entities.Enum;
using Entities.Models;
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
        Task<PayStatusType> VerifyPayment(toVerifyDto dto);
        Task CancelTransactionsByTempReservation(Guid TempReservationId);
    }
}
