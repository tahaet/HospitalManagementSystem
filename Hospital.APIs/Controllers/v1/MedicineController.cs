using AutoMapper;
using Hospital.DataAccess.Repository.IRepository;
using Hospital.Models;
using Hospital.Models.Dto.MedicineDTo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.APIs.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MedicineController : ControllerBase
    {
        private readonly ILogger<MedicineController> logger;
        private readonly IMedicineRepository medicineRepository;
        private readonly IMapper mapper;

        public MedicineController(ILogger<MedicineController> logger, IMedicineRepository medicineRepository, IMapper mapper)
        {
            this.logger = logger;
            this.medicineRepository = medicineRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetMedicineByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Medicine>> GetMedicineByIdAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var medicine = await medicineRepository.Get(x => x.Id == id,includeProperties:"Vendor");

                if (medicine == null)
                {
                    return NotFound($"No medicine exists with Id = {id}");
                }

                return Ok(medicine);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("name", Name = "GetMedicineByNameAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Medicine>> GetMedicineByNameAsync([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name must have a value");
            }

            try
            {
                var medicine = await medicineRepository.Get(x => x.Name == name, includeProperties: "Vendor");

                if (medicine == null)
                {
                    return NotFound($"No medicine exists with Name = {name}");
                }

                return Ok(medicine);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("all", Name = "GetAllMedicinesAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Medicine>>> GetAllMedicinesAsync()
        {
            try
            {
                var medicines = await medicineRepository.GetAll(includeProperties: "Vendor");

                if (medicines == null || !medicines.Any())
                {
                    return NotFound("No medicines exist");
                }

                return Ok(medicines);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost(Name = "CreateMedicineAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Medicine>> CreateMedicineAsync([FromBody] MedicineCreateDto medicineCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid");
            }

            try
            {
                var medicine = mapper.Map<Medicine>(medicineCreateDto);

                if (medicine == null)
                {
                    return StatusCode(500, "Internal server error");
                }

                await medicineRepository.Add(medicine);
                await medicineRepository.Save();

                return CreatedAtAction(nameof(GetMedicineByIdAsync), new { id = medicine.Id }, medicine);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}", Name = "UpdateMedicineAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateMedicineAsync([FromRoute] int id, [FromBody] MedicineUpdateDto medicineUpdateDto)
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
                var medicineFromDb = await medicineRepository.Get(x => x.Id == id);

                if (medicineFromDb == null)
                {
                    return NotFound($"No medicine exists with Id = {id}");
                }

                var medicine = mapper.Map<Medicine>(medicineUpdateDto);

                if (medicine == null)
                {
                    return StatusCode(500, "Internal server error");
                }

                medicineRepository.Update(medicine);
                await medicineRepository.Save();
                return Ok("Model was updated successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}", Name = "DeleteMedicineAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteMedicineAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var medicineFromDb = await medicineRepository.Get(x => x.Id == id);

                if (medicineFromDb == null)
                {
                    return NotFound($"No medicine exists with Id = {id}");
                }

                medicineRepository.Remove(medicineFromDb);
                await medicineRepository.Save();
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
