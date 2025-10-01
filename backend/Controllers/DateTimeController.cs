using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DateTimeController : ControllerBase
    {
        private readonly DateTimeService _dateTimeService;

        public DateTimeController(DateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var entries = await _dateTimeService.GetAllDateTimeEntriesAsync();
            return Ok(entries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entry = await _dateTimeService.GetDateTimeEntryByIdAsync(id);
            if (entry == null)
                return NotFound();
            return Ok(entry);
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentDateTime()
        {
            var currentTime = await _dateTimeService.GetCurrentDateTimeAsync();
            return Ok(currentTime);
        }

        [HttpGet("utc")]
        public async Task<IActionResult> GetUtcDateTime()
        {
            var utcTime = await _dateTimeService.GetUtcDateTimeAsync();
            return Ok(utcTime);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DateTimeInfo dateTimeInfo)
        {
            if (dateTimeInfo == null)
                return BadRequest();

            var createdEntry = await _dateTimeService.CreateDateTimeEntryAsync(dateTimeInfo);
            return CreatedAtAction(nameof(GetById), new { id = createdEntry.Id }, createdEntry);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DateTimeInfo dateTimeInfo)
        {
            var updatedEntry = await _dateTimeService.UpdateDateTimeEntryAsync(id, dateTimeInfo);
            if (updatedEntry == null)
                return NotFound();
            return Ok(updatedEntry);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _dateTimeService.DeleteDateTimeEntryAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
