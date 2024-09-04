using AutoMapper;
using Hospital.DataAccess.Repository.IRepository;
using Hospital.Models.Dto.VaccineAppointmentDto;
using Hospital.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.APIs.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class VaccineAppointmentController : ControllerBase
    {
        private readonly ILogger<VaccineAppointmentController> _logger;
        private readonly IVaccineAppointmentRepository _vaccineAppointmentRepository;
        private readonly IMapper _mapper;

        public VaccineAppointmentController(
            ILogger<VaccineAppointmentController> logger,
            IVaccineAppointmentRepository vaccineAppointmentRepository,
            IMapper mapper)
        {
            _logger = logger;
            _vaccineAppointmentRepository = vaccineAppointmentRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new vaccine appointment.
        /// </summary>
        /// <param name="createDto">The appointment details.</param>
        /// <returns>The created appointment.</returns>
        [HttpPost(Name = "CreateVaccineAppointmentAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VaccineAppointment>> CreateVaccineAppointmentAsync([FromBody] VaccineAppointmentCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            try
            {
                var vaccineAppointment = _mapper.Map<VaccineAppointment>(createDto);
                await _vaccineAppointmentRepository.Add(vaccineAppointment);
                await _vaccineAppointmentRepository.Save();

                return CreatedAtRoute(nameof(GetVaccineAppointmentByIdAsync), new { id = vaccineAppointment.Id }, vaccineAppointment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vaccine appointment");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves a vaccine appointment by ID.
        /// </summary>
        /// <param name="id">The appointment ID.</param>
        /// <returns>The appointment details.</returns>
        [HttpGet("{id}", Name = "GetVaccineAppointmentByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VaccineAppointment>> GetVaccineAppointmentByIdAsync([FromRoute] int id)
        {
            if (id < 1)
                return BadRequest("Id must be greater than 0");

            try
            {
                var vaccineAppointment = await _vaccineAppointmentRepository.Get(x=>x.Id == id, includeProperties: "Patient,Nurse,Vaccine");
                if (vaccineAppointment == null)
                    return NotFound($"No appointment found with Id = {id}");

                return Ok(vaccineAppointment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccine appointment");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves all vaccine appointments.
        /// </summary>
        /// <returns>The list of all appointments.</returns>
        [HttpGet("all", Name = "GetAllVaccineAppointmentsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<VaccineAppointment>>> GetAllVaccineAppointmentsAsync()
        {
            try
            {
                var vaccineAppointments = await _vaccineAppointmentRepository.GetAll(includeProperties: "Patient,Nurse,Vaccine");
                if (vaccineAppointments == null || !vaccineAppointments.Any())
                    return NotFound("No vaccine appointments exist");

                return Ok(vaccineAppointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccine appointments");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates a vaccine appointment.
        /// </summary>
        /// <param name="id">The appointment ID.</param>
        /// <param name="updateDto">The updated appointment details.</param>
        /// <returns>The result of the update operation.</returns>
        [HttpPut("{id}", Name = "UpdateVaccineAppointmentAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateVaccineAppointmentAsync([FromRoute] int id, [FromBody] VaccineAppointmentUpdateDto updateDto)
        {
            if (id < 1)
                return BadRequest("Id must be greater than 0");

            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            try
            {
                var appointmentFromDb = await _vaccineAppointmentRepository.Get(x=>x.Id == id);
                if (appointmentFromDb == null)
                    return NotFound($"No appointment found with Id = {id}");

                var updatedAppointment = _mapper.Map(updateDto, appointmentFromDb);
                _vaccineAppointmentRepository.Update(updatedAppointment);
                await _vaccineAppointmentRepository.Save();

                return Ok("Appointment updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vaccine appointment");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes a vaccine appointment.
        /// </summary>
        /// <param name="id">The appointment ID.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete("{id}", Name = "DeleteVaccineAppointmentAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteVaccineAppointmentAsync([FromRoute] int id)
        {
            if (id < 1)
                return BadRequest("Id must be greater than 0");

            try
            {
                var appointmentFromDb = await _vaccineAppointmentRepository.Get(x=>x.Id == id);
                if (appointmentFromDb == null)
                    return NotFound($"No appointment found with Id = {id}");

                _vaccineAppointmentRepository.Remove(appointmentFromDb);
                await _vaccineAppointmentRepository.Save();

                return Ok("Appointment deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vaccine appointment");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
