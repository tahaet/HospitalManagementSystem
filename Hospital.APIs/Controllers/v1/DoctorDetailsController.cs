using AutoMapper;
using Hospital.DataAccess.Repository.IRepository;
using Hospital.Models;
using Hospital.Models.Dto.DoctorDetailsDto;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.APIs.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class DoctorDetailsController : ControllerBase
    {
        private readonly ILogger<DoctorDetailsController> logger;
        private readonly IDoctorDetailsRepository doctorDetailsRepository;
        private readonly IMapper mapper;

        public DoctorDetailsController(ILogger<DoctorDetailsController> logger, IDoctorDetailsRepository doctorDetailsRepository, IMapper mapper)
        {
            this.logger = logger;
            this.doctorDetailsRepository = doctorDetailsRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetDoctorDetailsByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DoctorDetails>> GetDoctorDetailsByIdAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var doctorDetails = await doctorDetailsRepository.Get(x => x.Id == id, includeProperties: "User,Specialization,Designation,Department");
                if (doctorDetails == null)
                {
                    return NotFound($"No doctor details exist with Id = {id}");
                }
                return Ok(doctorDetails);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("all", Name = "GetAllDoctorDetailsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<DoctorDetails>>> GetAllDoctorDetailsAsync()
        {
            try
            {
                var doctorDetails = await doctorDetailsRepository.GetAll(includeProperties: "User,Specialization,Designation,Department");
                if (doctorDetails == null || !doctorDetails.Any())
                {
                    return NotFound("No doctor details exist");
                }
                return Ok(doctorDetails);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost(Name = "CreateDoctorDetailsAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DoctorDetails>> CreateDoctorDetailsAsync([FromBody] DoctorCreateDto doctorDetailsCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid");
            }

            try
            {
                var doctorDetails = mapper.Map<DoctorDetails>(doctorDetailsCreateDto);
                if (doctorDetails == null)
                {
                    return StatusCode(500, "Internal server error");
                }

                await doctorDetailsRepository.Add(doctorDetails);
                await doctorDetailsRepository.Save();
                return CreatedAtRoute(nameof(GetDoctorDetailsByIdAsync), new { id = doctorDetails.Id }, doctorDetails);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}", Name = "UpdateDoctorDetailsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateDoctorDetailsAsync([FromRoute] int id, [FromBody] DoctorUpdateDto doctorDetailsUpdateDto)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid");
            }

            try
            {
                var doctorDetailsFromDb = await doctorDetailsRepository.Get(x => x.Id == id);
                if (doctorDetailsFromDb == null)
                {
                    return NotFound($"No doctor details exist with Id = {id}");
                }

                var doctorDetails = mapper.Map(doctorDetailsUpdateDto, doctorDetailsFromDb);
                if (doctorDetails == null)
                {
                    return StatusCode(500, "Internal server error");
                }

                doctorDetailsRepository.Update(doctorDetails);
                await doctorDetailsRepository.Save();
                return Ok("Model was updated successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}", Name = "DeleteDoctorDetailsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteDoctorDetailsAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var doctorDetailsFromDb = await doctorDetailsRepository.Get(x => x.Id == id);
                if (doctorDetailsFromDb == null)
                {
                    return NotFound($"No doctor details exist with Id = {id}");
                }

                doctorDetailsRepository.Remove(doctorDetailsFromDb);
                await doctorDetailsRepository.Save();
                return Ok("Model was deleted successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }

}
