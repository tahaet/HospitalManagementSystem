using AutoMapper;
using Hospital.DataAccess.Repository.IRepository;
using Hospital.Models.Dto.TestResultDto;
using Hospital.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.APIs.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TestResultController : ControllerBase
    {
        private readonly ILogger<TestResultController> _logger;
        private readonly ITestResultRepository _testResultRepository;
        private readonly IMapper _mapper;

        public TestResultController(
            ILogger<TestResultController> logger,
            ITestResultRepository testResultRepository,
            IMapper mapper)
        {
            _logger = logger;
            _testResultRepository = testResultRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new test result.
        /// </summary>
        /// <param name="createDto">The test result details.</param>
        /// <returns>The created test result.</returns>
        [HttpPost(Name = "CreateTestResultAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TestResult>> CreateTestResultAsync([FromBody] TestResultCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            try
            {
                var testResult = _mapper.Map<TestResult>(createDto);
                await _testResultRepository.Add(testResult);
                await _testResultRepository.Save();

                return CreatedAtRoute(nameof(GetTestResultByIdAsync), new { id = testResult.Id }, testResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating test result");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves a test result by ID.
        /// </summary>
        /// <param name="id">The test result ID.</param>
        /// <returns>The test result details.</returns>
        [HttpGet("{id}", Name = "GetTestResultByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TestResult>> GetTestResultByIdAsync([FromRoute] int id)
        {
            if (id < 1)
                return BadRequest("Id must be greater than 0");

            try
            {
                var testResult = await _testResultRepository.Get(x=>x.Id == id, includeProperties: "User,Test");
                if (testResult == null)
                    return NotFound($"No test result found with Id = {id}");

                return Ok(testResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving test result");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves all test results.
        /// </summary>
        /// <returns>The list of all test results.</returns>
        [HttpGet("all", Name = "GetAllTestResultsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TestResult>>> GetAllTestResultsAsync()
        {
            try
            {
                var testResults = await _testResultRepository.GetAll(includeProperties: "User,Test");
                if (testResults == null || !testResults.Any())
                    return NotFound("No test results exist");

                return Ok(testResults);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving test results");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates a test result.
        /// </summary>
        /// <param name="id">The test result ID.</param>
        /// <param name="updateDto">The updated test result details.</param>
        /// <returns>The result of the update operation.</returns>
        [HttpPut("{id}", Name = "UpdateTestResultAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateTestResultAsync([FromRoute] int id, [FromBody] TestResultUpdateDto updateDto)
        {
            if (id < 1)
                return BadRequest("Id must be greater than 0");

            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            try
            {
                var testResultFromDb = await _testResultRepository.Get(x=>x.Id == id);
                if (testResultFromDb == null)
                    return NotFound($"No test result found with Id = {id}");

                var updatedTestResult = _mapper.Map(updateDto, testResultFromDb);
                _testResultRepository.Update(updatedTestResult);
                await _testResultRepository.Save();

                return Ok("Test result updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating test result");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes a test result.
        /// </summary>
        /// <param name="id">The test result ID.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete("{id}", Name = "DeleteTestResultAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteTestResultAsync([FromRoute] int id)
        {
            if (id < 1)
                return BadRequest("Id must be greater than 0");

            try
            {
                var testResultFromDb = await _testResultRepository.Get(x=>x.Id == id);
                if (testResultFromDb == null)
                    return NotFound($"No test result found with Id = {id}");

                 _testResultRepository.Remove(testResultFromDb);
                await _testResultRepository.Save();

                return Ok("Test result deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting test result");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
