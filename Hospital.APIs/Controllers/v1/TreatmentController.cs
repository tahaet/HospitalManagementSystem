using AutoMapper;
using Hospital.DataAccess.Repository.IRepository;
using Hospital.Models.Dto.TreatmentDTo;
using Hospital.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.APIs.Controllers.v1
{

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class TreatmentController : ControllerBase
    {
        private readonly ILogger<TreatmentController> logger;
        private readonly ITreatmentRepository treatmentRepository;
        private readonly IMapper mapper;

        public TreatmentController(ILogger<TreatmentController> logger, ITreatmentRepository treatmentRepository, IMapper mapper)
        {
            this.logger = logger;
            this.treatmentRepository = treatmentRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetTreatmentByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Treatment>> GetTreatmentByIdAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var treatment = await treatmentRepository.Get(x => x.Id == id);

                if (treatment == null)
                {
                    return NotFound($"No treatment exists with Id = {id}");
                }

                return Ok(treatment);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("name", Name = "GetTreatmentByNameAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Treatment>> GetTreatmentByNameAsync([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name must have a value");
            }

            try
            {
                var treatment = await treatmentRepository.Get(x => x.Name == name);

                if (treatment == null)
                {
                    return NotFound($"No treatment exists with Name = {name}");
                }

                return Ok(treatment);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("all", Name = "GetAllTreatmentsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Treatment>>> GetAllTreatmentsAsync()
        {
            try
            {
                var treatments = await treatmentRepository.GetAll();

                if (treatments == null || !treatments.Any())
                {
                    return NotFound("No treatments exist");
                }

                return Ok(treatments);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost(Name = "CreateTreatmentAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Treatment>> CreateTreatmentAsync([FromBody] TreatmentCreateDto treatmentCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid");
            }

            try
            {
                var treatment = mapper.Map<Treatment>(treatmentCreateDto);

                if (treatment == null)
                {
                    return StatusCode(500, "Internal server error");
                }

                await treatmentRepository.Add(treatment);
                await treatmentRepository.Save();

                return CreatedAtAction(nameof(GetTreatmentByIdAsync), new { id = treatment.Id }, treatment);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}", Name = "UpdateTreatmentAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateTreatmentAsync([FromRoute] int id, [FromBody] TreatmentUpdateDto treatmentUpdateDto)
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
                var treatmentFromDb = await treatmentRepository.Get(x => x.Id == id);

                if (treatmentFromDb == null)
                {
                    return NotFound($"No treatment exists with Id = {id}");
                }

                var treatment = mapper.Map<Treatment>(treatmentUpdateDto);

                if (treatment == null)
                {
                    return StatusCode(500, "Internal server error");
                }

                treatmentRepository.Update(treatment);
                await treatmentRepository.Save();
                return Ok("Model was updated successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}", Name = "DeleteTreatmentAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTreatmentAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var treatmentFromDb = await treatmentRepository.Get(x => x.Id == id);

                if (treatmentFromDb == null)
                {
                    return NotFound($"No treatment exists with Id = {id}");
                }

                treatmentRepository.Remove(treatmentFromDb);
                await treatmentRepository.Save();
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
