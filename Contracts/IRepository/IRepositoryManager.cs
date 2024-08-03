using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.IRepository
{
    public interface IRepositoryManager
    {
        ICategoryRepository ICategoryRepository { get; }
        IEventRepository IEventRepository { get; }
        IConcertRepository IConcertRepository { get; }
        ITicketRepository ITicketRepository { get; }
        IHallRepository IHallRepository { get; }
        ISeatRepository ISeatRepository { get; }
        IReservationRepository IReservationRepository { get; }
        IPromoterRepository IPromoterRepository { get; }
        IPlaceRepository IPlaceRepository { get; }
        IFloorRepository IFloorRepository { get; }
        IFloorSeatRepository IFloorSeatRepository { get; }


        IDynParametersRepository IDynParametersRepository { get; }
        IAttachmentsRepository IAttachmentsRepository { get; }

        IDiscountRepository IDiscountRepository { get; }
        ITransactionRepository ITransactionRepository { get; }
        ITempReservationRepository ITempReservationRepository { get; }
        void Save();
    }
}
