using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.IRepository
{
    public interface IRepositoryManager
    {
        ICategoryRepository _ICategoryRepository { get; }
        IEventRepository _IEventRepository { get; }
        IConcertRepository _IConcertRepository { get; }
        ITicketRepository _ITicketRepository { get; }
        IHallRepository _IHallRepository { get; }
        ISeatRepository _ISeatRepository { get; }
        IReservationRepository _IReservationRepository { get; }
        IPromoterRepository _IPromoterRepository { get; }
        IPlaceRepository _IPlaceRepository { get; }
        IFloorRepository _IFloorRepository { get; }
        IFloorSeatRepository _IFloorSeatRepository { get; }
        void Save();
    }
}
