using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeWorkedManagementSystem.Contexts;
using TimeWorkedManagementSystem.DTOs;
using TimeWorkedManagementSystem.Interfaces;
using TimeWorkedManagementSystem.Models;

namespace TimeWorkedManagementSystem.Controllers
{
    public class ShiftsController : Controller
    {
        private readonly UserDbContext _dbContext;
        private readonly IUserService _userService;

        public ShiftsController(UserDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        // GET: Shifts
        public async Task<IActionResult> GetAllShifts()
        {
            return Ok(await _dbContext.Shifts
                .Include(s => s.Breaks)
                .Include(s => s.Company)
                .ToListAsync());
        }
        
        // GET: Shifts/Active
        [HttpGet("Active")]
        public async Task<IActionResult> GetActiveShifts()
        {
            return Ok(await _dbContext.Shifts
                .Include(s => s.Breaks)
                .Include(s => s.Company)
                .Where(s => s.End == null)
                .ToListAsync());
        }
        
        // GET: Shifts/Company/{companyId}
        [HttpGet("Company/{companyId}")]
        public async Task<IActionResult> GetCompanyShifts(Guid companyId)
        {
            return Ok(await _dbContext.Shifts
                .Include(s => s.Breaks)
                .Include(s => s.Company)
                .Where(s => s.CompanyId == companyId)
                .ToListAsync());
        }

        // GET: Shifts/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShiftDetails(Guid id)
        {
            Shift? shift = await _dbContext.Shifts
                .Include(s => s.Breaks)
                .Include(s => s.Company)
                .FirstOrDefaultAsync(s => s.Id == id);
            
            if (shift is null)
            {
                return NotFound(id);
            }

            return Ok(shift);
        }
        
        // POST: Shifts
        [HttpPost]
        public async Task<IActionResult> CreateShift(CreateShiftRequest request)
        {
            var shift = _dbContext.Shifts.Add(new Shift
            {
                Start = request.Start,
                End = request.End,
                CompanyId = request.CompanyId,
            });
            if (request.Breaks is not null)
            {
                foreach (var breakDto in request.Breaks)
                {
                    _dbContext.Breaks.Add(new Break
                    {
                        Start = breakDto.Start,
                        End = breakDto.End,
                        ShiftId = shift.Entity.Id
                    });
                }
            }
            await _dbContext.SaveChangesAsync();
            return Ok(shift);
        }

        // POST: Shifts/Start
        [HttpPost("Start")]
        public async Task<IActionResult> StartShift(StartShiftRequest request)
        {
            var shift = _dbContext.Shifts.Add(new Shift
            {
                Start = DateTimeOffset.UtcNow,
                CompanyId = request.CompanyId,
                UserId = _userService.UserId
            });
            await _dbContext.SaveChangesAsync();
            return Ok(shift.Entity);
        }
        
        // PUT: Shifts/End
        [HttpPut("Start")]
        public async Task <IActionResult> EndShift(EndShiftRequest request)
        {
            var shift = await _dbContext.Shifts.FirstOrDefaultAsync(s => s.Id == request.ShiftId);
            if (shift is null)
                return NotFound(request.ShiftId);
            shift.End = DateTimeOffset.UtcNow;
            await _dbContext.SaveChangesAsync();
            return Ok(shift);
        }

        // PUT: Shifts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShift(Guid id, [FromBody] UpdateShiftRequest request)
        {
            Shift? shift;
            if ((shift = await _dbContext.Shifts.FirstOrDefaultAsync(s => s.Id == id)) is null)
            {
                return NotFound(id);
            }

            shift.Start = request.Start;
            shift.End = request.End;
            await _dbContext.SaveChangesAsync();
            return Ok(shift);
        }

        // DELETE: Shifts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShift(Guid id)
        {
            Shift? shift;
            if ((shift = await _dbContext.Shifts.FirstOrDefaultAsync(s => s.Id == id)) is null)
            {
                return NotFound(id);
            }

            _dbContext.Shifts.Remove(shift);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
