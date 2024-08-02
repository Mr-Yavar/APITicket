using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class DiscountService : ServiceBase, IDiscountService
    {


   

        public DiscountService(IMapper mapper,IRepositoryManager repositoryManager):base( repositoryManager,mapper) {
        
      
         
        }

        public async Task Add(DiscountDto dto)
        {
            Discount model = _mapper.Map<Discount>(dto);
            _repositoryManager.IDiscountRepository.Create(model);
            _repositoryManager.Save();

        }

        public async Task Update(DiscountDto dto)
        {
 
            var oldModel =  await _repositoryManager.IDiscountRepository.FindByCondition(x => x.Id == dto.Id, true).FirstOrDefaultAsync();

            oldModel.Title = dto.Title;
            oldModel.IsActive = dto.IsActive;
            oldModel.MaxDiscount = dto.MaxDiscount;
            oldModel.ObjectId = dto.ObjectId;
            oldModel.DiscountType = dto.DiscountType;
            oldModel.Percentage = dto.Percentage;
            oldModel.Code = dto.Code;
            _repositoryManager.IDiscountRepository.Update(oldModel);
            _repositoryManager.Save();

        }

        public async Task<DiscountDto> GetByCode(string Code)
        {

            var model = await _repositoryManager.IDiscountRepository.FindByCondition(x=>x.Code.ToLower()==Code.ToLower() && x.IsActive && x.ExpirationDate >= DateTime.Now,false).FirstOrDefaultAsync();
            var res = _mapper.Map<DiscountDto>(model);
            return res;


        }

        public async Task<DiscountDto> GetById(Guid Id)
        {

            var model =await _repositoryManager.IDiscountRepository.FindByCondition(x => x.Id ==Id, false).FirstOrDefaultAsync();
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

    }
}
