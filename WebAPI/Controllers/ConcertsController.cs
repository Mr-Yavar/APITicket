using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.PolicyTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConcertsController : ControllerBase
    {

        public IConcertService concertService { get; set; }
        private IReservationService _reservationService { get; set; }

        public ConcertsController(IConcertService _concertServic, IReservationService reservationService)
        {
            concertService = _concertServic;
            _reservationService = reservationService;

        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllConcerts(string CityIds="",string CategoryIds = "",string searchTxt = "")
        {

            List<Guid> Cities = new();
            List<Guid> Categories = new();
      

            var Guids = CityIds.Split(',');
            foreach (var item in Guids)
            {
                if(Guid.TryParse(item,out Guid res))
                    Cities.Add(res);
            }

             Guids = CategoryIds.Split(',');
            foreach (var item in Guids)
            {
                if (Guid.TryParse(item, out Guid res))
                    Categories.Add(res);
            }

            var result = concertService.GetAllPublicConcerts(Cities,Categories,searchTxt);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("[action]/{concertId}")]
        public async Task<IActionResult> GetSpecificConcert(Guid concertId)
        {

            var result = concertService.GetSpecificConcert(concertId);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("[action]/{eventId}")]
        public async Task<IActionResult> GetSpecificEvent(Guid eventId)
        {

            var result = concertService.GetSpecificEvent(eventId);

            return Ok(result);
        }


        /// <summary>
        /// اطلاعات کلی کنسرت
        /// </summary>
        /// <param name="ConcertId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{ConcertId}")]
        public async Task<IActionResult> GetConcertInfo(Guid ConcertId)
        {
            return Ok(await concertService.GetConcertByConcertId(ConcertId));
        }


        /// <summary>
        /// زمانی که کاربر به مرحله سوم رفت باید اضافه شود
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddToResevation([FromBody] TempReservationDto dto)
        {
            Guid UserId = Guid.Parse(base.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Ok(new { tempReservationId = await _reservationService.AddTemporaryReservation(UserId, dto) });
        }

        /// <summary>
        /// زمانی که کاربر از مرحله سوم برمیگردد
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> RemoveFromResevation()
        {
            Guid UserId = Guid.Parse(base.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Ok(await _reservationService.RemoveTemporaryReservation(UserId));
        }


        /// <summary>
        /// رزرو موقت کاربر
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetReservation()
        {
            Guid UserId = Guid.Parse(base.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Ok(await _reservationService.GetUserReservation(UserId));
        }



        [Authorize]
        [HttpGet("[action]/{ReservationId}")]
        public async Task<IActionResult> GetUserReservation(Guid ReservationId)
        {
            Guid UserId = Guid.Parse(base.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var rs = await _reservationService.GetUserReservationDetailsById(UserId, ReservationId);
            return Ok(rs);

        }

    }
}
