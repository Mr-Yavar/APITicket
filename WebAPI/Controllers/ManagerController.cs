using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.Enum;
using Entities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Utils;

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

        /// <summary>
        /// گرفتن کنتسرهای مرتبط با کاربر
        /// کاربر مدیر مکان برگزاری
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        [HttpGet("[action]/{pageSize}/{currentPage}")]
        public async Task<IActionResult> GetAllRelatedConcerts(int pageSize, int currentPage)
        {

            return Ok(_concertService.GetAllConcerts(pageSize, currentPage));
        }

        /// <summary>
        /// ایجاد کنسرت
        /// </summary>
        /// <param name="Files"></param>
        /// <param name="Concert"></param>
        /// <returns></returns>
        /// <exception cref="NoImageAndPoster"></exception>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateConcert([FromForm] FilesDto Files, [FromForm] String Concert)
        {

            glbCreationConcertDto dto = System.Text.Json.JsonSerializer.Deserialize<glbCreationConcertDto>(Concert,new System.Text.Json.JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            }) ?? new();

            try
            {

                var file = Files.Files.FirstOrDefault(x => x.Key.ToLower() == "img_url");

                if (file != null && file.File != null)
                    dto.img_Url = await FileHelper.SaveFileAsync(file.File, FileType.FileTypes[FileTypeEnum.Event]);
                else
                   throw new NoImageAndPoster();

                 file = Files.Files.FirstOrDefault(x => x.Key.ToLower() == "poster");

                if (file != null && file.File != null)
                    dto.poster = await FileHelper.SaveFileAsync(file.File, FileType.FileTypes[FileTypeEnum.Event]);
                else
                    throw new NoImageAndPoster();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            await _concertService.CreateConcert(dto);

            return BadRequest();
        }

        ///


        /// <summary>
        /// ایجاد کنسرت
        /// </summary>
        /// <param name="Files"></param>
        /// <param name="Concert"></param>
        /// <returns></returns>
        /// <exception cref="NoImageAndPoster"></exception>
        [HttpPatch("[action]")]
        public async Task<IActionResult> UpdateConcert([FromForm] FilesDto Files, [FromForm] String Concert)
        {

            glbCreationConcertDto dto = System.Text.Json.JsonSerializer.Deserialize<glbCreationConcertDto>(Concert, new System.Text.Json.JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            }) ?? new();

            try
            {

                var file = Files.Files.FirstOrDefault(x => x.Key.ToLower() == "img_url");

                if (file != null && file.File != null)
                    dto.img_Url = await FileHelper.SaveFileAsync(file.File, FileType.FileTypes[FileTypeEnum.Event]);
                else
                    dto.img_Url = null;

                file = Files.Files.FirstOrDefault(x => x.Key.ToLower() == "poster");

                if (file != null && file.File != null)
                    dto.poster = await FileHelper.SaveFileAsync(file.File, FileType.FileTypes[FileTypeEnum.Event]);
                else
                    dto.poster = null;

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            await _concertService.UpdateConcert(dto);

            return BadRequest();
        }



        /// <summary>
        /// اطلاعات کنسرت برای ویرایش
        /// </summary>
        /// <param name="ConcertId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{ConcertId}")]
        public async Task<IActionResult> GetConcertWithDetails(Guid ConcertId)
        {
            return Ok(await _concertService.GetConcert(ConcertId));
        }
    }
}
