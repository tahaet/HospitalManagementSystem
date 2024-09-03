using AutoMapper;
using Hospital.DataAccess.Repository.IRepository;
using Hospital.Models;
using Hospital.Models.Dto.BuildingDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.APIs.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BuildingController : ControllerBase
    {
        private readonly ILogger<BuildingController> logger;
        private readonly IBuildingRepository buildingRepository;
        private readonly IMapper mapper;

        public BuildingController(ILogger<BuildingController> logger,IBuildingRepository buildingRepository,IMapper mapper)
        {
            this.logger = logger;
            this.buildingRepository = buildingRepository;
            this.mapper = mapper;
        }



        [HttpGet("{id}",Name = "GetBuildingByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Building>> GetBuildingByIdAsync( [FromRoute] int id)
        {

            if(id < 1 )
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var building = await buildingRepository.Get(x => x.Id == id);

                if(building == null)
                {
                    return NotFound($"No building exists with Id = {id}");
                }

                return Ok(building);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("name",Name = "GetBuildingByNameAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Building>> GetBuildingByNameAsync([FromQuery] string name)
        {

            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name must have a value");
            }

            try
            {
                var building = await buildingRepository.Get(x => x.Name == name);

                if (building == null)
                {
                    return NotFound($"No building exists with Name = {name}");
                }

                return Ok(building);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("code", Name = "GetBuildingByCodeAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Building>> GetBuildingByCodeAsync([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("Code must have a value");
            }

            try
            {
                var building = await buildingRepository.Get(x => x.Code == code);

                if (building == null)
                {
                    return NotFound($"No building exists with code = {code}");
                }

                return Ok(building);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("all",Name = "GeAllBuildingsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Building>>> GeAllBuildingsAsync()
        {

            try
            {
                var buildings = await buildingRepository.GetAll();

                if (buildings == null)
                {
                    return NotFound($"No building exist");
                }

                return Ok(buildings);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost(Name = "CreateBuildingAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Building>> CreateBuildingAsync([FromBody] BuildingCreateDto buildingCreateDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("model is not valid");
            }

            try
            {
                var building = mapper.Map<Building>(buildingCreateDto);
                if (building == null)
                {
                    return StatusCode(500, "Internal server error");
                }
                await buildingRepository.Add(building);
                await buildingRepository.Save();
               
                return CreatedAtRoute(nameof(GetBuildingByIdAsync), new { id = building.Id},building);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}", Name = "UpdateBuildingAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateBuildingAsync([FromRoute] int id, [FromBody] BuildingUpdateDto buildingUpdateDto)
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

                var buildingFromDb = await buildingRepository.Get(x => x.Id == id);


                if (buildingFromDb == null)
                {
                    return NotFound($"No building exists with Id = {id}");
                }
                var building = mapper.Map<Building>(buildingUpdateDto);
                
                if(building == null)
                {
                    return StatusCode(500, "Internal server error");
                }

                buildingRepository.Update(building);
                await buildingRepository.Save();
                return Ok("Model was updated successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}", Name = "DeleteBuildingAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteBuildingAsync([FromRoute] int id)
        {

            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {

                var buildingFromDb = await buildingRepository.Get(x => x.Id == id);


                if (buildingFromDb == null)
                {
                    return NotFound($"No building exists with Id = {id}");
                }

                buildingRepository.Remove(buildingFromDb);
                await buildingRepository.Save();
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
