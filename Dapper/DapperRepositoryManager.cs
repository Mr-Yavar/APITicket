using Contracts.IDapperRepository;
using Dapper.DapperReporsitory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper
{
    public class DapperRepositoryManager : IDapperRepositoryManager
    {
        private readonly DapperContext _dapperContext;
        private readonly Lazy<ICategoryDapperRepository> _ICategoryDapperRepository;
        private readonly Lazy<IEventDapperRepository> _IEventDapperRepository;
        private readonly Lazy<IConcertDapperRepository> _IConcertDapperRepository;
        private readonly Lazy<ITicketDapperRepository> _ITicketDapperRepository;
        private readonly Lazy<IHallDapperRepository> _IHallDapperRepository;
        private readonly Lazy<ISeatDapperRepository> _ISeatDapperRepository;
        private readonly Lazy<IReservationDapperRepository> _IReservationDapperRepository;
        private readonly Lazy<IPromoterDapperRepository> _IPromoterDapperRepository;
        private readonly Lazy<IPlaceDapperRepository> _IPlaceDapperRepository;
        private readonly Lazy<IFloorDapperRepository> _IFloorDapperRepository;
        private readonly Lazy<IFloorSeatDapperRepository> _IFloorSeatDapperRepository;

        public DapperRepositoryManager(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
            _ICategoryDapperRepository = new Lazy<ICategoryDapperRepository>(() => new CategoryDapperRepository(_dapperContext));
            _IEventDapperRepository = new Lazy<IEventDapperRepository>(() => new EventDapperRepository(_dapperContext));
            _IConcertDapperRepository = new Lazy<IConcertDapperRepository>(() => new ConcertDapperRepository(_dapperContext));
            _ITicketDapperRepository = new Lazy<ITicketDapperRepository>(() => new TicketDapperRepository(_dapperContext));
            _IHallDapperRepository = new Lazy<IHallDapperRepository>(() => new HallDapperRepository(_dapperContext));
            _ISeatDapperRepository = new Lazy<ISeatDapperRepository>(() => new SeatDapperRepository(_dapperContext));
            _IReservationDapperRepository = new Lazy<IReservationDapperRepository>(() => new ReservationDapperRepository(_dapperContext));
            _IPromoterDapperRepository = new Lazy<IPromoterDapperRepository>(() => new PromoterDapperRepository(_dapperContext));
            _IPlaceDapperRepository = new Lazy<IPlaceDapperRepository>(() => new PlaceDapperRepository(_dapperContext));
            _IFloorDapperRepository = new Lazy<IFloorDapperRepository>(() => new FloorDapperRepository(_dapperContext));
            _IFloorSeatDapperRepository = new Lazy<IFloorSeatDapperRepository>(() => new FloorSeatDapperRepository(_dapperContext));

        }



        public ICategoryDapperRepository CategoryDapperRepository => _ICategoryDapperRepository.Value;

        public IEventDapperRepository EventDapperRepository => _IEventDapperRepository.Value;

        public IConcertDapperRepository ConcertDapperRepository => _IConcertDapperRepository.Value;
        public ITicketDapperRepository TicketDapperRepository => _ITicketDapperRepository.Value;

        public IHallDapperRepository HallDapperRepository => _IHallDapperRepository.Value;

        public ISeatDapperRepository SeatDapperRepository => _ISeatDapperRepository.Value;

        public IReservationDapperRepository ReservationDapperRepository => _IReservationDapperRepository.Value;

        public IPromoterDapperRepository ProvoterDapperRepository => _IPromoterDapperRepository.Value;

        public IPlaceDapperRepository PlaceDapperRepository => _IPlaceDapperRepository.Value;

        public IFloorDapperRepository FloorDapperRepository => _IFloorDapperRepository.Value;

        public IFloorSeatDapperRepository FloorSeatDapperRepository => _IFloorSeatDapperRepository.Value;
    }
}
