using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.Enum;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Services;
using WebAPI.Utils;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : ControllerBase
    {

        private IPlaceService _placeService;
        private IHallService _hallService;
        public PlaceController(IPlaceService placeService, IHallService hallService)
        {
            _placeService = placeService;
            _hallService = hallService;
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
        public async Task<IActionResult> CreatePlace([FromForm] FileDto Files, [FromForm] string Place)
        {

            PlaceDto dto = System.Text.Json.JsonSerializer.Deserialize<PlaceDto>(Place);
            try
            {
                if (Files != null && Files.File != null)
                    dto.img_Url = await FileHelper.SaveFileAsync(Files.File, FileType.FileTypes[FileTypeEnum.Place]);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


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
        public async Task<IActionResult> UpdatePlace([FromForm] FileDto Files, [FromForm] String Place)
        {
            PlaceDto dto = System.Text.Json.JsonSerializer.Deserialize<PlaceDto>(Place);
            try
            {
                if (Files != null && Files.File != null)
                    dto.img_Url = await FileHelper.SaveFileAsync(Files.File, FileType.FileTypes[FileTypeEnum.Place]);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


            if (await _placeService.ProcessUpdatePlace(dto))
                return Ok(dto);
            return BadRequest();

        }

        /// <summary>
        /// گرفتن تمامی سالن های یک مکان برگزاری
        /// </summary>
        /// <param name="placeId">ایدی مکان برگزاری</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetHallsByPlaceId(Guid placeId)
        {
            return Ok(await _hallService.GetHallsByPlaceId(placeId));
        }

        /// <summary>
        /// گرفتن جزئیات یک سالن 
        /// </summary>
        /// <param name="Id">آیدی سالن</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetHallById(Guid Id)
        {
            try
            {
                var res = await _hallService.GetHallById(Id);
                if (res == null)
                    return NotFound();
                else
                    return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new UnExpectedError(ex.Message));
            }
        }

        /// <summary>
        /// ایجاد یک سالن
        /// </summary>
        /// <param name="Files">عنصر شاخص ترتیب طبقات است</param>
        /// <param name="Hall"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateHall([FromForm] FilesDto Files, [FromForm] String Hall)
        {
            GlbCreationHallDto glbCreationHallDto = System.Text.Json.JsonSerializer.Deserialize<GlbCreationHallDto>(Hall);

            foreach (var file in Files.Files)
            {
                if (file.Key.ToLower() == "hall")
                    glbCreationHallDto.ImgUrl = await FileHelper.SaveFileAsync(file.File, FileType.FileTypes[FileTypeEnum.Hall]);
                else {
                    var floor= glbCreationHallDto.FloorInfoDtos.FirstOrDefault(x => x.DisplayOrder.ToString() == file.Key);
                    if (floor is null)
                        continue;
                    floor.seatMap_url = await FileHelper.SaveFileAsync(file.File, FileType.FileTypes[FileTypeEnum.SeatMap]);

                } 
            }

            if (glbCreationHallDto.FloorInfoDtos.Any(x => x.seatMap_url == null || x.seatMap_url == "" || x.seatMap_url == string.Empty))
                return BadRequest(new SeatMapDoesNotSpciefied());

            _hallService.ProcessCreationHall(glbCreationHallDto);

            return Ok();
        }

        [HttpPatch("[action]")]
        public async Task<IActionResult> UpdateHall([FromForm] FilesDto Files, [FromForm] String Hall)
        {
            GlbCreationHallDto glbCreationHallDto = System.Text.Json.JsonSerializer.Deserialize<GlbCreationHallDto>(Hall);

            foreach (var file in Files.Files)
            {
                if (file.Key.ToLower() == "hall")
                    glbCreationHallDto.ImgUrl = await FileHelper.SaveFileAsync(file.File, FileType.FileTypes[FileTypeEnum.Hall]);
                else
                {
                    var floor = glbCreationHallDto.FloorInfoDtos.FirstOrDefault(x => x.DisplayOrder.ToString() == file.Key);
                    if (floor is null)
                        continue;
                    floor.seatMap_url = await FileHelper.SaveFileAsync(file.File, FileType.FileTypes[FileTypeEnum.SeatMap]);

                }
            }

        

            _hallService.ProcessUpdateHall(glbCreationHallDto);

            return Ok();
        }

        [HttpGet("[action]/{PlaceId}")]
        public async Task<IActionResult> TogglePlace(Guid PlaceId)
        {
            await _placeService.TogglePlace(PlaceId);
            return Ok(new { status= 200});
        }

        [HttpGet("[action]/{PlaceId}")]
        public async Task<IActionResult> ToggleHall(Guid PlaceId)
        {
            await _hallService.ToggleHall(PlaceId);
            return Ok(new { status = 200 });
        }
    }
}
