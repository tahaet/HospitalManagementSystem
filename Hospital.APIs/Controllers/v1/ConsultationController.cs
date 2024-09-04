using AutoMapper;
using Hospital.DataAccess.Repository.IRepository;
using Hospital.Models;
using Hospital.Models.Dto.ConsultaionDto;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.APIs.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ConsultationController : ControllerBase
    {
        private readonly ILogger<ConsultationController> _logger;
        private readonly IConsultationRepository _consultationRepository;
        private readonly IMapper _mapper;

        public ConsultationController(
            ILogger<ConsultationController> logger,
            IConsultationRepository consultationRepository,
            IMapper mapper)
        {
            _logger = logger;
            _consultationRepository = consultationRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new consultation.
        /// </summary>
        /// <param name="consultationCreateDto">The consultation details.</param>
        /// <returns>The created consultation.</returns>
        [HttpPost(Name = "CreateConsultationAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Consultation>> CreateConsultationAsync([FromBody] ConsultationCreateDto consultationCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            try
            {
                var consultation = _mapper.Map<Consultation>(consultationCreateDto);
                await _consultationRepository.Add(consultation);
                await _consultationRepository.Save();

                
                return CreatedAtRoute(nameof(GetConsultationByIdAsync), new { id = consultation.Id }, consultation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating consultation");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves a consultation by ID.
        /// </summary>
        /// <param name="id">The consultation ID.</param>
        /// <returns>The consultation details.</returns>
        [HttpGet("{id}", Name = "GetConsultationByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Consultation>> GetConsultationByIdAsync([FromRoute] int id)
        {
            if (id < 1)
                return BadRequest("Id must be greater than 0");

            try
            {
                var consultation = await _consultationRepository.Get(x => x.Id == id, includeProperties: "Patient,ConsultCategory,Doctor");
                if (consultation == null)
                    return NotFound($"No consultation found with Id = {id}");

               
                return Ok(consultation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving consultation");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves all consultations.
        /// </summary>
        /// <returns>The list of all consultations.</returns>
        [HttpGet("all", Name = "GetAllConsultationsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Consultation>>> GetAllConsultationsAsync()
        {
            try
            {
                var consultations = await _consultationRepository.GetAll(includeProperties: "Patient,ConsultCategory,Doctor");
                if (consultations == null || !consultations.Any())
                    return NotFound("No consultations exist");

                
                return Ok(consultations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving consultations");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates a consultation.
        /// </summary>
        /// <param name="id">The consultation ID.</param>
        /// <param name="consultationUpdateDto">The updated consultation details.</param>
        /// <returns>The result of the update operation.</returns>
        [HttpPut("{id}", Name = "UpdateConsultationAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateConsultationAsync([FromRoute] int id, [FromBody] ConsultationUpdateDto consultationUpdateDto)
        {
            if (id < 1)
                return BadRequest("Id must be greater than 0");

            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            try
            {
                var consultationFromDb = await _consultationRepository.Get(x => x.Id == id);
                if (consultationFromDb == null)
                    return NotFound($"No consultation found with Id = {id}");

                var updatedConsultation = _mapper.Map(consultationUpdateDto, consultationFromDb);
                 _consultationRepository.Update(updatedConsultation);
                await _consultationRepository.Save();

                return Ok("Consultation updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating consultation");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes a consultation.
        /// </summary>
        /// <param name="id">The consultation ID.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete("{id}", Name = "DeleteConsultationAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteConsultationAsync([FromRoute] int id)
        {
            if (id < 1)
                return BadRequest("Id must be greater than 0");

            try
            {
                var consultationFromDb = await _consultationRepository.Get(x => x.Id == id);
                if (consultationFromDb == null)
                    return NotFound($"No consultation found with Id = {id}");

                 _consultationRepository.Remove(consultationFromDb);
                await _consultationRepository.Save();

                return Ok("Consultation deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting consultation");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

