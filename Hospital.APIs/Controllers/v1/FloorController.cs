using AutoMapper;
using Hospital.DataAccess.Repository.IRepository;
using Hospital.Models.Dto.FloorDto;
using Hospital.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.APIs.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class FloorController : ControllerBase
    {
        private readonly ILogger<FloorController> logger;
        private readonly IFloorRepository floorRepository;
        private readonly IMapper mapper;

        public FloorController(ILogger<FloorController> logger, IFloorRepository floorRepository, IMapper mapper)
        {
            this.logger = logger;
            this.floorRepository = floorRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetFloorByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Floor>> GetFloorByIdAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var floor = await floorRepository.Get(x => x.Id == id,includeProperties: "Building");

                if (floor == null)
                {
                    return NotFound($"No floor exists with Id = {id}");
                }

                return Ok(floor);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("all", Name = "GetAllFloorsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Floor>>> GetAllFloorsAsync()
        {
            try
            {
                var floors = await floorRepository.GetAll(includeProperties: "Building");

                if (floors == null || !floors.Any())
                {
                    return NotFound("No floors exist");
                }

                return Ok(floors);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost(Name = "CreateFloorAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Floor>> CreateFloorAsync([FromBody] FloorCreateDto floorCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid");
            }

            try
            {
                var floor = mapper.Map<Floor>(floorCreateDto);
                await floorRepository.Add(floor);
                await floorRepository.Save();

                return CreatedAtRoute(nameof(GetFloorByIdAsync), new { id = floor.Id }, floor);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}", Name = "UpdateFloorAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateFloorAsync([FromRoute] int id, [FromBody] FloorUpdateDto floorUpdateDto)
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
                var floorFromDb = await floorRepository.Get(x => x.Id == id);

                if (floorFromDb == null)
                {
                    return NotFound($"No floor exists with Id = {id}");
                }

                var floor = mapper.Map(floorUpdateDto, floorFromDb);

                floorRepository.Update(floor);
                await floorRepository.Save();

                return Ok("Floor was updated successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}", Name = "DeleteFloorAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteFloorAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var floorFromDb = await floorRepository.Get(x => x.Id == id);

                if (floorFromDb == null)
                {
                    return NotFound($"No floor exists with Id = {id}");
                }

                floorRepository.Remove(floorFromDb);
                await floorRepository.Save();

                return Ok("Floor was deleted successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }

}
