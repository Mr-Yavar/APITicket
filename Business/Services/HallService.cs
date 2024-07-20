using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NLog.LayoutRenderers;
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

            return _mapper.Map<List<HallDto>>(_repositoryManager.IHallRepository.FindByCondition(x => x.PlaceId == placeId && x.IsDeleted == false, false).ToList());
        }

        public async Task<List<HallDto>> GetAllHallsByPlaceId(Guid placeId)
        {

            return _mapper.Map<List<HallDto>>(_repositoryManager.IHallRepository.FindByCondition(x => x.PlaceId == placeId, false).ToList());
        }

        public async Task<List<GlbCreationHallDto>> GetHallsWithSeatsByPlaceId(Guid placeId)
        {

            var halls = _context.Halls.AsNoTracking().Where(x => x.PlaceId == placeId && x.IsDeleted==false).Include(x => x.Floors).ThenInclude(x => x.FloorSeats).ThenInclude(x => x.Seat).ToList();
           List<GlbCreationHallDto> result = new List<GlbCreationHallDto>();
            foreach (var hall in halls)
            {



                GlbCreationHallDto res = new()
                {
                    Description = hall.Description,
                    PlaceId = hall.PlaceId,
                    id = hall.Id,
                    ImgUrl = hall.ImgUrl,
                    IsDeleted = hall.IsDeleted,
                    Title = hall.Title,
                    FloorInfoDtos = new()
                };

                foreach (var floor in hall.Floors.OrderBy(x => x.DisplayOrder))
                {
                    var floorInfo = new FloorInfoDto()
                    {
                        DisplayOrder = floor.DisplayOrder,
                        Title = floor.Title,
                        floorId = floor.Id,

                        seatMap_url = floor.seatMap_url,
                        isDeleted = floor.IsDeleted,
                        SeatsInfo = new() { ItemDescription = "", ItemTitle = "seats", Items = new() { } }
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

                    res.FloorInfoDtos.Add(floorInfo);
                }
                result.Add(res);
            }
            return result;

        }

        public async Task<GlbCreationHallDto> GetHallById(Guid Id)
        {
            var hall = _context.Halls.AsNoTracking().Where(x => x.Id == Id).Include(x => x.Floors).ThenInclude(x => x.FloorSeats).ThenInclude(x => x.Seat).FirstOrDefault();
            GlbCreationHallDto res = new()
            {
                Description = hall.Description,
                PlaceId = hall.PlaceId,
                id = hall.Id,
                ImgUrl = hall.ImgUrl,
                IsDeleted = hall.IsDeleted,
                Title = hall.Title,
                FloorInfoDtos = new()
            };

            foreach (var floor in hall.Floors.OrderBy(x => x.DisplayOrder))
            {
                var floorInfo = new FloorInfoDto()
                {
                    DisplayOrder = floor.DisplayOrder,
                    Title = floor.Title,
                    floorId = floor.Id,

                    seatMap_url = floor.seatMap_url,
                    isDeleted = floor.IsDeleted,
                    SeatsInfo = new() { ItemDescription = "", ItemTitle = "seats", Items = new() { } }
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

                res.FloorInfoDtos.Add(floorInfo);
            }


            return res;
        }

        public async Task<bool> ProcessCreationHall(GlbCreationHallDto dto)
        {

            Guid HallId = Guid.NewGuid();
            Hall hall = new Hall()
            {
                Id = HallId,
                CreatedDate = DateTime.Now,
                Description = dto.Description,
                ImgUrl = dto.ImgUrl,
                IsDeleted = false,
                PlaceId = dto.PlaceId,
                Title = dto.Title,

            };


            Dictionary<Guid, List<int[]>> floorSeat = new();

            foreach (var floor in dto.FloorInfoDtos)
            {
                Guid floorId = Guid.NewGuid();
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


            _repositoryManager.IHallRepository.Create(hall);
            _repositoryManager.Save();
            return true;
        }
        public async Task<bool> ProcessUpdateHall(GlbCreationHallDto dto)
        {
            var hall = _context.Halls.Where(x => x.Id == dto.id).Include(x => x.Floors).FirstOrDefault();

            if (hall == null)
                throw new Exception("hall was not found");

            hall.Title = dto.Title;
            hall.Description = dto.Description;
            hall.ImgUrl = dto.ImgUrl;

            Dictionary<Guid, List<int[]>> floorSeat = new();

            foreach (var floor in hall.Floors)
            {

                var data = dto.FloorInfoDtos.FirstOrDefault(x => x.floorId == floor.Id);
                floor.seatMap_url = (data.seatMap_url is null || data.seatMap_url == "") ? floor.seatMap_url : data.seatMap_url;
                floor.Title = data.Title;
                floor.DisplayOrder = data.DisplayOrder;

                var newSeats = data.SeatsInfo.Items.Where(x => x.FloorSeatId == Guid.Empty || x.FloorSeatId == null).ToList();


                if (newSeats != null && newSeats.Count != 0)
                {
                    foreach (var seat in newSeats)
                    {
                        int[] FRC = seat.Name.Replace("f", "").Replace("r", "").Replace("c", "").Split("_").Select(x => Convert.ToInt32(x)).ToArray();
                        if (FRC.Length != 3)
                            continue;

                        if (floorSeat.TryGetValue(floor.Id, out List<int[]> temp))
                        {
                            temp.Add(FRC);
                        }
                        else
                        {
                            floorSeat.Add(floor.Id, new List<int[]>() { FRC });
                        }



                    }
                    var RCs = floorSeat[floor.Id].Select(RC => RC[1].ToString() + RC[2].ToString()).ToList();

                    var Seats = _repositoryManager.ISeatRepository.FindByCondition(x => RCs.Contains(x.Row.ToString() + x.Column.ToString()), false).ToList();
                    var t  = Seats.Select(x => x.Row.ToString() + x.Column.ToString()).GroupBy(x=>x).Select(x => new {key=x.Key,count=x.Count()}).ToList();
                    foreach (var Seat in Seats)
                    {
                        floor.FloorSeats.Add(new()
                        {
                            Id = Guid.NewGuid(),
                            CreatedDate = DateTime.Now,
                            FloorId = floor.Id,
                            SeatId = Seat.Id,
                        });
                    }
                }



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




                _repositoryManager.IFloorRepository.Create(newfloor);

            }
            _repositoryManager.IFloorRepository.SaveChanges();
            //hall.Floors.Clear();
            _context.Halls.Update(hall);
            _context.SaveChanges();
            return true;
        }


        public async Task<bool> ToggleHall(Guid id)
        {
            var Hall = _repositoryManager.IHallRepository.FindByCondition(x => x.Id == id, false).FirstOrDefault();
            if (Hall is null)
                throw new Exception();

            Hall.IsDeleted = Hall.IsDeleted ? false : true;
            _repositoryManager.IHallRepository.Update(Hall);
            _repositoryManager.Save();
            return true;
        }
    }
}
