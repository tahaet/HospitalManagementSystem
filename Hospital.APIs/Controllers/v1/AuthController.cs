using Hospital.APIs.Service.IService;
using Hospital.Models.Dto.AuthDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hospital.APIs.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly ILogger<AuthController> logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            this.authService = authService;
            this.logger = logger;
        }

        [HttpPost("login", Name = "Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Model is not valid");
            }

            try
            {
                var loginResponseDto = await authService.Login(loginRequestDto);
                if (loginResponseDto.User is null)
                {
                   return NotFound("Email or password is not correct");
                }
                
                return Ok(loginResponseDto);

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("register", Name = "Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid");
            }

            try
            {
                var error = await authService.Register(registrationRequestDto);
                
                if (!string.IsNullOrEmpty(error))
                {
                    return BadRequest(registrationRequestDto);
                }

                return Ok("User was registered successfully");

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("AssignRole",Name = "AssignRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignRole( [FromQuery] string email, [FromQuery] string role)
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role))
            {
                return BadRequest("Email or role is not valid");
            }

            try
            {
                var success = await authService.AssignRole(email, role.ToUpper());
                if (!success)
                {

                    return NotFound("Email or role does not exist");

                }
                return Ok("Role was assigned to user successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }

        }
    }
}
