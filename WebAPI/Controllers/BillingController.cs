using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.PayingDto;
using Entities.DataTransferObjects.PayService.PayPing;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private IDiscountService _discountService { get; set; }
        private IReservationService _reservationService { get; set; }

        //  public IConfigurationSection _configuration;

        public BillingController(IDiscountService _concertServic, IReservationService reservationService)
        {
            _discountService = _concertServic;
            _reservationService = reservationService;

   

        }

        [HttpGet("[action]/{TempReservationId}/{Code}")]
        public async Task<IActionResult> GetDiscount(Guid TempReservationId,string Code)
        {
            DiscountDto res = await  _discountService.GetByCode(TempReservationId,Code);
            if (res == null)
            {
                res = new DiscountDto()
                {
                    IsActive = false,
                    DiscountType=Entities.Enum.DiscountType.INVALIAD 
                };
            }else
            res.DiscountType = -0;
            res.ObjectId = Guid.Empty;

            return Ok(res);
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> Pay([FromBody] toPayDto dto )
        {
            Guid UserId = Guid.Parse(base.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
            if(UserId == Guid.Empty) 
                return Unauthorized();

           var res =await  _reservationService.ReservationToPay(UserId,dto);




            return Ok(res);
        }



        [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> Verify([FromBody] toVerifyDto dto)
        {
            Guid UserId = Guid.Parse(base.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var res = await _reservationService.VerifyPaidBilling(UserId, dto);

            return Ok(res);
        }

    }
}
