using AutoMapper;
using Hospital.DataAccess.Repository.IRepository;
using Hospital.Models.Dto.VendorDto;
using Hospital.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.APIs.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class VendorController : ControllerBase
    {
        private readonly ILogger<VendorController> logger;
        private readonly IVendorRepository vendorRepository;
        private readonly IMapper mapper;

        public VendorController(ILogger<VendorController> logger, IVendorRepository vendorRepository, IMapper mapper)
        {
            this.logger = logger;
            this.vendorRepository = vendorRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetVendorByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Vendor>> GetVendorByIdAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var vendor = await vendorRepository.Get(x => x.Id == id);

                if (vendor == null)
                {
                    return NotFound($"No vendor exists with Id = {id}");
                }

                return Ok(vendor);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("all", Name = "GetAllVendorsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Vendor>>> GetAllVendorsAsync()
        {
            try
            {
                var vendors = await vendorRepository.GetAll();

                if (vendors == null)
                {
                    return NotFound("No vendors exist");
                }

                return Ok(vendors);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost(Name = "CreateVendorAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Vendor>> CreateVendorAsync([FromBody] VendorCreateDto vendorCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid");
            }

            try
            {
                var vendor = mapper.Map<Vendor>(vendorCreateDto);

                if (vendor == null)
                {
                    return StatusCode(500, "Internal server error");
                }

                await vendorRepository.Add(vendor);
                await vendorRepository.Save();

                return CreatedAtRoute(nameof(GetVendorByIdAsync), new { id = vendor.Id }, vendor);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}", Name = "UpdateVendorAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateVendorAsync([FromRoute] int id, [FromBody] VendorUpdateDto vendorUpdateDto)
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
                var vendorFromDb = await vendorRepository.Get(x => x.Id == id);

                if (vendorFromDb == null)
                {
                    return NotFound($"No vendor exists with Id = {id}");
                }

                var vendor = mapper.Map(vendorUpdateDto, vendorFromDb);

                if (vendor == null)
                {
                    return StatusCode(500, "Internal server error");
                }

                vendorRepository.Update(vendor);
                await vendorRepository.Save();
                return Ok("Model was updated successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}", Name = "DeleteVendorAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteVendorAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var vendorFromDb = await vendorRepository.Get(x => x.Id == id);

                if (vendorFromDb == null)
                {
                    return NotFound($"No vendor exists with Id = {id}");
                }

                vendorRepository.Remove(vendorFromDb);
                await vendorRepository.Save();
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
