using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.Enum;
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
            var userTcikets = await _repositoryManager.ITempReservationRepository.GetByWithIncludes(x => x.ExpirationDate > DateTime.Now && x.UserId == UserId.ToString() && x.Status == Entities.Enum.ReservationStatusType.InProgress, x => x.Tickets)
                .ThenInclude(x => x.FloorSeat.Seat)
                .Include(x => x.Tickets)
                .ThenInclude(x => x.FloorSeat.Floor).AsNoTracking()
                .FirstOrDefaultAsync();

            if (userTcikets == null)
                return new()
                {
                    Amount = -1,
                    Tickets = new()
                };

            TempReservationInfo res = new()
            {
                Amount = userTcikets.Tickets.Sum(x => x.Price),
                CreatedDate = userTcikets.Tickets.Count != 0 ? userTcikets.Tickets.First().CreatedDate : DateTime.MinValue,
                ExpirationDate = userTcikets.Tickets.Count != 0 ? userTcikets.ExpirationDate.Value : DateTime.MinValue,
                Tickets = userTcikets.Tickets.Count != 0 ? userTcikets.Tickets.Select(ticket => new SeatInfo()
                {
                    Id = ticket.Id,
                    EventId = ticket.EventId,
                    FloorSeatId = ticket.FloorSeatId,
                    Name = "f" + ticket.FloorSeat.Floor.DisplayOrder + "_r" + ticket.FloorSeat.Seat.Row.ToString() + "_c" + ticket.FloorSeat.Seat.Column.ToString(),
                    Price = ticket.Price,
                    SeatId = ticket.FloorSeat.Seat.Id,
                    isReserved = ticket.ReservedGroupId != null
                }).ToList() : new(),
                TempReservationId = userTcikets.Id

            };

            return res;

        }

        public async Task<Guid?> AddTemporaryReservation(Guid UserId, TempReservationDto dto)
        {


            var tempReserve = await _repositoryManager.ITempReservationRepository.FindByCondition(x => (x.UserId == UserId.ToString() && x.Status == Entities.Enum.ReservationStatusType.InProgress), true).ToListAsync();
            tempReserve.ForEach(item =>
            {
                item.Status = Entities.Enum.ReservationStatusType.Cancelled;

                _repositoryManager.ITempReservationRepository.Update(item);
            });

            _repositoryManager.ITempReservationRepository.SaveChanges();

            var tickets = await _repositoryManager.ITicketRepository
                .GetByWithIncludes(x => dto.TicketIds.Contains(x.Id) && (
                x.TempReservedId == null ||
                x.TempReservation.Status == ReservationStatusType.Cancelled ||
                x.TempReservation.Status == ReservationStatusType.InProgress && x.TempReservation.ExpirationDate < DateTime.Now
                ), x => x.TempReservation ).ToListAsync();

            if (tickets.GroupBy(x => x.Id).Count() != dto.TicketIds.Count())
                return null;

            Guid TempReservedId = Guid.NewGuid();
            DateTime ExpirationDate = DateTime.Now.AddSeconds(_TimeOutExpirationDate);

            _repositoryManager.ITempReservationRepository.Create(new()
            {
                ExpirationDate = ExpirationDate,
                CreatedDate = DateTime.Now,
                Id = TempReservedId,
                UserId = UserId.ToString(),
                Status = ReservationStatusType.InProgress,
            });
            _repositoryManager.ITempReservationRepository.SaveChanges();

            foreach (var ticket in tickets)
            {
                ticket.TempReservedId = TempReservedId;
                _repositoryManager.ITicketRepository.Update(ticket);
            }


            _repositoryManager.ITicketRepository.SaveChanges();
            return TempReservedId;


        }



        public async Task<bool> RemoveTemporaryReservation(Guid UserId, Guid TempReservedId)
        {



            var tickets = await _repositoryManager.ITicketRepository.GetByWithIncludes(x => x.TempReservedId == TempReservedId , x=>x.TempReservation).ToListAsync();

            if (tickets.Count() == 0)
                return true;

            Guid tempID = tickets.First().TempReservedId.Value;
            DateTime ExpirationDate = DateTime.Now.AddSeconds(_TimeOutExpirationDate);
            foreach (var ticket in tickets)
            {
                ticket.TempReservedId = null;
                _repositoryManager.ITicketRepository.Update(ticket);
            }

            var Reservation = _repositoryManager.ITempReservationRepository.FindByCondition(x => x.Id == tempID, true).FirstOrDefault();
            Reservation.Status = ReservationStatusType.Cancelled;
            _repositoryManager.ITempReservationRepository.Update(Reservation);
            _repositoryManager.ITempReservationRepository.SaveChanges();
            _repositoryManager.ITicketRepository.SaveChanges();
            return true;


        }


        public async Task<bool> ReservationToPay(Guid UserId, Guid TempReservedId)
        {
            return true;
        }


        //public async Task<bool> AddReservation(Guid UserId, TempReservation dto)
        //{
        //    var tickets = await _repositoryManager.ITicketRepository.FindByCondition(x => dto.TicketIds.Contains(x.Id) && x.ExpirationDate > DateTime.Now && x.UserId == UserId && x.ReservedGroupId == null, true).ToListAsync();


        //    DateTime ExpirationDate = DateTime.Now.AddSeconds(_TimeOutExpirationDate);
        //    foreach (var ticket in tickets)
        //    {
        //        ticket.UserId = null;
        //        ticket.ExpirationDate = null;
        //        _repositoryManager.ITicketRepository.Update(ticket);
        //    }


        //    _repositoryManager.ITicketRepository.SaveChanges();
        //    return true;


        //}
    }
}
