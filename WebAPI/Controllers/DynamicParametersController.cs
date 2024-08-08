using Contracts.IService;
using Entities.DataTransferObjects.DynParameters;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamicParametersController : ControllerBase
    {
        IDynParameterService _dynParameterService;
        public DynamicParametersController(IDynParameterService dynParameterService):base(){
            _dynParameterService = dynParameterService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllParameters()
        {
            var result =await _dynParameterService.GetParents();
            return Ok(result);
        }


        [HttpGet("[action]/{Id}")]
        public async Task<IActionResult> GetChildrenbyParentId(Guid Id)
        {
            var result = await _dynParameterService.GetChildrenByParentId(Id);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ManipulateParameter([FromForm]String DynParam,[FromForm] FilesDto file)
        {
            DynParametersDto dynParametersDto = System.Text.Json.JsonSerializer.Deserialize<DynParametersDto>(DynParam, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            var result = await _dynParameterService.ManipulateChildren(dynParametersDto,file);
            return Ok(result);

        }



        [HttpGet("[action]/{Id}")]
        public async Task<IActionResult> DeleteData(Guid Id)
        {
            await _dynParameterService.DeleteChild(Id);
            return Ok();    
        }



    }
}
