using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class HallService : ServiceBase, IHallService
    {


        private DBContextProvider _context { get; set; }
        public HallService(DBContextProvider context, IMapper mapper, IRepositoryManager repositoryManager) : base(repositoryManager, mapper)
        {
            _context = context;
        }



        public async Task<List<HallDto>> GetHallsByPlaceId(Guid placeId)
        {

            return _mapper.Map<List<HallDto>>(_repositoryManager.IHallRepository.FindByCondition(x => x.PlaceId == placeId, false).ToList());
        }

        public async Task<GlbCreationHallDto> GetHallById(Guid Id)
        {
            var hall = _context.Halls.Where(x => x.Id == Id).Include(x => x.Floors).ThenInclude(x => x.FloorSeats).ThenInclude(x => x.Seat).FirstOrDefault();
            GlbCreationHallDto res = new()
            {
                Description = hall.Description,
                PlaceId = hall.PlaceId,
                id = hall.Id,
                ImgUrl = hall.ImgUrl,
                IsDeleted = hall.IsDeleted,
                lat = hall.lat,
                lng = hall.lng,
                Title = hall.Title,
                FloorInfoDtos = new()
            };

            foreach (var floor in hall.Floors)
            {
                var floorInfo = new FloorInfoDto()
                {
                    DisplayOrder = floor.DisplayOrder,
                    Title = floor.Title,
                    floorId = floor.Id,
                    SeatsInfo = new(),
                    seatMap_url = floor.seatMap_url,
                    isDeleted = floor.IsDeleted,
                };

                foreach (var floorSeat in floor.FloorSeats)
                {
                    floorInfo.SeatsInfo.Items.Add(new()
                    {
                        Id = floorSeat.SeatId,
                        FloorSeatId = floorSeat.Id,
                        Name = $"f{floorInfo.DisplayOrder}_r{floorSeat.Seat.Row}_c{floorSeat.Seat.Column}",

                    });
                }
            }


            return res;
        }

        public async Task<bool> ProcessCreationHall(GlbCreationHallDto dto)
        {

            Guid HallId = new Guid();
            Hall hall = new Hall()
            {
                Id = HallId,
                CreatedDate = DateTime.Now,
                Description = dto.Description,
                ImgUrl = dto.ImgUrl,
                IsDeleted = false,
                lat = dto.lat,
                lng = dto.lng,
                PlaceId = dto.PlaceId,
                Title = dto.Title,

            };


            Dictionary<Guid, List<int[]>> floorSeat = new();

            foreach (var floor in dto.FloorInfoDtos)
            {
                Guid floorId = new Guid();
                Floor newfloor = new Floor()
                {
                    Id = floorId,
                    CreatedDate = DateTime.Now,
                    Title = floor.Title,
                    DisplayOrder = floor.DisplayOrder,
                    HallId = HallId,
                    IsDeleted = false,
                    seatMap_url = floor.seatMap_url
                };

                floorSeat.Add(floorId, new());

                foreach (var seat in floor.SeatsInfo.Items)
                {
                    int[] FRC = seat.Name.Replace("f", "").Replace("r", "").Replace("c", "").Split("_").Select(x => Convert.ToInt32(x)).ToArray();
                    if (FRC.Length != 3)
                        continue;

                    floorSeat[floorId].Add(FRC);


                }
                var RCs = floorSeat[floorId].Select(RC => RC[1].ToString() + RC[2].ToString()).ToList();
                var Seats = _repositoryManager.ISeatRepository.FindByCondition(x => RCs.Contains(x.Row.ToString() + x.Column.ToString()), false).ToList();

                foreach (var Seat in Seats)
                {
                    newfloor.FloorSeats.Add(new()
                    {
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now,
                        FloorId = floorId,
                        SeatId = Seat.Id,
                    });
                }


                hall.Floors.Add(newfloor);
            }



            _repositoryManager.Save();
            return true;
        }
        public async Task<bool> ProcessUpdateHall(GlbCreationHallDto dto)
        {
            var hall = _context.Halls.Where(x => x.Id == dto.id).Include(x => x.Floors).FirstOrDefault();

            if (hall != null)
                throw new Exception("hall was not found");

            hall.Title = dto.Title;
            hall.Description = dto.Description;
            hall.ImgUrl = dto.ImgUrl;
            hall.lng = dto.lng;
            hall.lat = dto.lat;

            Dictionary<Guid, List<int[]>> floorSeat = new();

            foreach (var floor in hall.Floors)
            {

                var data = dto.FloorInfoDtos.FirstOrDefault(x => x.floorId == floor.Id);
                floor.seatMap_url = (data.seatMap_url is null || data.seatMap_url == "") ? floor.seatMap_url : data.seatMap_url;
                floor.Title = data.Title;
                floor.DisplayOrder = data.DisplayOrder;


            }

            foreach (var floor in dto.FloorInfoDtos)
            {
                if (floor.floorId != null && floor.floorId != Guid.Empty)
                    continue;



                Guid floorId = new Guid();
                Floor newfloor = new Floor()
                {
                    Id = floorId,
                    CreatedDate = DateTime.Now,
                    Title = floor.Title,
                    DisplayOrder = floor.DisplayOrder,
                    HallId = hall.Id,
                    IsDeleted = false,
                    seatMap_url = floor.seatMap_url
                };

                floorSeat.Add(floorId, new());

                foreach (var seat in floor.SeatsInfo.Items)
                {
                    int[] FRC = seat.Name.Replace("f", "").Replace("r", "").Replace("c", "").Split("_").Select(x => Convert.ToInt32(x)).ToArray();
                    if (FRC.Length != 3)
                        continue;

                    floorSeat[floorId].Add(FRC);


                }
                var RCs = floorSeat[floorId].Select(RC => RC[1].ToString() + RC[2].ToString()).ToList();
                var Seats = _repositoryManager.ISeatRepository.FindByCondition(x => RCs.Contains(x.Row.ToString() + x.Column.ToString()), false).ToList();

                foreach (var Seat in Seats)
                {
                    newfloor.FloorSeats.Add(new()
                    {
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now,
                        FloorId = floorId,
                        SeatId = Seat.Id,
                    });
                }




                _repositoryManager.IFloorRepository.Update(newfloor);

            }


            _context.Halls.Update(hall);
            _context.SaveChanges();
            return true;
        }


    }
}
