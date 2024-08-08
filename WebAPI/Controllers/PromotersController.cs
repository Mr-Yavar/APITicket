using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.Enum;
using Entities.Models;
using Entities.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Services;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotersController : ControllerBase
    {

        private IPromoterService _promoterService;
        private IAttachmentsService _attachmentsService;
        public PromotersController(IPromoterService promoterService,IAttachmentsService attachmentsService)
        {
            _promoterService = promoterService;
            _attachmentsService = attachmentsService;


        }

        /// <summary>
        /// شطح ادمین
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllPromoters()
        {
            var result = await _promoterService.GetAllPromoters();
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetPromoters()
        {
            var result =await _promoterService.GetPromoters();
            return Ok(result);
        }

        /// <summary>
        /// گرفتن  مکان  برگزاری
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetPromoter(Guid id)
        {
            return Ok(await _promoterService.GetPromoter(id));

        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreatePromoter([FromForm] FileDto File ,[FromForm] String Promoter)
        {

            PromoterDto dto = System.Text.Json.JsonSerializer.Deserialize<PromoterDto>(Promoter);
            dto.Id = Guid.NewGuid();
            try
            {
                if (File != null && File.File != null)
                {

                    string tempName = await FileHelper.SaveFileAsync(File.File, FileType.FileTypes[FileTypeEnum.Promoter]);

                    dto.img_url = await _attachmentsService.AddToStore(tempName, FileType.FileTypes[FileTypeEnum.Promoter], File.File.ContentType, dto.Id.Value);

                
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


            if (await _promoterService.ProcessCreationPromoter(dto))
                return Ok(dto);
            return BadRequest();

        
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdatePromoter([FromForm] FileDto file, [FromForm] String Promoter)
        {

            PromoterDto dto = System.Text.Json.JsonSerializer.Deserialize<PromoterDto>(Promoter);
            try
            {
                if (file != null && file.File != null)
                {
                    string tempName = await FileHelper.SaveFileAsync(file.File, FileType.FileTypes[FileTypeEnum.Promoter]);

                    dto.img_url = await _attachmentsService.AddToStore(tempName, FileType.FileTypes[FileTypeEnum.Promoter], file.File.ContentType, dto.Id.Value);
                   
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


            if (await _promoterService.ProcessUpdatePromoter(dto))
                return Ok(dto);
            return BadRequest();


        }

        [HttpGet("[action]/{PromoterId}")]
        public async Task<IActionResult> TogglePromoter(Guid PromoterId)
        {
            await _promoterService.TogglePromoter(PromoterId);
            return Ok(new { status = 200 });
        }


    }
}
