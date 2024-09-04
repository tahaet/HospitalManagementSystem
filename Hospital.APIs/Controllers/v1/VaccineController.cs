using AutoMapper;
using Hospital.DataAccess.Repository.IRepository;
using Hospital.Models;
using Hospital.Models.Dto.VaccineDto;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.APIs.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class VaccineController : ControllerBase
    {
        private readonly ILogger<VaccineController> _logger;
        private readonly IVaccineRepository _vaccineRepository;
        private readonly IMapper _mapper;

        public VaccineController(
            ILogger<VaccineController> logger,
            IVaccineRepository vaccineRepository,
            IMapper mapper)
        {
            _logger = logger;
            _vaccineRepository = vaccineRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new vaccine.
        /// </summary>
        /// <param name="vaccineCreateDto">The vaccine details.</param>
        /// <returns>The created vaccine.</returns>
        [HttpPost(Name = "CreateVaccineAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Vaccine>> CreateVaccineAsync([FromBody] VaccineCreateDto vaccineCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            try
            {
                var vaccine = _mapper.Map<Vaccine>(vaccineCreateDto);
                await _vaccineRepository.Add(vaccine);
                await _vaccineRepository.Save();

                return CreatedAtRoute(nameof(GetVaccineByIdAsync), new { id = vaccine.Id }, vaccine);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vaccine");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves a vaccine by ID.
        /// </summary>
        /// <param name="id">The vaccine ID.</param>
        /// <returns>The vaccine details.</returns>
        [HttpGet("{id}", Name = "GetVaccineByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Vaccine>> GetVaccineByIdAsync([FromRoute] int id)
        {
            if (id < 1)
                return BadRequest("Id must be greater than 0");

            try
            {
                var vaccine = await _vaccineRepository.Get(x=>x.Id == id, includeProperties: "Medicine");
                if (vaccine == null)
                    return NotFound($"No vaccine found with Id = {id}");

                return Ok(vaccine);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccine");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves all vaccines.
        /// </summary>
        /// <returns>The list of all vaccines.</returns>
        [HttpGet("all", Name = "GetAllVaccinesAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Vaccine>>> GetAllVaccinesAsync()
        {
            try
            {
                var vaccines = await _vaccineRepository.GetAll(includeProperties: "Medicine");
                if (vaccines == null || !vaccines.Any())
                    return NotFound("No vaccines exist");

                return Ok(vaccines);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccines");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates a vaccine.
        /// </summary>
        /// <param name="id">The vaccine ID.</param>
        /// <param name="vaccineUpdateDto">The updated vaccine details.</param>
        /// <returns>The result of the update operation.</returns>
        [HttpPut("{id}", Name = "UpdateVaccineAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateVaccineAsync([FromRoute] int id, [FromBody] VaccineUpdateDto vaccineUpdateDto)
        {
            if (id < 1)
                return BadRequest("Id must be greater than 0");

            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            try
            {
                var vaccineFromDb = await _vaccineRepository.Get(x => x.Id == id);
                if (vaccineFromDb == null)
                    return NotFound($"No vaccine found with Id = {id}");

                var updatedVaccine = _mapper.Map(vaccineUpdateDto, vaccineFromDb);
                _vaccineRepository.Update(updatedVaccine);
                await _vaccineRepository.Save();

                return Ok("Vaccine updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vaccine");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes a vaccine.
        /// </summary>
        /// <param name="id">The vaccine ID.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete("{id}", Name = "DeleteVaccineAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteVaccineAsync([FromRoute] int id)
        {
            if (id < 1)
                return BadRequest("Id must be greater than 0");

            try
            {
                var vaccineFromDb = await _vaccineRepository.Get(x=>x.Id == id);
                if (vaccineFromDb == null)
                    return NotFound($"No vaccine found with Id = {id}");

                 _vaccineRepository.Remove(vaccineFromDb);
                await _vaccineRepository.Save();

                return Ok("Vaccine deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vaccine");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
