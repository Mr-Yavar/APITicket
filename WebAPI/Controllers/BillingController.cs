using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.PayService.PayPing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Utils;

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

        [HttpGet("[action]/{Code}")]
        public async Task<IActionResult> GetDiscount(string Code)
        {
           var res = await  _discountService.GetByCode(Code);
            res.DiscountType = -0;
            res.ObjectId = Guid.Empty;

            return Ok(res);
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> Pay([FromBody] TempReservation dto )
        {


            // ساخت کد پرداخت به منظور هدایت به درگاه بانکی
           // var payCode = await RequestHelper.CallApi<PayPing_PaymentResult>(_PAYPING_API["PAYMENT_URL"], , _PAYPING_API["Key"]);

            //هدایت کاربر به درگاه بانک
          //  return Redirect(config.AppSettings["GOTOIPG_URL"] + payCode.Code);




            return Ok(dto);
        }

    }
}
