using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TimeWorkedManagementSystem.Contexts;
using TimeWorkedManagementSystem.Extensions;
using TimeWorkedManagementSystem.Interfaces;
using TimeWorkedManagementSystem.Models;
using TimeWorkedManagementSystem.Services;

namespace TimeWorkedManagementSystem.Controllers
{
    public class ShiftsController : Controller
    {
        private readonly UserDbContext _context;
        private readonly IUserService _userService;

        public ShiftsController(UserDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: Shifts
        public async Task<IActionResult> Index()
        {
            var userDbContext = _context.Shifts
                .Include(s => s.Breaks)
                .Include(s => s.Company);
            return View(await userDbContext.ToListAsync());
        }

        // GET: Shifts
        public async Task<IActionResult> PartialShiftsIndex()
        {
            var userDbContext = _context.Shifts.Include(s => s.Breaks).Include(s => s.Company);
            return PartialView("~/Views/Shared/PartialShiftsIndex.cshtml", await userDbContext.ToListAsync());
        }

        // GET: Shifts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Shifts == null)
            {
                return NotFound();
            }

            var shift = await _context.Shifts
                .Include(s => s.Company)
                .Include(s => s.Breaks)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shift == null)
            {
                return NotFound();
            }

            return View(shift);
        }

        // GET: Shifts/Create
        public IActionResult Create()
        {
            //ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id");
            ViewData["CompanyId"] = new SelectList(_context.Companies, nameof(Company.Id), nameof(Company.Name));
            return View();
        }

        // POST: Shifts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Start,End,CompanyId")] Shift shift)
        {
            if (ModelState.IsValid)
            {
                //shift.Id = Guid.NewGuid();
                shift.UserId = _userService.UserId;
                _context.Add(shift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", shift.CompanyId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, nameof(Company.Id), nameof(Company.Name));
            return View(shift);
        }

        // GET: Shifts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Shifts == null)
            {
                return NotFound();
            }

            var shift = await _context.Shifts.Include(s => s.Breaks).FirstOrDefaultAsync(s => s.Id == id);//.FindAsync(id);
            if (shift == null)
            {
                return NotFound();
            }
            shift.SortBreaks();
            //shift.Breaks.Sort(new Comparison<Break>((Break b1, Break b2) => b1.Start.CompareTo(b2.Start)));
            //ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", shift.CompanyId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, nameof(Company.Id), nameof(Company.Name));
            return View(shift);
        }

        // POST: Shifts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Start,End,CompanyId,Breaks")] Shift shift)
        {
            if (id != shift.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    shift.UserId = _userService.UserId;
                    //_context.Update(shift);
                    Shift? contextShift = await _context.Shifts
                        .Include(s => s.Breaks)
                        .FirstOrDefaultAsync(s => s.Id == shift.Id);
                    if (contextShift == null) { return NotFound(); }
                    var exceptBreaks = contextShift.Breaks.Select(b => b.Id).Except(shift.Breaks.Select(b => b.Id)).ToList();
                    foreach (Guid diffBreak in exceptBreaks)
                    {
                        Break? foundBreak = await _context.Breaks.FindAsync(diffBreak);
                        if (foundBreak is not null)
                            _context.Breaks.Remove(foundBreak!);
                    }
                    _context.Shifts.Remove(contextShift);
                    _context.Shifts.Add(shift);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShiftExists(shift.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, nameof(Company.Id), nameof(Company.Name));
            return View(shift);
        }

        // GET: Shifts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Shifts == null)
            {
                return NotFound();
            }

            var shift = await _context.Shifts
                .Include(s => s.Company)
                .Include(s => s.Breaks)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shift == null)
            {
                return NotFound();
            }

            return View(shift);
        }

        // POST: Shifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Shifts == null)
            {
                return Problem("Entity set 'UserDbContext.Shifts'  is null.");
            }
            var shift = await _context.Shifts.FindAsync(id);
            if (shift != null)
            {
                _context.Shifts.Remove(shift);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShiftExists(Guid id)
        {
          return (_context.Shifts?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBreak(Guid breakId, [Bind("Id,Start,End,CompanyId,Breaks")] Shift shift)
        {
            if (!shift.Breaks.Select(b => b.Id).Contains(breakId) || _context.Breaks == null || shift is null)
            {
                return NotFound();
            }
            
            var breakItem = shift.Breaks.FirstOrDefault(b => b.Id == breakId);

            if (breakItem == null)
            {
                return NotFound();
            }

            bool rem = shift.Breaks.Remove(breakItem);

            ViewData["CompanyId"] = new SelectList(_context.Companies, nameof(Company.Id), nameof(Company.Name));
            return View("Edit", shift);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBreak([Bind("Id,Start,End,CompanyId,Breaks")] Shift shift)
        {
            DateTime startTime = shift.Start;
            if (shift.Breaks.Count > 0)
            {
                shift.SortBreaks();
                startTime = shift.Breaks[shift.Breaks.Count - 1].End;
            }
            Break newBreak = new Break
            {
                UserId = _userService.UserId,
                Id = Guid.NewGuid(),
                Start = startTime,
                End = startTime,
                ShiftId = shift.Id
            };

            shift.Breaks.Add(newBreak);

            ViewData["CompanyId"] = new SelectList(_context.Companies, nameof(Company.Id), nameof(Company.Name));
            return View("Edit", shift);
        }

    }
}
