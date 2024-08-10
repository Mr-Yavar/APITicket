using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.PayingDto;
using Entities.Enum;
using Entities.Models;
using Entities.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
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
        private IDiscountService _discountService;
        private UserManager<User> _userManager;
        private ITransactionService _transactionService;
        private string _frontEnd;
        public ReservationService(IMapper mapper, IRepositoryManager repositoryManager, IConfiguration configuration, IDiscountService discountService, UserManager<User> userManager, ITransactionService transactionService) : base(repositoryManager, mapper)
        {

            _TimeOutExpirationDate = Convert.ToInt32(configuration.GetSection("Configuration:TicketTimeout").Value ?? "0");
            _discountService = discountService;
            _userManager = userManager;
            _transactionService = transactionService;
           
            _frontEnd = configuration.GetSection("Configuration:FrontEnd").Value;
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
                ), x => x.TempReservation).ToListAsync();

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



        public async Task<bool> RemoveTemporaryReservation(Guid UserId)
        {


            var tempReserve = await _repositoryManager.ITempReservationRepository.FindByCondition(x => (x.UserId == UserId.ToString() && x.Status == Entities.Enum.ReservationStatusType.InProgress), false).FirstOrDefaultAsync();
            if (tempReserve == null)
                return true;

            Guid TempReservedId = tempReserve.Id;
            var tickets = await _repositoryManager.ITicketRepository.GetByWithIncludes(x => x.TempReservedId == TempReservedId, x => x.TempReservation).ThenInclude(x => x.Transactions).ToListAsync();

            await _transactionService.CancelTransactionsByTempReservation(TempReservedId);

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

        /// <summary>
        /// رزرو به پرداخت
        /// به انتهای آدرس وب آیدی تراکنش اضافه میشود ، دقت شود
        /// نوع سرویس دهنده
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<toPayResponse> ReservationToPay(Guid UserId, toPayDto dto)
        {
            var userTcikets = await _repositoryManager.ITempReservationRepository
                .GetByWithIncludes(x => x.ExpirationDate > DateTime.Now && x.UserId == UserId.ToString() && x.Id == dto.ReservationId && x.Status == Entities.Enum.ReservationStatusType.InProgress, x => x.Tickets).FirstOrDefaultAsync();

            if (userTcikets == null)
                return new()
                {
                    Amount = -1,
                    PayUrl = null,

                };

            var User = await _userManager.FindByIdAsync(UserId.ToString());

            var Amount = userTcikets.Tickets.Sum(x => x.Price);


            dto.url = dto.url.TrimEnd('/');
            Guid TransactionId = Guid.NewGuid();
            BillingDto billingDto = new BillingDto()
            {
                Amount = Amount,
                DiscountId = dto.DiscountId,
                BillingAmount = dto.DiscountId.HasValue ? await _discountService.CalculateDiscount(Amount, dto.DiscountId.Value, Guid.Empty, Guid.Empty, Guid.Empty, true) : Amount,
                FullName = User.FirstName + " " + User.LastName,
                PhoneNumber = User.PhoneNumber,
                //  ReturnUrl = _frontEnd + dto.url + "/"+dto.PayService +"/" + TransactionId.ToIfcGuid(),
                TransactionId = TransactionId,
                UserId = UserId,
                TempReservationId = dto.ReservationId,
                PayService = dto.PayService,
            };

            billingDto.PayService = billingDto.BillingAmount < 10 ? PayServiceType.NoService : billingDto.PayService;
            billingDto.ReturnUrl = _frontEnd + dto.url + "/" + billingDto.PayService + "/" + TransactionId.ToIfcGuid();

            string payUrl = await _transactionService.CreatePayment(billingDto); // کد درگاه پرداخت


            return new()
            {
                Amount = Amount,
                PayUrl = (billingDto.PayService == PayServiceType.NoService) ? billingDto.ReturnUrl : payUrl,
                PayService = billingDto.PayService.ToString(),
            };
        }

        /// <summary>
        /// اعتبار سنجی پرداخت صورتحساب برای رزرو
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<toVerifyResponse> VerifyPaidBilling(Guid UserId, toVerifyDto dto)
        {
            /***
             *  چند حالت است:
             *  1- In temporary reservation :
             *      1-1- InProgress && Not Expired   -> tx should verify
             *      1-2- InProgress && Expired  -> tx should not verify
             *      1-3- Cancelled && (Expired || Not Expired) -> tx should not verify
             *      
             *  2- In permanent reservation : -> tx should not verify
             *      2-1- Completed 
             *      2-2- Cancelled
             */
            var userTcikets = await _repositoryManager.ITransactionRepository.GetByWithIncludes(x => x.Id == dto.clientrefid &&
            (x.TempReservedId != null && x.TempReservation.UserId == UserId.ToString() || x.ReservedGroupId != null && x.Reservation.UserId == UserId.ToString()) &&
            ((x.TempReservation.ExpirationDate > DateTime.Now && x.TempReservation.Status == ReservationStatusType.InProgress) || x.Reservation.Status == ReservationStatusType.Completed)
            , x => x.TempReservation).ThenInclude(x => x.Tickets).Include(x => x.Reservation).FirstOrDefaultAsync();

            //رزرو فاقد اعتبار بوده است
            if (userTcikets == null)
            {

                var tempTranactions = await _repositoryManager.ITransactionRepository.GetByWithIncludes(x => x.Id == dto.clientrefid && x.TempReservation.Status == ReservationStatusType.InProgress, x => x.TempReservation).ThenInclude(x => x.Tickets).FirstOrDefaultAsync();

                //ممکن است کاربر دوباره به آدرسی برا پردازش یک مورد قدیمی اقدام کند
                if (tempTranactions != null)
                {
                    tempTranactions.Status = PayStatusType.Refuted;
                    _repositoryManager.ITransactionRepository.Update(tempTranactions);

                    tempTranactions.TempReservation.Status = ReservationStatusType.Cancelled;
                    _repositoryManager.ITempReservationRepository.Update(tempTranactions.TempReservation);
                    _repositoryManager.ITempReservationRepository.SaveChanges();
                }

                return new()
                {
                    Success = false,
                    Reservation = null,
                    Seats = null,
                    Message = EnumHelper.GetEnumDescription<ReservationResultMessage>(ReservationResultMessage.ReservationCancelled),
                };



            }

            ///اگر وضعیت تراکنش تاییده شده بود از قبل
            if (userTcikets.Reservation != null && userTcikets.Status == PayStatusType.Verified && userTcikets.Reservation.Status == ReservationStatusType.Completed)
            {
                return new()
                {
                    Message = EnumHelper.GetEnumDescription<ReservationResultMessage>(ReservationResultMessage.AlreadySuccess),
                    Success = true,
                    Seats = new(),
                    Reservation = new()
                    {

                    },
                    ReservationId = userTcikets.ReservedGroupId.Value,
                    //  Seats = _repositoryManager.ITicketRepository
                    //.FindByCondition(x => x.TempReservedId == dto.clientrefid, false)
                    //.Include(x => x.FloorSeat.Floor).Include(x => x.FloorSeat.Seat)
                    //.Select(ticket => new SeatInfo()
                    //{
                    //    Id = ticket.Id,
                    //    EventId = ticket.EventId,
                    //    FloorSeatId = ticket.FloorSeatId,
                    //    Name = "f" + ticket.FloorSeat.Floor.DisplayOrder + "_r" + ticket.FloorSeat.Seat.Row.ToString() + "_c" + ticket.FloorSeat.Seat.Column.ToString(),
                    //    Price = ticket.Price,
                    //    SeatId = ticket.FloorSeat.Seat.Id,
                    //    isReserved = ticket.ReservedGroupId != null
                    //}).ToList()

                };
            }

            dto.BillAmount = userTcikets.BillAmount;
            dto.PayService = userTcikets.ServiceType;

            PayStatusType res = await _transactionService.VerifyPayment(dto, userTcikets);


            if (res == PayStatusType.Verified)
            {

                var tempTranactions = await _repositoryManager.ITransactionRepository.GetByWithIncludes(x => x.Id == dto.clientrefid && x.TempReservation.Status == Entities.Enum.ReservationStatusType.InProgress, x => x.TempReservation).ThenInclude(x => x.Tickets).FirstOrDefaultAsync();

                Reservation model = new()
                {
                    CreatedDate = DateTime.Now,
                    DiscountId = tempTranactions.TempReservation.DiscountId,
                    Id = tempTranactions.TempReservation.Id,
                    Status = ReservationStatusType.Completed,
                    UserId = UserId.ToString(),

                };


                _repositoryManager.IReservationRepository.Create(model);
                _repositoryManager.IReservationRepository.SaveChanges();

                foreach (var item in tempTranactions.TempReservation.Transactions)
                {
                    item.ReservedGroupId = tempTranactions.TempReservation.Id;
                    item.TempReservedId = null;
                    _repositoryManager.ITransactionRepository.Update(item);
                }


                foreach (var item in userTcikets.TempReservation.Tickets)
                {
                    item.ReservedGroupId = item.TempReservedId;
                    item.TempReservedId = null;
                    _repositoryManager.ITicketRepository.Update(item);
                }



                _repositoryManager.ITicketRepository.SaveChanges();
                _repositoryManager.ITransactionRepository.SaveChanges();

                _repositoryManager.ITempReservationRepository.Delete(tempTranactions.TempReservation);
                _repositoryManager.ITempReservationRepository.SaveChanges();
                return new()
                {
                    Message = EnumHelper.GetEnumDescription<ReservationResultMessage>(ReservationResultMessage.Success),
                    Success = true,
                    Seats = new(),
                    ReservationId = model.Id,
                };
            }
            else
            {

                var tempTranactions = await _repositoryManager.ITransactionRepository.GetByWithIncludes(x => x.Id == dto.clientrefid && x.TempReservation.Status == Entities.Enum.ReservationStatusType.InProgress, x => x.TempReservation).ThenInclude(x => x.Tickets).FirstOrDefaultAsync();

                tempTranactions.Status = PayStatusType.Refuted;
                _repositoryManager.ITransactionRepository.Update(tempTranactions);

                //tempTranactions.TempReservation.Status = ReservationStatusType.Cancelled;
                //_repositoryManager.ITempReservationRepository.Update(tempTranactions.TempReservation);
                //_repositoryManager.ITempReservationRepository.SaveChanges();


                return new()
                {
                    Message = EnumHelper.GetEnumDescription<ReservationResultMessage>(ReservationResultMessage.NotVerified),
                    Success = false,
                    Seats = null

                };
            }



        }

        /// <summary>
        /// اطلاعات رزرو یوزر - موفق بوده است
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ReservationId"></param>
        /// <returns></returns>
        public async Task<ReservationResultDto> GetUserReservationDetailsById(Guid UserId, Guid ReservationId)
        {
            // تمامی جزئیات از کنسرت تا صندلی
            var UserReservation = await _repositoryManager.IReservationRepository.GetByWithIncludes(x => x.Id == ReservationId && x.UserId == UserId.ToString() && x.Status == ReservationStatusType.Completed, x => x.Tickets)
                .ThenInclude(x => x.Event.Concert)
                .Include(x => x.Transactions.OrderByDescending(x => x.CreatedDate))
                .Include(x => x.Tickets).ThenInclude(x => x.Event.Hall.Place.City)
                .Include(x => x.Tickets).ThenInclude(x => x.FloorSeat.Floor)
                .Include(x => x.Tickets).ThenInclude(x => x.FloorSeat.Seat).FirstOrDefaultAsync();
            if (UserReservation == null)
                return null;

            var eventInfo = UserReservation.Tickets.FirstOrDefault().Event;
            var concertInfo = eventInfo.Concert;


            ReservationResultDto res = new()
            {
                Amount = UserReservation.Transactions.FirstOrDefault().BillAmount,
                ConcertInfo = new()
                {
                    Concertid = concertInfo.Id,
                    Title = concertInfo.Title,
                    EndDate = concertInfo.EndDate,
                    StartDate = concertInfo.StartDate,
                    Note = concertInfo.Note,


                },
                EventInfo = new()
                {
                    EndSaleDate = eventInfo.EndSaleDate,
                    StartSaleDate = eventInfo.StartSaleDate,
                    ExecutionDate = eventInfo.ExecutionDate,
                    Floors = eventInfo.Hall.Floors.Select(x => new FloorInfoDto()
                    {
                        DisplayOrder = x.DisplayOrder,
                        Title = x.Title,
                        floorId = x.Id
                    }).ToList(),
                },
                Place = new() { 
                    address  = eventInfo.Hall.Place.Address,
                    City = eventInfo.Hall.Place.City.Title,
                    Name = eventInfo.Hall.Place.Title,
                    placeId = eventInfo.Hall.Place.Id,
                    Description = eventInfo.Hall.Place.Description,
                },
                Hall = new()
                {
                    Name = eventInfo.Hall.Title,
                   
                },
                ReservationId = UserReservation.Id,
                seats = UserReservation.Tickets
                .Select(ticket => new SeatInfo()
                {
                    Id = ticket.Id,
                    EventId = ticket.EventId,
                    FloorSeatId = ticket.FloorSeatId,
                    Name = "f" + ticket.FloorSeat.Floor.DisplayOrder + "_r" + ticket.FloorSeat.Seat.Row.ToString() + "_c" + ticket.FloorSeat.Seat.Column.ToString(),
                    Price = ticket.Price,
                    SeatId = ticket.FloorSeat.Seat.Id,
                    isReserved = ticket.ReservedGroupId != null
                }).ToList()

            };

            return res;
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
