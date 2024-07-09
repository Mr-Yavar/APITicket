using Contracts.IService;
using Entities.Models;
using Entities.PolicyTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConcertsController : ControllerBase
    {

        public IConcertService concertService { get; set; }

        public ConcertsController(IConcertService _concertServic)
        { 
        concertService = _concertServic;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllConcerts()
        {
            
            var result = concertService.GetAllPublicConcerts();

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
    }
}
