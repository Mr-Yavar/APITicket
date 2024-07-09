using Contracts.IService;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class ManagerController : Controller
    {
        public IConcertService _concertService { get; set; }
        private IPlaceService _placeService;
        private IHallService _hallService;
        public ManagerController(IPlaceService placeService, IHallService hallService, IConcertService concertService)
        {
            _placeService = placeService;
            _concertService = concertService;
            _hallService = hallService;
        }

        [HttpGet("[action]/{pageSize}/{currentPage}")]
        public async Task<IActionResult> GetAllConcerts(int pageSize,int currentPage)
        {

            return Ok(_concertService.GetAllConcerts(pageSize,currentPage));
        }


        [HttpGet("[action]/{pageSize}/{currentPage}")]
        public async Task<IActionResult> GetAllRelatedConcerts(int pageSize, int currentPage)
        {

            return Ok(_concertService.GetAllConcerts(pageSize, currentPage));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CreateConcert()
        {

            return BadRequest();
        }
    }
}
