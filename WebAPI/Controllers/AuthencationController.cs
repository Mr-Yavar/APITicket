using Contracts.IService;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using WebAPI.ActionFilters;

namespace WebAPI.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _Authenticationservice;
        public AuthenticationController(IAuthenticationService Authenticationservice) => _Authenticationservice = Authenticationservice;

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var result = await _Authenticationservice.RegisterUser(userForRegistration);

            if (!result.Succeeded) {
                foreach (var error in result.Errors) {
                    ModelState.TryAddModelError(error.Code, error.Description);
                } 
                return BadRequest(ModelState);
            }
            return StatusCode(201);
        }

    }
}
