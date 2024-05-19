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

        [Authorize(Policy = PolicyTypes.Users.EditRole)]
        [HttpGet("[action]")]

        public async Task<IActionResult> GetAllConcerts()
        {
            
           

            var result = concertService.GetAllConcerts();

            return Ok(result);
        }


    }
}
