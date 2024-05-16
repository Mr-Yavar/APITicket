using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.IDapperRepository
{
    public interface IDapperRepositoryManager
    {
        ICategoryDapperRepository CategoryDapperRepository { get; }
        IEventDapperRepository EventDapperRepository { get; }
        IConcertDapperRepository ConcertDapperRepository { get; }
        ITicketDapperRepository TicketDapperRepository { get; }
        IHallDapperRepository HallDapperRepository { get; }
        ISeatDapperRepository SeatDapperRepository { get; }
        IReservationDapperRepository ReservationDapperRepository { get; }
        IPromoterDapperRepository ProvoterDapperRepository { get; }
        IPlaceDapperRepository PlaceDapperRepository { get; }
        IFloorDapperRepository FloorDapperRepository { get; }
        IFloorSeatDapperRepository FloorSeatDapperRepository { get; }

    }
}
