using AutoMapper;
using Hospital.DataAccess.Repository.IRepository;
using Hospital.Models.Dto.Department;
using Hospital.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hospital.APIs.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class DepartmentController : ControllerBase
    {
        private readonly ILogger<DepartmentController> logger;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        public DepartmentController(ILogger<DepartmentController> logger, IDepartmentRepository departmentRepository, IMapper mapper)
        {
            this.logger = logger;
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetDepartmentByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Department>> GetDepartmentByIdAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var department = await departmentRepository.Get(x => x.Id == id,includeProperties:"Floor");

                if (department == null)
                {
                    return NotFound($"No department exists with Id = {id}");
                }

                return Ok(department);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("name", Name = "GetDepartmentByNameAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Department>> GetDepartmentByNameAsync([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name must have a value");
            }

            try
            {
                var department = await departmentRepository.Get(x => x.Name == name, includeProperties: "Floor");

                if (department == null)
                {
                    return NotFound($"No department exists with Name = {name}");
                }

                return Ok(department);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("all", Name = "GetAllDepartmentsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Department>>> GetAllDepartmentsAsync()
        {
            try
            {
                var departments = await departmentRepository.GetAll(includeProperties: "Floor");

                if (departments == null || !departments.Any())
                {
                    return NotFound("No departments exist");
                }

                return Ok(departments);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost(Name = "CreateDepartmentAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Department>> CreateDepartmentAsync([FromBody] DepartmentCreateDto departmentCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid");
            }

            try
            {
                var department = mapper.Map<Department>(departmentCreateDto);

                if (department == null)
                {
                    return StatusCode(500, "Internal server error");
                }

                await departmentRepository.Add(department);
                await departmentRepository.Save();

                return CreatedAtAction(nameof(GetDepartmentByIdAsync), new { id = department.Id }, department);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}", Name = "UpdateDepartmentAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateDepartmentAsync([FromRoute] int id, [FromBody] DepartmentUpdateDto departmentUpdateDto)
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
                var departmentFromDb = await departmentRepository.Get(x => x.Id == id);

                if (departmentFromDb == null)
                {
                    return NotFound($"No department exists with Id = {id}");
                }

                var department = mapper.Map<Department>(departmentUpdateDto);

                if (department == null)
                {
                    return StatusCode(500, "Internal server error");
                }

                departmentRepository.Update(department);
                await departmentRepository.Save();
                return Ok("Model was updated successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}", Name = "DeleteDepartmentAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteDepartmentAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var departmentFromDb = await departmentRepository.Get(x => x.Id == id);

                if (departmentFromDb == null)
                {
                    return NotFound($"No department exists with Id = {id}");
                }

                departmentRepository.Remove(departmentFromDb);
                await departmentRepository.Save();
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
