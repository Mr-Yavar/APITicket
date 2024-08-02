using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ReservationService : ServiceBase, IReservationService
    {


        private int _TimeOutExpirationDate = 0;

        public ReservationService(IMapper mapper, IRepositoryManager repositoryManager, IConfiguration configuration) : base(repositoryManager, mapper)
        {

            _TimeOutExpirationDate = Convert.ToInt32(configuration.GetSection("Configuration:TicketTimeout").Value ?? "0");


        }


        public async Task<TempReservationInfo> GetUserReservation(Guid UserId)
        {
            var userTcikets = await _repositoryManager.ITicketRepository.GetByWithIncludes(x => x.ExpirationDate > DateTime.Now && x.UserId == UserId && x.ReservedGroupId == null, x => x.FloorSeat.Floor)
                .Include(x => x.FloorSeat.Seat)
                .ToListAsync();

            TempReservationInfo res = new()
            {
                Amount = userTcikets.Sum(x => x.Price),
                CreatedDate = userTcikets.Count != 0 ?  userTcikets[0].CreatedDate : DateTime.MinValue,
                ExpirationDate = userTcikets.Count != 0 ?  userTcikets[0].ExpirationDate.Value : DateTime.MinValue,
                Tickets = userTcikets.Count!=0 ? userTcikets.Select(ticket => new SeatInfo()
                {
                    Id = ticket.Id,
                    EventId = ticket.EventId,
                    FloorSeatId = ticket.FloorSeatId,
                    Name = "f" + ticket.FloorSeat.Floor.DisplayOrder + "_r" + ticket.FloorSeat.Seat.Row.ToString() + "_c" + ticket.FloorSeat.Seat.Column.ToString(),
                    Price = ticket.Price,
                    SeatId = ticket.FloorSeat.Seat.Id,
                    isReserved = ticket.ReservedGroupId != null
                }).ToList() : new()

            };

            return res;

        }

        public async Task<bool> AddTemporaryReservation(Guid UserId, TempReservation dto)
        {


            var userTcikets = await _repositoryManager.ITicketRepository.FindByCondition(x => x.ExpirationDate > DateTime.Now && x.UserId == UserId && x.ReservedGroupId == null, true).ToListAsync();
            userTcikets.ForEach(item =>
            {
                item.ExpirationDate = null;
                item.UserId = null;
                _repositoryManager.ITicketRepository.Update(item);
            });

            _repositoryManager.ITicketRepository.SaveChanges();

            var tickets = await _repositoryManager.ITicketRepository.FindByCondition(x => dto.TicketIds.Contains(x.Id) && (x.ExpirationDate < DateTime.Now || x.ExpirationDate == null) && x.ReservedGroupId == null, true).ToListAsync();

            if (tickets.GroupBy(x => x.Id).Count() != dto.TicketIds.Count())
                return false;


            DateTime ExpirationDate = DateTime.Now.AddSeconds(_TimeOutExpirationDate);
            foreach (var ticket in tickets)
            {
                ticket.UserId = UserId;
                ticket.ExpirationDate = ExpirationDate;
                _repositoryManager.ITicketRepository.Update(ticket);
            }


            _repositoryManager.ITicketRepository.SaveChanges();
            return true;


        }



        public async Task<bool> RemoveTemporaryReservation(Guid UserId, TempReservation dto)
        {



            var tickets = await _repositoryManager.ITicketRepository.FindByCondition(x => dto.TicketIds.Contains(x.Id) && x.ExpirationDate > DateTime.Now && x.UserId == UserId && x.ReservedGroupId == null, true).ToListAsync();


            DateTime ExpirationDate = DateTime.Now.AddSeconds(_TimeOutExpirationDate);
            foreach (var ticket in tickets)
            {
                ticket.UserId = null;
                ticket.ExpirationDate = null;
                _repositoryManager.ITicketRepository.Update(ticket);
            }


            _repositoryManager.ITicketRepository.SaveChanges();
            return true;


        }


        public async Task<bool> AddReservation(Guid UserId, TempReservation dto)
        {
            var tickets = await _repositoryManager.ITicketRepository.FindByCondition(x => dto.TicketIds.Contains(x.Id) && x.ExpirationDate > DateTime.Now && x.UserId == UserId && x.ReservedGroupId == null, true).ToListAsync();


            DateTime ExpirationDate = DateTime.Now.AddSeconds(_TimeOutExpirationDate);
            foreach (var ticket in tickets)
            {
                ticket.UserId = null;
                ticket.ExpirationDate = null;
                _repositoryManager.ITicketRepository.Update(ticket);
            }


            _repositoryManager.ITicketRepository.SaveChanges();
            return true;


        }
    }
}
