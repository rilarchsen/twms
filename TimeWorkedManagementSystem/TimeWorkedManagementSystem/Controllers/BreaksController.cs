using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeWorkedManagementSystem.Contexts;
using TimeWorkedManagementSystem.DTOs;
using TimeWorkedManagementSystem.Interfaces;
using TimeWorkedManagementSystem.Models;

namespace TimeWorkedManagementSystem.Controllers
{
    public class BreaksController : Controller
    {
        private readonly UserDbContext _dbContext;
        private readonly IUserService _userService;

        public BreaksController(UserDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        // GET: Breaks
        public async Task<IActionResult> GetAllBreaks()
        {
            return Ok(await _dbContext.Breaks.ToListAsync());
        }

        public async Task<IActionResult> GetShiftBreaks(Guid shiftId)
        {
            Shift? shift = await _dbContext.Shifts.FirstOrDefaultAsync(s => s.Id == shiftId);
            if (shift is null)
                return NotFound(shiftId);
            return Ok(await _dbContext.Breaks.Where(b => b.ShiftId == shiftId).ToListAsync());
        }

        // GET: Breaks/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBreakDetails(Guid id)
        {
            Break? @break;
            if ((@break = await _dbContext.Breaks.FirstOrDefaultAsync(b => b.Id == id)) is null)
            {
                return NotFound(id);
            }

            return Ok(@break);
        }

        // PUT: Breaks
        public async Task<IActionResult> AddBreak(CreateBreakRequest request)
        {
            var @break = _dbContext.Breaks.Add(new Break
            {
                Start = request.Start,
                End = request.End,
                ShiftId = request.ShiftId,
            });
            await _dbContext.SaveChangesAsync();
            return Ok(@break.Entity);
        }
        
        // PUT: Breaks/Start
        [HttpPut("Start")]
        public async Task<IActionResult> StartBreak(StartBreakRequest request)
        {
            var @break = _dbContext.Breaks.Add(new Break
            {
                Start = DateTimeOffset.UtcNow,
                ShiftId = request.ShiftId,
            });
            await _dbContext.SaveChangesAsync();
            return Ok(@break.Entity);
        }
        
        // PUT: Breaks/End
        [HttpPut("End")]
        public async Task<IActionResult> EndBreak(EndBreakRequest request)
        {
            var @break = await _dbContext.Breaks.FirstOrDefaultAsync(b => b.Id == request.BreakId);
            if (@break is null)
                return NotFound(request.BreakId);
            @break.End = DateTimeOffset.UtcNow;
            await _dbContext.SaveChangesAsync();
            return Ok(@break);
        }

        // PUT: Breaks/Edit
        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(EditBreakRequest request)
        {
            var existingBreak = await _dbContext.Breaks.FirstOrDefaultAsync(b => b.Id == request.Id);
            
            if (existingBreak is null) 
                return NotFound(request);
            
            existingBreak.Start = request.Start;
            existingBreak.End = request.End;
            await _dbContext.SaveChangesAsync();
            return Ok(existingBreak);
        }

        // DELETE: Breaks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Break? @break;
            if ((@break = await _dbContext.Breaks.FirstOrDefaultAsync(b => b.Id == id)) is null)
            {
                return NotFound(id);
            }

            _dbContext.Breaks.Remove(@break);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
