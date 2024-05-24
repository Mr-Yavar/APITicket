using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class glbEventDto{
        public List<EventInfoDto> Events { get; set; }

    }


    public class glbConcertsDto
    {

        public List<ConcertInfoDto> Concerts { get; set; }

 

    }

    public class ConcertInfoDto
    {
        public Guid Concertid { get; set; }
        public string Category { get; set; }
        public string img_Url { get; set; }
        public string? poster {  get; set; }
        public string Title { get; set; }

        public DateTime StartDate { get; set; }
        public PlaceInfoDto Place { get; set; }

        public DateTime EndDate { get; set; }
        public Decimal lat { get; set; }
        public Decimal lng { get; set; }

        public KeyItemsDto<EventInfoDto> Events { get; set; }

    }


    public class PlaceInfoDto
    {
                      
        public Guid placeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string address { get; set; }

    }


    public class EventInfoDto
    {
        public Guid EventId { get; set; }
        public DateTime StartSaleDate { get; set; }

        public DateTime EndSaleDate { get; set; }

        public DateTime ExecutionDate { get; set; }
        public string? Note { get; set; }
        public bool isFilled { get; set; }
        public List<FloorInfoDto> Floors { get; set; }


    }


    public class FloorInfoDto
    {
        public Guid floorId { get; set; }
        public string Title { get; set; }

        public string seatMap_url { get; set; }
        public int DisplayOrder { get; set; }

        public KeyItemsDto<SeatInfo> SeatsInfo { get; set; }
    }


    public class SeatInfo
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Decimal Price { get; set; }

        public Guid EventId { get; set; }

        public Guid SeatId { get; set; }
        public Guid FloorSeatId { get; set; }
        public Guid? ReservedGroupId { get; set; }

        public bool isReserved { get; set; }

    }


    public class KeyItemsDto<T> where T : class
    {
        public string ItemTitle { get; set; }
        public string ItemDescription { get; set; } = string.Empty;

        public List<T> Items { get; set; }
    }
}
