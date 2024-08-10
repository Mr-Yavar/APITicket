using Entities.DataTransferObjects;
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
    public interface IDiscountService : IService
    {
        Task<PagedData<List<DiscountDto>>> GetAll(DiscountTableRequest dto);
        Task Add(DiscountDto dto);
        Task Update(DiscountDto dto);
        Task<DiscountDto> GetByCode(string Code);
        Task<DiscountDto> GetById(Guid Id);
        Task<DiscountDto> ToggleDiscount(Guid Id);
        Task<Decimal> CalculateDiscount(Decimal Amount, Guid DiscountId, Guid ConcertId, Guid PromoterId, Guid PlaceId,bool checking = true);
    }
}
