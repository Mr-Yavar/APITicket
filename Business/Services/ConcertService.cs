using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities;
using Entities.DataTransferObjects;
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
        public ConcertService(IRepositoryManager repositoryManager, IMapper mapper, DBContextProvider context) : base(repositoryManager, mapper)
        {
            _context = context;
        }



        public glbConcertsDto GetAllConcerts()
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
                        isFilled = x.Tickets.Any(x=>x.ReservedGroupId==null),

                    }).ToList() }

                });

            });

            return concerts;


            return null;
        }
    }
}
