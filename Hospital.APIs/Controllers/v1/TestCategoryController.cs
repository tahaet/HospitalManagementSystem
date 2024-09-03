using AutoMapper;
using Hospital.DataAccess.Repository.IRepository;
using Hospital.Models.Dto.TestCategoryDto;
using Hospital.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.APIs.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class TestCategoryController : ControllerBase
    {
        private readonly ILogger<TestCategoryController> logger;
        private readonly ITestCategoryRepository testCategoryRepository;
        private readonly IMapper mapper;

        public TestCategoryController(ILogger<TestCategoryController> logger, ITestCategoryRepository testCategoryRepository, IMapper mapper)
        {
            this.logger = logger;
            this.testCategoryRepository = testCategoryRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetTestCategoryByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TestCategory>> GetTestCategoryByIdAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var testCategory = await testCategoryRepository.Get(x => x.Id == id);

                if (testCategory == null)
                {
                    return NotFound($"No test category exists with Id = {id}");
                }

                return Ok(testCategory);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("all", Name = "GetAllTestCategoriesAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<TestCategory>>> GetAllTestCategoriesAsync()
        {
            try
            {
                var testCategories = await testCategoryRepository.GetAll();

                if (testCategories == null)
                {
                    return NotFound("No test categories exist");
                }

                return Ok(testCategories);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost(Name = "CreateTestCategoryAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TestCategory>> CreateTestCategoryAsync([FromBody] TestCategoryCreateDto testCategoryCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid");
            }

            try
            {
                var testCategory = mapper.Map<TestCategory>(testCategoryCreateDto);

                if (testCategory == null)
                {
                    return StatusCode(500, "Internal server error");
                }

                await testCategoryRepository.Add(testCategory);
                await testCategoryRepository.Save();

                return CreatedAtRoute(nameof(GetTestCategoryByIdAsync), new { id = testCategory.Id }, testCategory);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}", Name = "UpdateTestCategoryAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateTestCategoryAsync([FromRoute] int id, [FromBody] TestCategoryUpdateDto testCategoryUpdateDto)
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
                var testCategoryFromDb = await testCategoryRepository.Get(x => x.Id == id);

                if (testCategoryFromDb == null)
                {
                    return NotFound($"No test category exists with Id = {id}");
                }

                var testCategory = mapper.Map(testCategoryUpdateDto, testCategoryFromDb);

                if (testCategory == null)
                {
                    return StatusCode(500, "Internal server error");
                }

                testCategoryRepository.Update(testCategory);
                await testCategoryRepository.Save();
                return Ok("Model was updated successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}", Name = "DeleteTestCategoryAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTestCategoryAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var testCategoryFromDb = await testCategoryRepository.Get(x => x.Id == id);

                if (testCategoryFromDb == null)
                {
                    return NotFound($"No test category exists with Id = {id}");
                }

                testCategoryRepository.Remove(testCategoryFromDb);
                await testCategoryRepository.Save();
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
