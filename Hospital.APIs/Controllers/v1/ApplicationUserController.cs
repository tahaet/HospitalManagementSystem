using AutoMapper;
using Hospital.Models.Dto.ApplicationUserDto;
using Hospital.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospital.APIs.Service.IService;

namespace Hospital.APIs.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ApplicationUserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<ApplicationUserController> logger;
        private readonly IMapper mapper;
        private readonly IAuthService authService;

        public ApplicationUserController(UserManager<ApplicationUser> userManager, ILogger<ApplicationUserController> logger, IMapper mapper ,
            IAuthService authService)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.mapper = mapper;
            this.authService = authService;
        }

        [HttpGet("{id}", Name = "GetApplicationUserByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApplicationUser>> GetApplicationUserByIdAsync([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id must be provided");
            }

            try
            {
                var user = await userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound($"No user exists with Id = {id}");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("email", Name = "GetApplicationUserByEmailAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApplicationUser>> GetApplicationUserByEmailAsync([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Id must be provided");
            }

            try
            {
                var user = await userManager.FindByEmailAsync(email.ToLower());

                if (user == null)
                {
                    return NotFound($"No user exists with email = {email}");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("all", Name = "GetAllApplicationUsersAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAllApplicationUsersAsync()
        {
            try
            {
                var users = await userManager.Users.ToListAsync();

                if (users == null || users.Count == 0)
                {
                    return NotFound("No users exist");
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost(Name = "CreateApplicationUserAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApplicationUser>> CreateApplicationUserAsync([FromBody] ApplicationUserCreateDto userCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid");
            }

            try
            {
                var user = new ApplicationUser
                {
                    UserName = userCreateDto.Email,
                    Email = userCreateDto.Email,
                    Name = userCreateDto.Name,
                    About = userCreateDto.About,
                    Details = userCreateDto.Details
                };

                var result = await userManager.CreateAsync(user, userCreateDto.Password);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                await authService.AssignRole(user.Email, userCreateDto.Role);
                return CreatedAtRoute(nameof(GetApplicationUserByEmailAsync), new { email = user.Email });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}", Name = "UpdateApplicationUserAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateApplicationUserAsync([FromRoute] string id, [FromBody] ApplicationUserUpdateDto userUpdateDto)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id must be provided");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid");
            }

            try
            {
                var userFromDb = await userManager.FindByIdAsync(id);

                if (userFromDb == null)
                {
                    return NotFound($"No user exists with Id = {id}");
                }

                userFromDb.Name = userUpdateDto.Name;
                userFromDb.About = userUpdateDto.About;
                userFromDb.Details = userUpdateDto.Details;
                userFromDb.Email = userUpdateDto.Email;
                userFromDb.UserName = userUpdateDto.Email;

                var result = await userManager.UpdateAsync(userFromDb);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                var success =  await authService.AssignRole(userUpdateDto.Email,userUpdateDto.Role);
                
                if (success)
                {
                    await userManager.RemoveFromRoleAsync(userFromDb, userManager.GetRolesAsync(userFromDb).Result.FirstOrDefault());
                }
                return Ok("User was updated successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}", Name = "DeleteApplicationUserAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteApplicationUserAsync([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id must be provided");
            }

            try
            {
                var userFromDb = await userManager.FindByIdAsync(id);

                if (userFromDb == null)
                {
                    return NotFound($"No user exists with Id = {id}");
                }

                var result = await userManager.DeleteAsync(userFromDb);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                return Ok("User was deleted successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }

}
