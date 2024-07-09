using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class PlaceService : ServiceBase,IPlaceService
    {


   

        public PlaceService(IMapper mapper,IRepositoryManager repositoryManager):base( repositoryManager,mapper) {
        
      
         
        }


        public async Task<List<PlaceDto>> GetAllPlaces()
        {
           return _mapper.Map< List<PlaceDto>>(_repositoryManager.IPlaceRepository.FindAll(false).ToList());
        }
        public async Task<PlaceDto> GetById(Guid id)
        {
            return _mapper.Map<PlaceDto>(_repositoryManager.IPlaceRepository.FindByCondition(x=>x.Id==id,false).FirstOrDefault());
        }

        public async Task<bool> ProcessCreationPlace(PlaceDto dto)
        {
            if (dto == null)
            return false;

           var place =  _mapper.Map< Place>(dto);
            place.Id = Guid.NewGuid();
           
            _repositoryManager.IPlaceRepository.Create(place);
            _repositoryManager.IPlaceRepository.SaveChanges();
            return true;
        }
        public async Task<bool> ProcessUpdatePlace(PlaceDto dto)
        {
            if (dto == null)
                return false;
           var place = _repositoryManager.IPlaceRepository.FindByCondition(x => x.Id == dto.Id, false).FirstOrDefault();

            place.Title = dto.Title;
            place.Description = dto.Description;
            place.Address = dto.Address;
            place.City = dto.City;
            place.img_Url = dto.img_Url ?? place.img_Url;
            _repositoryManager.IPlaceRepository.Update(place);
            _repositoryManager.IPlaceRepository.SaveChanges();
            return true;
        }


        public async Task<bool> TogglePlace(Guid id)
        {
            var Place = _repositoryManager.IPlaceRepository.FindByCondition(x => x.Id == id, false).FirstOrDefault();
            if (Place is null)
                throw new Exception();

            Place.IsDeleted = Place.IsDeleted ? false : true;
            _repositoryManager.IPlaceRepository.Update(Place);
            _repositoryManager.Save();
            return true;
        }
    }
}
