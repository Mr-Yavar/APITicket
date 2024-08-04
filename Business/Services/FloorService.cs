using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class FloorService : IFloorService
    {


        public IPlaceRepository _placeRepository {  get; set; }
        public IFloorRepository _floorRepository { get; set; }
        public  IHallRepository _hallRepository { get; set; }
        private IMapper _mapper;

        public FloorService(IMapper mapper,IPlaceRepository placeRepository,IFloorRepository floorRepository,IHallRepository hallRepository) {
        
            _floorRepository = floorRepository;
            _hallRepository = hallRepository;
            _placeRepository = placeRepository;
            _mapper = mapper;
        
        }


        public async Task<List<PlaceDto>> GetAllPlaces()
        {
           return _mapper.Map< List<PlaceDto>>(_placeRepository.FindAll(false).ToList());
        }


    }
}
