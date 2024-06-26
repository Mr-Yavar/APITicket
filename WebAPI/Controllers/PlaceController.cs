using Contracts.IService;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : ControllerBase
                {

        private IPlaceService _placeService;
        public PlaceController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        /// <summary>
        /// گرفتن تمامی مکان های برگزاری
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllPlaces()
        {
            return Ok(await _placeService.GetAllPlaces());
            
        }

        /// <summary>
        /// گرفتن  مکان  برگزاری
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetPlace(Guid id)
        {
            return Ok(await _placeService.GetById(id));

        }

        /// <summary>
        /// ساختن مکان برگزاری
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreatePlace([FromBody]PlaceDto dto)
        {
            if (await _placeService.ProcessCreationPlace(dto))
                return Ok(dto);
            return BadRequest();

        }

        /// <summary>
        /// ویرایش اطلاعات یک مکان برگزاری
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdatePlace([FromBody] PlaceDto dto)
        {
            if (await _placeService.ProcessUpdatePlace(dto))
                return Ok(dto);
            return BadRequest();

        }

    }
}
