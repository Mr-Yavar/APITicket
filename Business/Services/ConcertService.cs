using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ConcertService : ServiceBase, IConcertService
    {
        private DBContextProvider _context;
        public ConcertService(IRepositoryManager repositoryManager,
                              IMapper mapper,
                              DBContextProvider context) : base(repositoryManager, mapper)
        {
            _context = context;
        }

        public glbConcertsDto GetAllConcerts(int pageSize, int currentPage)
        {
            if (currentPage <= 0)
                throw new Exception();


            var concerts = new glbConcertsDto()
            {
                currentPage = currentPage,
                TotalCount = _repositoryManager.IConcertRepository.FindAll(false).Count(),
                PageSize = pageSize,
                Concerts = new(),
            };

            

            _repositoryManager.IConcertRepository.FindAll(false).Skip(((currentPage-1)*pageSize)).Take(currentPage).Include(x=>x.Category).Include(x=>x.Place).ToList().ForEach(concert =>
            {

                concerts.Concerts.Add(new()
                {
                    Title = concert.Title,
                    EndDate = concert.EndDate,
                    StartDate = concert.StartDate,
                    Category = concert.Category.Title,
                    img_Url = concert.img_url,
                    Concertid = concert.Id,
                    Place = new()
                    {
                        Name = concert.Place.Title,
                    },
                    isEnded = concert.EndDate >= DateTime.Now

                });

            });

            return concerts;
        }

        public glbConcertsDto GetAllPublicConcerts()
        {

            var concerts = new glbConcertsDto();

            concerts.Concerts = new();

            _context.Concerts.Where(x => x.EndDate >= DateTime.Now).Include(x => x.Category).Include(x => x.Events).Include(x => x.Place).ToList().ForEach(concert =>
            {

                concerts.Concerts.Add(new()
                {
                    Title = concert.Title,
                    EndDate = concert.EndDate,
                    StartDate = concert.StartDate,
                    Category = concert.Category.Title,
                    img_Url = concert.img_url,
                    Concertid = concert.Id,
                    Place = new()
                    {
                        Name = concert.Place.Title,
                    }

                });

            });

            return concerts;

        }



        public glbConcertsDto GetSpecificConcert(Guid ConcertId)
        {
            var concerts = new glbConcertsDto();

            concerts.Concerts = new();

            _context.Concerts.Where(x => x.EndDate >= DateTime.Now && x.Id == ConcertId).Include(x => x.Category).Include(x => x.Events).Include(x => x.Place).ToList().ForEach(concert =>
            {

                concerts.Concerts.Add(new()
                {
                    Title = concert.Title,
                    EndDate = concert.EndDate,
                    StartDate = concert.StartDate,
                    Category = concert.Category.Title,
                    poster = concert.poster,
                    img_Url = concert.img_url,
                    Concertid = concert.Id,
                    Place = new()
                    {
                        placeId = concert.Place.Id,
                        Name = concert.Place.Title,
                        address = concert.Place.Address,
                    },
                    Events = new() { ItemTitle = "Events", Items = concert.Events.Select(x => new EventInfoDto() { 
                        EventId = x.Id,
                        EndSaleDate = x.EndSaleDate,
                        StartSaleDate = x.StartSaleDate,
                        ExecutionDate = x.ExecutionDate,
                        Note = x.Note ,
                        isEnded = !(x.EndSaleDate > DateTime.Now && x.StartSaleDate <= DateTime.Now),
                        isFilled = x.Tickets.Any(x=>x.ReservedGroupId==null),

                    }).ToList() }

                });

            });

            return concerts;


          
        }

        public glbEventDto GetSpecificEvent(Guid EventId)
        {
            var concerts = new glbEventDto();

            concerts.Events = new();

            _context.Events.Where(x => x.ExecutionDate > DateTime.Now && x.EndSaleDate > DateTime.Now && x.StartSaleDate < DateTime.Now  && x.Id==EventId)
                .Include(x=>x.Hall.Floors)
                .Include(x => x.Tickets.OrderBy(x => Convert.ToInt32(((x.FloorSeat.Seat.Row+999).ToString()) + ((x.FloorSeat.Seat.Column + 999).ToString())) )).ThenInclude(y=>y.FloorSeat.Seat).AsNoTracking()
               
                .ToList().ForEach(evnt =>
            {

                concerts.Events.Add(new()
                {

                    EventId = evnt.Id,
                    EndSaleDate = evnt.EndSaleDate,
                    StartSaleDate = evnt.StartSaleDate,
                    ExecutionDate = evnt.ExecutionDate,
                    Note = evnt.Note,
                    isFilled = evnt.Tickets.Any(x => x.ReservedGroupId == null),
                    Floors = evnt.Hall.Floors.Select(floor => new FloorInfoDto()
                    {
                        floorId = floor.Id,
                        Title = floor.Title,
                        DisplayOrder = floor.DisplayOrder,
                        seatMap_url = floor.seatMap_url,
                        SeatsInfo = new ()
                        {
                            ItemTitle = "tikets",
                            Items = evnt.Tickets.Where(x =>  x.FloorSeat.FloorId==floor.Id).Select(ticket => new SeatInfo()
                            {
                                Id = ticket.Id,
                                EventId = evnt.Id,
                                FloorSeatId = ticket.FloorSeatId,
                                Name = "f"+floor.DisplayOrder+"_r"+ticket.FloorSeat.Seat.Row.ToString()+"_c"+ticket.FloorSeat.Seat.Column.ToString(),
                                Price = ticket.Price,
                                SeatId = ticket.FloorSeat.Seat.Id,
                                isReserved = ticket.ReservedGroupId!=null
                            }).ToList()

                        }


                    } ).ToList()


                
                });

            });

            return concerts;



        }





        public async Task<glbCreationConcertDto> GetConcert(Guid ConcertId)
        {
            return default;
        }
        public async Task<glbCreationConcertDto> CreateConcert(glbCreationConcertDto dto)
        {

            if (dto.img_Url == null || dto.poster == null)
                throw new NoImageAndPoster();

            Concert concert = new()
            {
                CategoryId = dto.CategoryId,
                PlaceId = dto.PlaceId,
                CreatedDate = DateTime.Now,
                EndDate = dto.EndDate,
                StartDate = dto.StartDate,
                Id = Guid.NewGuid(),
                img_url = dto.img_Url,
                poster = dto.poster,
                PromoterId = dto.PromoterId,
                Title = dto.Title,
                Note = dto.Note,

            };


            foreach (var eventItem in dto.Events.Items) {

                Event eventModel = new()
                {
                    EndSaleDate = eventItem.EndSaleDate,
                    StartSaleDate = eventItem.StartSaleDate,
                    ExecutionDate = eventItem.ExecutionDate,
                    ConcertId = concert.Id,
                    CreatedDate = DateTime.Now,
                    HallId = eventItem.HallId,
                    Id = Guid.NewGuid(),
                };

              


                foreach(var seat in eventItem.SeatsInfo.Items)
                {
                    if (seat.Price != null && seat.Price > 0)
                    {
                        eventModel.Tickets.Add(new Ticket()
                        {
                            CreatedDate = DateTime.Now,
                            EventId = eventModel.Id,
                            FloorSeatId = seat.FloorSeatId,
                            Id = Guid.NewGuid(),
                            ReservedGroupId = null,
                            Price = seat.Price,
                        });
                    }
                    else
                    {

                    }
                    
                    
                }
                concert.Events.Add(eventModel);

            }

            _repositoryManager.IConcertRepository.Create(concert);
            _repositoryManager.Save();

            return default;

        }



    }
}
