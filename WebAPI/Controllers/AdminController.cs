using Contracts.IService;
using Entities.DataTransferObjects;
using Features.CustomRequest;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        

        private IUsersService _userService;
        IReservationService _reservationService;
        ITransactionService _transactionService;
        public AdminController(IUsersService userService, IReservationService reservationService, ITransactionService transactionService) : base() {
            _userService = userService;
            _reservationService = reservationService;
            _transactionService = transactionService;
        }

        /// <summary>
        /// گرفتن همه کاربران به صورت صفحه بندی
        /// نقش ادمین سیستم
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("[action]/{pageSize}/{currentPage}")]
        public async Task<IActionResult> GetUsers(int pageSize,int currentPage,string? name=null)        {

            var res = await _userService.GetUsers(new()
            {
                FullName = name,
                PageSize = pageSize,
                PageNumber = currentPage
            });

            return Ok(res);
        }


        /// <summary>
        /// گرفتن  کاربر
        /// نقش ادمین سیستم
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("[action]/{UserId}")]
        public async Task<IActionResult> GetUser(Guid UserId)
        {

            var res = await _userService.GetUser(UserId);

            return Ok(res);
        }

        /// <summary>
        /// ویرایش اطلاعات کاربر
        /// نقش ادمین سیستم
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto dto)
        {

            var res = await _userService.UpdateUser(dto);
            if (res.Status == "Success")
                return Ok(res);
            else
                return BadRequest(res);
        }


        [HttpGet("[action]/{UserId}/{pageSize}/{currentPage}")]
        public async Task<IActionResult> GetUserPayments(Guid UserId ,int pageSize, int currentPage)
        {
            var res = await _transactionService.GetUserTransactions(UserId, new() { PageNumber = currentPage ,PageSize = pageSize});
            return Ok(res);

        }


        [HttpGet("[action]/{UserId}/{pageSize}/{currentPage}")]
        public async Task<IActionResult> GetUserReservations(Guid UserId, int pageSize, int currentPage)
        {
            var res = await _reservationService.GetUserReservations(UserId, new() { PageNumber = currentPage, PageSize = pageSize });
            return Ok(res);

        }


        [HttpGet("[action]/{ConcertId}/{pageSize}/{currentPage}")]
        public async Task<IActionResult> GetConcertReservations(Guid ConcertId, int pageSize, int currentPage)
        {
            var res = await _reservationService.GetConcertReservations(ConcertId, new() { PageNumber = currentPage, PageSize = pageSize });
            return Ok(res);

        }



    }
}
