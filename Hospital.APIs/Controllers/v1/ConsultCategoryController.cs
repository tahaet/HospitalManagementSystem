using AutoMapper;
using Hospital.DataAccess.Repository.IRepository;
using Hospital.Models;
using Hospital.Models.Dto.ConsultCategoryDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.APIs.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ConsultCategoryController : ControllerBase
    {
        private readonly ILogger<ConsultCategoryController> logger;
        private readonly IConsultCategoryRepository consultCategoryRepository;
        private readonly IMapper mapper;

        public ConsultCategoryController(ILogger<ConsultCategoryController> logger, IConsultCategoryRepository consultCategoryRepository, IMapper mapper)
        {
            this.logger = logger;
            this.consultCategoryRepository = consultCategoryRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetConsultCategoryByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ConsultCategory>> GetConsultCategoryByIdAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var category = await consultCategoryRepository.Get(x => x.Id == id);

                if (category == null)
                {
                    return NotFound($"No consult category exists with Id = {id}");
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("name", Name = "GetConsultCategoryByNameAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ConsultCategory>> GetConsultCategoryByNameAsync([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name must have a value");
            }

            try
            {
                var category = await consultCategoryRepository.Get(x => x.Name == name);

                if (category == null)
                {
                    return NotFound($"No consult category exists with Name = {name}");
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("all", Name = "GetAllConsultCategoriesAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ConsultCategory>>> GetAllConsultCategoriesAsync()
        {
            try
            {
                var categories = await consultCategoryRepository.GetAll();

                if (categories == null || !categories.Any())
                {
                    return NotFound("No consult categories exist");
                }

                return Ok(categories);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost(Name = "CreateConsultCategoryAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ConsultCategory>> CreateConsultCategoryAsync([FromBody] ConsultCategoryCreateDto consultCategoryCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid");
            }

            try
            {
                var category = mapper.Map<ConsultCategory>(consultCategoryCreateDto);

                if (category == null)
                {
                    return StatusCode(500, "Internal server error");
                }

                await consultCategoryRepository.Add(category);
                await consultCategoryRepository.Save();

                return CreatedAtAction(nameof(GetConsultCategoryByIdAsync), new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}", Name = "UpdateConsultCategoryAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateConsultCategoryAsync([FromRoute] int id, [FromBody] ConsultCategoryUpdateDto consultCategoryUpdateDto)
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
                var categoryFromDb = await consultCategoryRepository.Get(x => x.Id == id);

                if (categoryFromDb == null)
                {
                    return NotFound($"No consult category exists with Id = {id}");
                }

                var category = mapper.Map<ConsultCategory>(consultCategoryUpdateDto);

                if (category == null)
                {
                    return StatusCode(500, "Internal server error");
                }

                consultCategoryRepository.Update(category);
                await consultCategoryRepository.Save();
                return Ok("Model was updated successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}", Name = "DeleteConsultCategoryAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteConsultCategoryAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var categoryFromDb = await consultCategoryRepository.Get(x => x.Id == id);

                if (categoryFromDb == null)
                {
                    return NotFound($"No consult category exists with Id = {id}");
                }

                consultCategoryRepository.Remove(categoryFromDb);
                await consultCategoryRepository.Save();
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
