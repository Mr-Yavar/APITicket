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
            var userTcikets = await _repositoryManager.IReservationRepository.GetByWithIncludes(x => x.ExpirationDate > DateTime.Now && x.UserId == UserId.ToString() && x.Status == Entities.Enum.ReservationStatusType.InProgress, x => x.TempTickets)
                .ThenInclude(x => x.FloorSeat.Seat)
                .Include(x => x.TempTickets)
                .ThenInclude(x => x.FloorSeat.Floor).AsNoTracking()
                .FirstOrDefaultAsync();

            TempReservationInfo res = new()
            {
                Amount = userTcikets.TempTickets.Sum(x => x.Price),
                CreatedDate = userTcikets.TempTickets.Count != 0 ? userTcikets.TempTickets.First().CreatedDate : DateTime.MinValue,
                ExpirationDate = userTcikets.TempTickets.Count != 0 ? userTcikets.ExpirationDate.Value : DateTime.MinValue,
                Tickets = userTcikets.TempTickets.Count != 0 ? userTcikets.TempTickets.Select(ticket => new SeatInfo()
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


            var tempReserve = await _repositoryManager.IReservationRepository.FindByCondition(x => (x.UserId == UserId.ToString() && x.Status == Entities.Enum.ReservationStatusType.InProgress), true).ToListAsync();
            tempReserve.ForEach(item =>
            {
                item.Status = Entities.Enum.ReservationStatusType.Cancelled;

                _repositoryManager.IReservationRepository.Update(item);
            });

            _repositoryManager.IReservationRepository.SaveChanges();

            var tickets = await _repositoryManager.ITicketRepository
                .GetByWithIncludes(x => dto.TicketIds.Contains(x.Id) && (
                x.TempReservedId == null ||
                x.TempReservation.Status == ReservationStatusType.Cancelled ||
                x.TempReservation.Status == ReservationStatusType.InProgress && x.TempReservation.ExpirationDate < DateTime.Now
                ), x => x.TempReservation ).ToListAsync();

            if (tickets.GroupBy(x => x.Id).Count() != dto.TicketIds.Count())
                return false;

            Guid TempReservedId = Guid.NewGuid();
            DateTime ExpirationDate = DateTime.Now.AddSeconds(_TimeOutExpirationDate);

            _repositoryManager.IReservationRepository.Create(new()
            {
                ExpirationDate = ExpirationDate,
                CreatedDate = DateTime.Now,
                Id = TempReservedId,
                UserId = UserId.ToString(),
                Status = ReservationStatusType.InProgress,
            });
            _repositoryManager.IReservationRepository.SaveChanges();

            foreach (var ticket in tickets)
            {
                ticket.TempReservedId = TempReservedId;
                _repositoryManager.ITicketRepository.Update(ticket);
            }


            _repositoryManager.ITicketRepository.SaveChanges();
            return true;


        }



        public async Task<bool> RemoveTemporaryReservation(Guid UserId, TempReservation dto)
        {



            var tickets = await _repositoryManager.ITicketRepository.FindByCondition(x => dto.TicketIds.Contains(x.Id) && x.TempReservedId != null, true).ToListAsync();

            Guid tempID = tickets.First().TempReservedId.Value;
            DateTime ExpirationDate = DateTime.Now.AddSeconds(_TimeOutExpirationDate);
            foreach (var ticket in tickets)
            {
                ticket.TempReservedId = null;
                _repositoryManager.ITicketRepository.Update(ticket);
            }

            var Reservation = _repositoryManager.IReservationRepository.FindByCondition(x => x.Id == tempID, true).FirstOrDefault();
            Reservation.Status = ReservationStatusType.Cancelled;
            _repositoryManager.IReservationRepository.Update(Reservation);
            _repositoryManager.IReservationRepository.SaveChanges();
            _repositoryManager.ITicketRepository.SaveChanges();
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
