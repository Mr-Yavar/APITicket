using Contracts.IService;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        public IDiscountService _discountService { get; set; }
        private IReservationService _reservationService { get; set; }

        public BillingController(IDiscountService _concertServic, IReservationService reservationService)
        {
            _discountService = _concertServic;
            _reservationService = reservationService;

        }

        [HttpGet("[action]/{Code}")]
        public async Task<IActionResult> GetDiscount(string Code)
        {
           var res = await  _discountService.GetByCode(Code);
            res.DiscountType = -0;
            res.ObjectId = Guid.Empty;

            return Ok(res);
        }


        public async Task<IActionResult> Pay([FromBody] TempReservation dto )
        {

            return Ok(dto);
        }

    }
}
