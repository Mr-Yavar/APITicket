using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.Enum;
using Entities.Models;
using Features.CustomRequest;
using Features.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services.Services
{
    public class DiscountService : ServiceBase, IDiscountService
    {

        public DiscountService(IMapper mapper, IRepositoryManager repositoryManager) : base(repositoryManager, mapper)
        {



        }

        public async Task<PagedData<List<DiscountDto>>> GetAll(DiscountTableRequest dto)
        {
            var query = await _repositoryManager.IDiscountRepository.GetPagedWithBy<DateTime>(x => true, x => x.CreatedDate, false, dto);
            var data = await query.ToListAsync();

            PagedList<DiscountDto> result = new(_mapper.Map<List<DiscountDto>>(data), _repositoryManager.IDiscountRepository.FindAll(false).Count(), dto.PageNumber, dto.PageSize);

            return new PagedData<List<DiscountDto>>(result.MetaData, result.ToList());
        }

        public async Task Add(DiscountDto dto)
        {
            dto.CreatedDate = DateTime.Now;

            Discount model = _mapper.Map<Discount>(dto);
            _repositoryManager.IDiscountRepository.Create(model);
            _repositoryManager.Save();

        }

        public async Task Update(DiscountDto dto)
        {

            var oldModel = await _repositoryManager.IDiscountRepository.FindByCondition(x => x.Id == dto.Id, true).FirstOrDefaultAsync();

            oldModel.Title = dto.Title;
            oldModel.IsActive = dto.IsActive;
            oldModel.MaxDiscount = dto.MaxDiscount;
            oldModel.ObjectId = dto.ObjectId;
            oldModel.DiscountType = dto.DiscountType;
            oldModel.Percentage = dto.Percentage;
            oldModel.Code = dto.Code;
            oldModel.ExpirationDate = dto.ExpirationDate;
            oldModel.LimitOfUses = dto.LimitOfUses;

            _repositoryManager.IDiscountRepository.Update(oldModel);
            _repositoryManager.Save();

        }

        public async Task<DiscountDto> GetByCode(Guid tempReservationId, string Code)
        {

            var model = await _repositoryManager.IDiscountRepository.FindByCondition(x => x.Code.ToLower() == Code.ToLower() && x.IsActive && x.ExpirationDate >= DateTime.Now, false).FirstOrDefaultAsync();

            if (model==null )
                return null;



            var res = _mapper.Map<DiscountDto>(model);

            if (!(await ValidationOfUse(res, tempReservationId)))
                return null;

            return res;


        }

        public async Task<DiscountDto> GetById(Guid Id)
        {

            var model = await _repositoryManager.IDiscountRepository.FindByCondition(x => x.Id == Id, false).FirstOrDefaultAsync();
            var res = _mapper.Map<DiscountDto>(model);
            return res;


        }

        public async Task<DiscountDto> ToggleDiscount(Guid Id)
        {

            var oldModel = await _repositoryManager.IDiscountRepository.FindByCondition(x => x.Id == Id, true).FirstOrDefaultAsync();


            oldModel.IsActive = !oldModel.IsActive;

            _repositoryManager.IDiscountRepository.Update(oldModel);
            _repositoryManager.Save();
            var res = _mapper.Map<DiscountDto>(oldModel);
            return res;
        }


        public async Task<Decimal?> CalculateDiscount(Decimal Amount, Guid DiscountId, Guid TempReservationID, bool checking = true)
        {

            

            var discount = await this.GetById(DiscountId);
            if (discount == null)
                return Amount;




            if (!checking || (discount.IsActive == true && discount.ExpirationDate >= DateTime.Now))
            {

                if (!(await ValidationOfUse(discount, TempReservationID)))
                    return null;

                Decimal res = Amount;
                if (discount.MaxDiscount.HasValue && !discount.Percentage.HasValue)
                {
                    res = Amount - discount.MaxDiscount.Value;

                    res = res < 0 ? 0 : res;

                }
                else if (!discount.MaxDiscount.HasValue && discount.Percentage.HasValue)
                {
                    res = Amount * discount.Percentage.Value / 100;
                    res = Amount - res;
                    res = res < 0 ? 0 : res;
                }
                else if (discount.MaxDiscount.HasValue && discount.Percentage.HasValue)
                {
                    res = Amount * discount.Percentage.Value / 100;
                    res = res < discount.MaxDiscount.Value ? res : discount.MaxDiscount.Value;
                    res = Amount - res;
                    res = res < 0 ? 0 : res;
                }
                return res;
            }


            return Amount;

        }


        public async Task<bool> ValidationOfUse(DiscountDto dscnt, Guid TempReservationId)
        {

            if (dscnt.LimitOfUses.HasValue)
            {
                if (dscnt.LimitOfUses.Value <= 0)
                    return false;

                int compeletedCount = _repositoryManager.IReservationRepository.FindByCondition(x => x.DiscountId == dscnt.Id, false).Count();

                int uncompeletedCount = _repositoryManager.ITempReservationRepository.FindByCondition(x => x.DiscountId == dscnt.Id && x.Status != ReservationStatusType.Cancelled && x.ExpirationDate > DateTime.Now, false).Count();

                if (compeletedCount + uncompeletedCount >= dscnt.LimitOfUses.Value)
                    return false;

            }

            switch (dscnt.DiscountType)
            {
                case DiscountType.NOCONDITION:
                    return true;
                case DiscountType.City:
                    {

                        if (!dscnt.ObjectId.HasValue)
                            return false;

                        var tempReservation = await _repositoryManager.ITempReservationRepository.GetByWithIncludes(x => x.Id == TempReservationId, x => x.Tickets).ThenInclude(x => x.Event.Hall.Place).FirstOrDefaultAsync();
                        if (tempReservation == null)
                            return false;

                        var ticket = tempReservation.Tickets.ElementAtOrDefault(0);
                        if (ticket == null)
                            return false;

                        return ticket.Event.Hall.Place.CityId == dscnt.ObjectId.Value;
                    }
                    break;
                case DiscountType.Concert:
                    {

                        if (!dscnt.ObjectId.HasValue)
                            return false;

                        var tempReservation = await _repositoryManager.ITempReservationRepository.GetByWithIncludes(x => x.Id == TempReservationId, x => x.Tickets).ThenInclude(x => x.Event.Concert).FirstOrDefaultAsync();
                        if (tempReservation == null)
                            return false;

                        var ticket = tempReservation.Tickets.ElementAtOrDefault(0);
                        if (ticket == null)
                            return false;

                        return ticket.Event.Concert.Id == dscnt.ObjectId.Value;
                    }

                case DiscountType.Promoter:
                    {

                        if (!dscnt.ObjectId.HasValue)
                            return false;

                        var tempReservation = await _repositoryManager.ITempReservationRepository.GetByWithIncludes(x => x.Id == TempReservationId, x => x.Tickets).ThenInclude(x => x.Event.Concert).FirstOrDefaultAsync();
                        if (tempReservation == null)
                            return false;

                        var ticket = tempReservation.Tickets.ElementAtOrDefault(0);
                        if (ticket == null)
                            return false;

                        return ticket.Event.Concert.PromoterId == dscnt.ObjectId.Value;
                    }
                case DiscountType.Place:
                    {

                        if (!dscnt.ObjectId.HasValue)
                            return false;

                        var tempReservation = await _repositoryManager.ITempReservationRepository.GetByWithIncludes(x => x.Id == TempReservationId, x => x.Tickets).ThenInclude(x => x.Event.Concert).FirstOrDefaultAsync();
                        if (tempReservation == null)
                            return false;

                        var ticket = tempReservation.Tickets.ElementAtOrDefault(0);
                        if (ticket == null)
                            return false;

                        return ticket.Event.Concert.PlaceId == dscnt.ObjectId.Value;
                    }
                case DiscountType.INVALIAD:
                    return false;

            }

            return true;
        }

    }
}
