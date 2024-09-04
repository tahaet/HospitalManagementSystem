using AutoMapper;
using Hospital.DataAccess.Repository.IRepository;
using Hospital.Models.Dto.ExpenseDto;
using Hospital.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.APIs.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ExpenseController : ControllerBase
    {
        private readonly ILogger<ExpenseController> logger;
        private readonly IExpenseRepository expenseRepository;
        private readonly IMapper mapper;

        public ExpenseController(ILogger<ExpenseController> logger, IExpenseRepository expenseRepository, IMapper mapper)
        {
            this.logger = logger;
            this.expenseRepository = expenseRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetExpenseByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Expense>> GetExpenseByIdAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var expense = await expenseRepository.Get(x => x.Id == id,includeProperties:"Department");

                if (expense == null)
                {
                    return NotFound($"No expense exists with Id = {id}");
                }

                return Ok(expense);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("all", Name = "GetAllExpensesAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Expense>>> GetAllExpensesAsync()
        {
            try
            {
                var expenses = await expenseRepository.GetAll(includeProperties: "Department");

                if (expenses == null)
                {
                    return NotFound("No expenses exist");
                }

                return Ok(expenses);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost(Name = "CreateExpenseAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Expense>> CreateExpenseAsync([FromBody] ExpenseCreateDto expenseCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid");
            }

            try
            {
                var expense = mapper.Map<Expense>(expenseCreateDto);

                if (expense == null)
                {
                    return StatusCode(500, "Internal server error");
                }

                await expenseRepository.Add(expense);
                await expenseRepository.Save();

                return CreatedAtRoute(nameof(GetExpenseByIdAsync), new { id = expense.Id }, expense);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}", Name = "UpdateExpenseAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateExpenseAsync([FromRoute] int id, [FromBody] ExpenseUpdateDto expenseUpdateDto)
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
                var expenseFromDb = await expenseRepository.Get(x => x.Id == id);

                if (expenseFromDb == null)
                {
                    return NotFound($"No expense exists with Id = {id}");
                }

                var expense = mapper.Map(expenseUpdateDto, expenseFromDb);

                if (expense == null)
                {
                    return StatusCode(500, "Internal server error");
                }

                expenseRepository.Update(expense);
                await expenseRepository.Save();
                return Ok("Model was updated successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}", Name = "DeleteExpenseAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteExpenseAsync([FromRoute] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id must be greater than 0");
            }

            try
            {
                var expenseFromDb = await expenseRepository.Get(x => x.Id == id);

                if (expenseFromDb == null)
                {
                    return NotFound($"No expense exists with Id = {id}");
                }

                expenseRepository.Remove(expenseFromDb);
                await expenseRepository.Save();
                return Ok("Model was deleted successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("PaymentStatus", Name = "UpdateExpensePaymentAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateExpensePaymentAsync([FromQuery] int id, [FromQuery] string PaymentStatus)
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
                var success = await expenseRepository.UpdatePaymentStatus(id, PaymentStatus);
                if (success)
                {
                    await expenseRepository.Save();
                    return Ok("Model was updated successfully");
                }
                return NotFound($"No expense exists with Id = {id}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }


    }

}
