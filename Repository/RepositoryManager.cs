using Contracts.IRepository;
using Contracts.IRepository;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private DBContextProvider _context;


        private readonly Lazy<ICategoryRepository> _ICategoryRepository;
        private readonly Lazy<IEventRepository> _IEventRepository;
        private readonly Lazy<IConcertRepository> _IConcertRepository;
        private readonly Lazy<ITicketRepository> _ITicketRepository;
        private readonly Lazy<IHallRepository> _IHallRepository;
        private readonly Lazy<ISeatRepository> _ISeatRepository;
        private readonly Lazy<IReservationRepository> _IReservationRepository;
        private readonly Lazy<IPromoterRepository> _IPromoterRepository;
        private readonly Lazy<IPlaceRepository> _IPlaceRepository;
        private readonly Lazy<IFloorRepository> _IFloorRepository;
        private readonly Lazy<IFloorSeatRepository> _IFloorSeatRepository;
        private readonly Lazy<IAttachmentsRepository> _IAttachmentsRepository;
        private readonly Lazy<IDynParametersRepository> _IDynParametersRepository;
        private readonly Lazy<IDiscountRepository> _IDiscountRepository;
        private readonly Lazy<ITransactionRepository> _ITransactionRepository;
        private readonly Lazy<ITempReservationRepository> _ITempReservationRepository;
        public RepositoryManager(DBContextProvider Context)
        {
            _context = Context;
            _ICategoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(_context));
            _IEventRepository = new Lazy<IEventRepository>(() => new EventRepository(_context));
            _IConcertRepository = new Lazy<IConcertRepository>(() => new ConcertRepository(_context));
            _ITicketRepository = new Lazy<ITicketRepository>(() => new TicketRepository(_context));
            _IHallRepository = new Lazy<IHallRepository>(() => new HallRepository(_context));
            _ISeatRepository = new Lazy<ISeatRepository>(() => new SeatRepository(_context));
            _IReservationRepository = new Lazy<IReservationRepository>(() => new ReservationRepository(_context));
            _IPromoterRepository = new Lazy<IPromoterRepository>(() => new PromoterRepository(_context));
            _IPlaceRepository = new Lazy<IPlaceRepository>(() => new PlaceRepository(_context));
            _IFloorRepository = new Lazy<IFloorRepository>(() => new FloorRepository(_context));
            _IFloorSeatRepository = new Lazy<IFloorSeatRepository>(() => new FloorSeatRepository(_context));
            _IAttachmentsRepository = new Lazy<IAttachmentsRepository>(() => new AttachmentsRepository(_context));
            _IDynParametersRepository = new Lazy<IDynParametersRepository>(() => new DynParametersRepository(_context));
            _ITempReservationRepository = new Lazy<ITempReservationRepository>(() => new TempReservationRepository(_context));


            _IDiscountRepository = new Lazy<IDiscountRepository>(() => new DiscountRepository(_context));
            _ITransactionRepository = new Lazy<ITransactionRepository>(() => new TransactionRepository(_context));

        }



        public ICategoryRepository ICategoryRepository => _ICategoryRepository.Value;

        public IEventRepository IEventRepository => _IEventRepository.Value;

        public IConcertRepository IConcertRepository => _IConcertRepository.Value;
        public ITicketRepository ITicketRepository => _ITicketRepository.Value;

        public IHallRepository IHallRepository => _IHallRepository.Value;

        public ISeatRepository ISeatRepository => _ISeatRepository.Value;

        public IReservationRepository IReservationRepository => _IReservationRepository.Value;

        public IPromoterRepository IPromoterRepository => _IPromoterRepository.Value;

        public IPlaceRepository IPlaceRepository => _IPlaceRepository.Value;

        public IFloorRepository IFloorRepository => _IFloorRepository.Value;

        public IFloorSeatRepository IFloorSeatRepository => _IFloorSeatRepository.Value;

        public IDynParametersRepository IDynParametersRepository => _IDynParametersRepository.Value;
        public IAttachmentsRepository IAttachmentsRepository => _IAttachmentsRepository.Value;


        public ITransactionRepository ITransactionRepository => _ITransactionRepository.Value;
        public IDiscountRepository IDiscountRepository => _IDiscountRepository.Value;
        public ITempReservationRepository ITempReservationRepository => _ITempReservationRepository.Value;

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

