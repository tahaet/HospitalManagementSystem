using AutoMapper;
using Hospital.DataAccess.Repository.IRepository;
using Hospital.Models;
using Hospital.Models.Dto.PatientDetailsDto;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.APIs.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class PatientDetailsController : ControllerBase
    {
        private readonly ILogger<PatientDetailsController> logger;
        private readonly IPatientDetailsRepository patientDetailsRepository;
        private readonly IMapper mapper;

        public PatientDetailsController(ILogger<PatientDetailsController> logger, IPatientDetailsRepository patientDetailsRepository, IMapper mapper)
        {
            this.logger = logger;
            this.patientDetailsRepository = patientDetailsRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetPatientDetailsByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatientDetails>> GetPatientDetailsByIdAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var patientDetails = await patientDetailsRepository.Get(x => x.Id == id, includeProperties: "User");
                if (patientDetails == null)
                {
                    return NotFound($"No patient details exist with Id = {id}");
                }
                return Ok(patientDetails);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("user/{userId}", Name = "GetPatientDetailsByUserIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PatientDetails>>> GetPatientDetailsByUserIdAsync([FromRoute] string userId)
        {
            try
            {
                var patientDetails = await patientDetailsRepository.GetAll(x => x.UserId == userId, includeProperties: "User");
                if (patientDetails == null || !patientDetails.Any())
                {
                    return NotFound("No patient details found for the user");
                }
                return Ok(patientDetails);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("all", Name = "GetAllPatientDetailsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PatientDetails>>> GetAllPatientDetailsAsync()
        {
            try
            {
                var patientDetails = await patientDetailsRepository.GetAll(includeProperties: "User");
                if (patientDetails == null || !patientDetails.Any())
                {
                    return NotFound("No patient details exist");
                }
                return Ok(patientDetails);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost(Name = "CreatePatientDetailsAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PatientDetails>> CreatePatientDetailsAsync([FromBody] PatientDetailsCreateDto patientDetailsCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid");
            }

            try
            {
                var patientDetails = mapper.Map<PatientDetails>(patientDetailsCreateDto);
                if (patientDetails == null)
                {
                    return StatusCode(500, "Internal server error");
                }

                await patientDetailsRepository.Add(patientDetails);
                await patientDetailsRepository.Save();
                return CreatedAtRoute(nameof(GetPatientDetailsByIdAsync), new { id = patientDetails.Id }, patientDetails);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}", Name = "UpdatePatientDetailsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdatePatientDetailsAsync([FromRoute] int id, [FromBody] PatientDetailsUpdateDto patientDetailsUpdateDto)
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
                var patientDetailsFromDb = await patientDetailsRepository.Get(x => x.Id == id);
                if (patientDetailsFromDb == null)
                {
                    return NotFound($"No patient details exist with Id = {id}");
                }

                var patientDetails = mapper.Map(patientDetailsUpdateDto, patientDetailsFromDb);
                if (patientDetails == null)
                {
                    return StatusCode(500, "Internal server error");
                }

                patientDetailsRepository.Update(patientDetails);
                await patientDetailsRepository.Save();
                return Ok("Model was updated successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}", Name = "DeletePatientDetailsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeletePatientDetailsAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var patientDetailsFromDb = await patientDetailsRepository.Get(x => x.Id == id);
                if (patientDetailsFromDb == null)
                {
                    return NotFound($"No patient details exist with Id = {id}");
                }

                patientDetailsRepository.Remove(patientDetailsFromDb);
                await patientDetailsRepository.Save();
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
