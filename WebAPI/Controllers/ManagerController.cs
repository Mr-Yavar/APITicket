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
        private IAttachmentsService _attachmentsService;
        private IDiscountService _discount;
        public ManagerController(IPlaceService placeService, IHallService hallService, IConcertService concertService,IAttachmentsService attachments,IDiscountService discount)
        {
            _placeService = placeService;
            _concertService = concertService;
            _hallService = hallService;
            _attachmentsService = attachments;
            _discount= discount;
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
            dto.ConcertId = Guid.NewGuid();
            try
            {

                var file = Files.Files.FirstOrDefault(x => x.Key.ToLower() == "img_url");

                if (file != null && file.File != null)
                {
                    string tempName = await FileHelper.SaveFileAsync(file.File, FileType.FileTypes[FileTypeEnum.Event]);
                    dto.img_Url = await _attachmentsService.AddToStore(tempName, FileType.FileTypes[FileTypeEnum.Event],file.File.ContentType, dto.ConcertId.Value);
                }
                else
                    throw new NoImageAndPoster();

                 file = Files.Files.FirstOrDefault(x => x.Key.ToLower() == "poster");

                if (file != null && file.File != null)
                {
                    string tempName = await FileHelper.SaveFileAsync(file.File, FileType.FileTypes[FileTypeEnum.Event]);
                    dto.poster = await _attachmentsService.AddToStore(tempName, FileType.FileTypes[FileTypeEnum.Event], file.File.ContentType, dto.ConcertId.Value);
                    
                }
                else
                    throw new NoImageAndPoster();

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            await _concertService.CreateConcert(dto);
            return Ok(new { message = "successfull" });
            
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
                {
                    string tempName = await FileHelper.SaveFileAsync(file.File, FileType.FileTypes[FileTypeEnum.Event]);
                    dto.img_Url = await _attachmentsService.AddToStore(tempName, FileType.FileTypes[FileTypeEnum.Event], file.File.ContentType, dto.ConcertId.Value);
                }
                else
                    dto.img_Url = null;

                file = Files.Files.FirstOrDefault(x => x.Key.ToLower() == "poster");

                if (file != null && file.File != null)
                {
                    string tempName = await FileHelper.SaveFileAsync(file.File, FileType.FileTypes[FileTypeEnum.Event]);
                    dto.poster = await _attachmentsService.AddToStore(tempName, FileType.FileTypes[FileTypeEnum.Event], file.File.ContentType, dto.ConcertId.Value);
                }
                else
                    dto.poster = null;

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            await _concertService.UpdateConcert(dto);

            return Ok(new {message = "successfull"});
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



        /// <summary>
        /// اطلاعات کلی کنسرت
        /// </summary>
        /// <param name="ConcertId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{ConcertId}")]
        public async Task<IActionResult> GetConcertInfo(Guid ConcertId)
        {
            return Ok(await _concertService.GetConcertByConcertId(ConcertId));
        }

        /// <summary>
        ///  ویرایش اطلاعات کلی کنسرت
        /// </summary>
        /// <param name="Files"></param>
        /// <param name="Concert"></param>
        /// <returns></returns>
        /// <exception cref="NoImageAndPoster"></exception>
        [HttpPatch("[action]")]
        public async Task<IActionResult> UpdateConcertInfo( [FromForm] String Concert)
        {

            glbCreationConcertDto dto = System.Text.Json.JsonSerializer.Deserialize<glbCreationConcertDto>(Concert, new System.Text.Json.JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            }) ?? new();

         

            await _concertService.UpdateConcertInfo(dto);

            return Ok(new { message = "successfull" });
        }





        [HttpGet("[action]/{Id}")]
        public async Task<IActionResult> GetDiscountById(Guid Id)
        {
            var res = await _discount.GetById(Id);
            return Ok(res);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddDiscount([FromBody] DiscountDto dto)
        {
           await _discount.Add(dto);
            return Ok(new {Message = "OK"});
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateDiscount([FromBody] DiscountDto dto)
        {
            await _discount.Update(dto);
            return Ok(new { Message = "OK" });
        }

        [HttpGet("[action]/{Id}")]
        public async Task<IActionResult> ToggleDiscount(Guid Id)
        {
            var res = await _discount.ToggleDiscount(Id);
            return Ok(res);
        }

    }
}
