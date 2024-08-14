using AutoMapper;
using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.Models;
using Features.CustomRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        IHttpContextAccessor _httpContextAccessor;
        IReservationService _reservationService;
        ITransactionService _transactionService;
        public AccountController(IMapper mapper,
                                 IHttpContextAccessor httpContextAccessor,
                                 UserManager<User> userManager,
                                 IReservationService reservationService,
                                   ITransactionService transactionService
                                 ) : base() {

            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(_httpContextAccessor));
            _reservationService = reservationService ?? throw new ArgumentNullException(    nameof(reservationService));
            _transactionService = transactionService ?? throw new ArgumentNullException(    nameof(transactionService));
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetProfile()
        {
            string UserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userInfo = await _userManager.FindByIdAsync(UserId);

            if(userInfo!= null)
            {
                var res = new UserForRegistrationDto()
                {
                    Email = userInfo.Email,
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName,
                    PhoneNumber = userInfo.PhoneNumber,
                    UserName = userInfo.UserName
                };

                return Ok(res); 
                
            }

            return BadRequest();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDto dto)
        {
            string UserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userInfo = await _userManager.FindByIdAsync(UserId);

            userInfo.PhoneNumber = dto.PhoneNumber;
            userInfo.UserName = dto.UserName;
            userInfo.FirstName = dto.FirstName;
            userInfo.LastName = dto.LastName;   
            userInfo.Email = dto.Email;
            
            await _userManager.UpdateAsync(userInfo);

            var res = new UserForRegistrationDto()
            {
                Email = userInfo.Email,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                PhoneNumber = userInfo.PhoneNumber,
                UserName = userInfo.UserName
            };
            return Ok(res);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdatePassword(ChangePasswordDto dto)
        {
            string UserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userInfo = await _userManager.FindByIdAsync(UserId);
            var res = await _userManager.ChangePasswordAsync(userInfo,dto.CurrentPassword,dto.Password);
            if (res.Succeeded)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Errors);
            }
        }

        [HttpGet("[action]/{pageSize}/{currentPage}")]
        public async Task<IActionResult> MyReservation(int pageSize,int currentPage)
        {
            string UserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _reservationService.GetUserReservations(Guid.Parse(UserId), new() { PageNumber = currentPage, PageSize = pageSize });
            return Ok(res);
        }

        [HttpGet("[action]/{pageSize}/{currentPage}")]
        public async Task<IActionResult> AllPayments(int pageSize, int currentPage)
        {
            string UserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _transactionService.GetUserTransactions(Guid.Parse(UserId), new() { PageNumber = currentPage, PageSize = pageSize });
            return Ok(res);
            
        }



    }
}
