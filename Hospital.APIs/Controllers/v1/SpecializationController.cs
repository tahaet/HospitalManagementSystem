using AutoMapper;
using Hospital.DataAccess.Repository.IRepository;
using Hospital.Models.Dto.Specialization;
using Hospital.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.APIs.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class SpecializationController : ControllerBase
    {
        private readonly ILogger<SpecializationController> logger;
        private readonly ISpecializationRepository specializationRepository;
        private readonly IMapper mapper;

        public SpecializationController(ILogger<SpecializationController> logger, ISpecializationRepository specializationRepository, IMapper mapper)
        {
            this.logger = logger;
            this.specializationRepository = specializationRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetSpecializationByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Specialization>> GetSpecializationByIdAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var specialization = await specializationRepository.Get(x => x.Id == id);

                if (specialization == null)
                {
                    return NotFound($"No specialization exists with Id = {id}");
                }

                return Ok(specialization);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("all", Name = "GetAllSpecializationsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Specialization>>> GetAllSpecializationsAsync()
        {
            try
            {
                var specializations = await specializationRepository.GetAll();

                if (specializations == null)
                {
                    return NotFound("No specializations exist");
                }

                return Ok(specializations);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost(Name = "CreateSpecializationAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Specialization>> CreateSpecializationAsync([FromBody] SpecializationCreateDto specializationCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid");
            }

            try
            {
                var specialization = mapper.Map<Specialization>(specializationCreateDto);
                if (specialization == null)
                {
                    return StatusCode(500, "Internal server error");
                }
                await specializationRepository.Add(specialization);
                await specializationRepository.Save();

                return CreatedAtRoute(nameof(GetSpecializationByIdAsync), new { id = specialization.Id }, specialization);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}", Name = "UpdateSpecializationAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateSpecializationAsync([FromRoute] int id, [FromBody] SpecializationUpdateDto specializationUpdateDto)
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
                var specializationFromDb = await specializationRepository.Get(x => x.Id == id);

                if (specializationFromDb == null)
                {
                    return NotFound($"No specialization exists with Id = {id}");
                }

                mapper.Map(specializationUpdateDto, specializationFromDb);
                specializationRepository.Update(specializationFromDb);
                await specializationRepository.Save();

                return Ok("Specialization was updated successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}", Name = "DeleteSpecializationAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteSpecializationAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var specializationFromDb = await specializationRepository.Get(x => x.Id == id);

                if (specializationFromDb == null)
                {
                    return NotFound($"No specialization exists with Id = {id}");
                }

                specializationRepository.Remove(specializationFromDb);
                await specializationRepository.Save();

                return Ok("Specialization was deleted successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }

}