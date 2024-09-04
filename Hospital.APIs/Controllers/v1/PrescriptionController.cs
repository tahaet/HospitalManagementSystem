using AutoMapper;
using Hospital.DataAccess.Repository.IRepository;
using Hospital.Models.Dto.PrescriptionDto;
using Hospital.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.APIs.Controllers.v1
{

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class PrescriptionController : ControllerBase
    {
        private readonly ILogger<PrescriptionController> _logger;
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IMapper _mapper;

        public PrescriptionController(
            ILogger<PrescriptionController> logger,
            IPrescriptionRepository prescriptionRepository,
            IMapper mapper)
        {
            _logger = logger;
            _prescriptionRepository = prescriptionRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new prescription.
        /// </summary>
        /// <param name="prescriptionCreateDto">The prescription details.</param>
        /// <returns>The created prescription.</returns>
        [HttpPost(Name = "CreatePrescriptionAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Prescription>> CreatePrescriptionAsync([FromBody] PrescriptionCreateDto prescriptionCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            try
            {
                var prescription = _mapper.Map<Prescription>(prescriptionCreateDto);
                await _prescriptionRepository.Add(prescription);
                await _prescriptionRepository.Save();

                return CreatedAtRoute(nameof(GetPrescriptionDetailByIdAsync), new { id = prescription.Id }, prescription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating prescription");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves a prescription by ID.
        /// </summary>
        /// <param name="id">The prescription ID.</param>
        /// <returns>The prescription details.</returns>
        [HttpGet("{id}", Name = "GetPrescriptionDetailByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Prescription>> GetPrescriptionDetailByIdAsync([FromRoute] int id)
        {
            if (id < 1)
                return BadRequest("Id must be greater than 0");

            try
            {
                var prescription = await _prescriptionRepository.Get(x=>x.Id == id, includeProperties: "User,Treatment,Test");
                if (prescription == null)
                    return NotFound($"No prescription found with Id = {id}");

                return Ok(prescription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving prescription");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves prescriptions by user ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>The list of prescriptions for the user.</returns>
        [HttpGet("user/{userId}", Name = "GetPrescriptionsByUserIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Prescription>>> GetPrescriptionsByUserIdAsync([FromRoute] string userId)
        {
            try
            {
                var prescriptions = await _prescriptionRepository.Get(p => p.UserId == userId, includeProperties: "ApplicationUser,Treatment,Test");
                if (prescriptions == null)
                    return NotFound("No prescriptions found for the user");

                return Ok(prescriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving prescriptions for user");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves all prescriptions.
        /// </summary>
        /// <returns>The list of all prescriptions.</returns>
        [HttpGet("all", Name = "GetAllPrescriptionsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Prescription>>> GetAllPrescriptionsAsync()
        {
            try
            {
                var prescriptions = await _prescriptionRepository.GetAll(includeProperties: "User,Treatment,Test");
                if (prescriptions == null || !prescriptions.Any())
                    return NotFound("No prescriptions exist");

                return Ok(prescriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving prescriptions");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates a prescription.
        /// </summary>
        /// <param name="id">The prescription ID.</param>
        /// <param name="prescriptionUpdateDto">The updated prescription details.</param>
        /// <returns>The result of the update operation.</returns>
        [HttpPut("{id}", Name = "UpdatePrescriptionAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdatePrescriptionAsync([FromRoute] int id, [FromBody] PrescriptionUpdateDto prescriptionUpdateDto)
        {
            if (id < 1)
                return BadRequest("Id must be greater than 0");

            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            try
            {
                var prescriptionFromDb = await _prescriptionRepository.Get(x=>x.Id == id);
                if (prescriptionFromDb == null)
                    return NotFound($"No prescription found with Id = {id}");

                var updatedPrescription = _mapper.Map(prescriptionUpdateDto, prescriptionFromDb);
                _prescriptionRepository.Update(updatedPrescription);
                await _prescriptionRepository.Save();

                return Ok("Prescription updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating prescription");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes a prescription.
        /// </summary>
        /// <param name="id">The prescription ID.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete("{id}", Name = "DeletePrescriptionAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeletePrescriptionAsync([FromRoute] int id)
        {
            if (id < 1)
                return BadRequest("Id must be greater than 0");

            try
            {
                var prescriptionFromDb = await _prescriptionRepository.Get(x=>x.Id == id);
                if (prescriptionFromDb == null)
                    return NotFound($"No prescription found with Id = {id}");

                 _prescriptionRepository.Remove(prescriptionFromDb);
                await _prescriptionRepository.Save();

                return Ok("Prescription deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting prescription");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
