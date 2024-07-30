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
    public class PromoterService : ServiceBase, IPromoterService
    {


   

        public PromoterService(IMapper mapper,IRepositoryManager repositoryManager):base( repositoryManager,mapper) {
        
      
         
        }


        public async Task<List<PromoterDto>> GetAllPromoters()
        {
            var res = await _repositoryManager.IPromoterRepository.FindAll(false).ToListAsync();
            if (res != null )
           return _mapper.Map< List<PromoterDto>>(res);
            return default;
        }

        public async Task<List<PromoterDto>> GetPromoters()
        {
            return _mapper.Map<List<PromoterDto>>(_repositoryManager.IPromoterRepository.GetByWithIncludes(x=>x.IsDeleted==false,x=>x.Category).ToList());
        }

        public async Task<PromoterDto> GetPromoter(Guid id)
        {
            return _mapper.Map<PromoterDto>(_repositoryManager.IPromoterRepository.FindByCondition(x=>x.Id==id,false).FirstOrDefault());
        }

        public async Task<bool> ProcessCreationPromoter(PromoterDto dto)
        {
            if (dto == null)
            return false;

           var p =  _mapper.Map< Promoter>(dto);
          
           
            _repositoryManager.IPromoterRepository.Create(p);
            _repositoryManager.IPromoterRepository.SaveChanges();
            return true;
        }
        public async Task<bool> ProcessUpdatePromoter(PromoterDto dto)
        {
            if (dto == null)
                return false;
           var p =await _repositoryManager.IPromoterRepository.FindByCondition(x => x.Id == dto.Id, false).FirstOrDefaultAsync();

            p.Title = dto.Title;
            p.Description = dto.Description;
            p.CategoryId = dto.CategoryId;
           
            p.img_url = dto.img_url ?? p.img_url;
            _repositoryManager.IPromoterRepository.Update(p);
            _repositoryManager.IPromoterRepository.SaveChanges();
            return true;
        }


        public async Task<bool> TogglePromoter(Guid id)
        {
            var p =await _repositoryManager.IPromoterRepository.FindByCondition(x => x.Id == id, false).FirstOrDefaultAsync();
            if (p is null)
                throw new Exception();

            p.IsDeleted = p.IsDeleted ? false : true;
            _repositoryManager.IPromoterRepository.Update(p);
            _repositoryManager.Save();
            return true;
        }
    }
}
