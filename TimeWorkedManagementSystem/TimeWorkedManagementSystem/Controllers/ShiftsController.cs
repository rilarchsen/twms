using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TimeWorkedManagementSystem.Contexts;
using TimeWorkedManagementSystem.DTOs;
using TimeWorkedManagementSystem.Interfaces;
using TimeWorkedManagementSystem.Models;

namespace TimeWorkedManagementSystem.Controllers
{
    public class ShiftsController : ApiControllerBase
    {
        private readonly UserDbContext _dbContext;
        private readonly IUserService _userService;

        public ShiftsController(UserDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        // GET: Shifts
        [HttpGet]
        [SwaggerResponse(200, "OK", typeof(Shift[]))]
        public async Task<IActionResult> GetAllShifts()
        {
            List<Shift> allShifts = await _dbContext.Shifts
                .Include(s => s.Breaks)
                .AsNoTracking()
                .ToListAsync();
            foreach (Shift shift in allShifts)
            {
                shift.Company = await _dbContext.Companies
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == shift.CompanyId);
            }
            return Ok(allShifts);
        }
        
        // GET: Shifts/Active
        [HttpGet("Active")]
        [SwaggerResponse(200, "OK", typeof(Shift[]))]
        public async Task<IActionResult> GetActiveShifts()
        {
            List<Shift> activeShifts = await _dbContext.Shifts
                .Include(s => s.Breaks)
                .Where(s => s.End == null)
                .AsNoTracking()
                .ToListAsync();
            foreach (Shift shift in activeShifts)
            {
                shift.Company = await _dbContext.Companies
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == shift.CompanyId);
            }
            return Ok(activeShifts);
        }
        
        // GET: Shifts/Company/{companyId}
        [HttpGet("Company/{companyId}")]
        [SwaggerResponse(200, "OK", typeof(Shift[]))]
        public async Task<IActionResult> GetCompanyShifts(Guid companyId)
        {
            List<Shift> companyShifts = await _dbContext.Shifts
                .Include(s => s.Breaks)
                .Where(s => s.CompanyId == companyId)
                .AsNoTracking()
                .ToListAsync();
            foreach (Shift shift in companyShifts)
            {
                shift.Company = await _dbContext.Companies
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == shift.CompanyId);
            }
            return Ok(companyShifts);
        }

        // GET: Shifts/{id}
        [HttpGet("{id}")]
        [SwaggerResponse(200, "OK", typeof(Shift))]
        public async Task<IActionResult> GetShiftDetails(Guid id)
        {
            Shift? shift = await _dbContext.Shifts
                .Include(s => s.Breaks)
                .FirstOrDefaultAsync(s => s.Id == id);
            
            if (shift is null)
            {
                return NotFound(id);
            }

            shift.Company = await _dbContext.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == shift.CompanyId);

            return Ok(shift);
        }
        
        // POST: Shifts
        [HttpPost]
        [SwaggerResponse(200, "OK", typeof(Shift))]
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
        [SwaggerResponse(200, "OK", typeof(Shift))]
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
        [HttpPut("End")]
        [SwaggerResponse(200, "OK", typeof(Shift))]
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
        [SwaggerResponse(200, "OK", typeof(Shift))]
        public async Task<IActionResult> UpdateShift(Guid id, [FromBody] UpdateShiftRequest request)
        {
            Shift? shift;
            if ((shift = await _dbContext.Shifts.FirstOrDefaultAsync(s => s.Id == id)) is null)
            {
                return NotFound(id);
            }

            shift.Start = request.Start;
            shift.End = request.End;
            //TODO: handle breaks
            await _dbContext.SaveChangesAsync();
            return Ok(shift);
        }

        // DELETE: Shifts/{id}
        [HttpDelete("{id}")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(404, "Not Found")]
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
