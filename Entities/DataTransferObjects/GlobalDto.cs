using Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
   
    /// <summary>
    /// اطلاعات رزرو و بلیت ها
    /// </summary>
    public class ReservationResultDto
    {
        public ConcertInfoDto ConcertInfo { get; set; }
        public EventInfoDto EventInfo { get; set; }
        public List<SeatInfo> seats { get; set; }
        public PlaceInfoDto Place { get; set; }
        public HallInfoDto Hall { get; set; }
        public TransactionInfoDto? Transaction { get; set; }
        public Guid ReservationId { get; set; }
        public Decimal Amount { get; set; }
        public DateTime ReservationDate { get; set; }
        public int? TicketCount {  get; set; } 

    }



    public class TempReservationDto
    {
        public Guid ConcertId { get; set; }
        public List<Guid> TicketIds { get; set; }

        public Guid? DiscountId { get; set; }

    }

    public class TempReservationInfo{
        public Guid ConcertId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Guid? TempReservationId { get; set; }
        public List<SeatInfo> Tickets { get; set; }
        public Decimal Amount { get; set; }
        
    }

    public class Response
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public List<string > Errors { get; set; }
    }
    public class glbEventDto
    {
        public List<EventInfoDto> Events { get; set; }

    }


    public class glbConcertsDto
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int currentPage { get; set; }
        public List<ConcertInfoDto> Concerts { get; set; }



    }

    public class ConcertInfoDto
    {
        public Guid Concertid { get; set; }
        public string Category { get; set; }
        public string img_Url { get; set; }
        public string? poster { get; set; }
        public string Title { get; set; }

        public DateTime StartDate { get; set; }
        public PlaceInfoDto Place { get; set; }
        public PromoterDto? Promoter { get; set; }
        public DateTime EndDate { get; set; }
        public string Note { get; set; }
        public Guid CategoryId { get; set; }
        public Guid PlaceId { get; set; }
        public Decimal lat { get; set; }
        public Decimal lng { get; set; }

        public bool? isEnded { get; set; }
        public KeyItemsDto<EventInfoDto> Events { get; set; }

        public int? ReservationCount { get; set; }
        public int? TicketCount { get; set; }
        public int? ReservedTicketCount { get; set; }

        public int? EventCount { get; set; }
        

    }


    public class PlaceInfoDto
    {

        public Guid placeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }


        public string address { get; set; }

       

    }
    public class HallInfoDto
    {

        public Guid placeId { get; set; }
        public string Name { get; set; }
        


    }

    public class EventInfoDto
    {
        public Guid EventId { get; set; }
        public DateTime StartSaleDate { get; set; }

        public DateTime EndSaleDate { get; set; }

        public DateTime ExecutionDate { get; set; }
        public string? Note { get; set; }
        public bool isFilled { get; set; }
        public bool isEnded { get; set; }
        public List<FloorInfoDto> Floors { get; set; }


    }


    public class FloorInfoDto
    {
        public Guid? floorId { get; set; }
        public string Title { get; set; }

        public bool isDeleted { get; set; }
        public string? seatMap_url { get; set; }
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
        public string? FloorTitle { get; set; }
        public bool isReserved { get; set; }
       
    }


    public class KeyItemsDto<T> where T : class
    {
        public string ItemTitle { get; set; }
        public string ItemDescription { get; set; } = string.Empty;

        public List<T> Items { get; set; }
    }


    public class GlbCreationHallDto
    {
        public Guid? id { get; set; }
        public string Title { get; set; }

        public Guid PlaceId { get; set; }

        public string? Description { get; set; }

        public string? ImgUrl { get; set; }

        public bool IsDeleted { get; set; }


        public List<FloorInfoDto> FloorInfoDtos { get; set; }

    }

    public class FileDto {
        public string Key { get; set; }
        public IFormFile? File { get; set; }

    }
    public class FilesDto
    {

        public List<FileDto> Files { get; set; }

    }

    public class glbCreationConcertDto{

        public Guid? ConcertId { get; set; }
        public string Category { get; set; }
        public string img_Url { get; set; }
        public string? poster { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Note { get; set; }
        public Guid CategoryId { get; set; }
        public Guid PlaceId { get; set; }
        public Guid PromoterId { get; set; }

        public bool? isEnded { get; set; }
        public KeyItemsDto<glbCreationEventDto> Events { get; set; }


    }

    public class glbCreationEventDto
    {
        public Guid? EventId { get; set; }
        public Guid HallId { get; set; }

        public DateTime StartSaleDate { get; set; }

        public DateTime EndSaleDate { get; set; }

        public DateTime ExecutionDate { get; set; }

        public Dictionary<int,List<SeatInfo>> SeatsInfo { get; set; }

    }

}
