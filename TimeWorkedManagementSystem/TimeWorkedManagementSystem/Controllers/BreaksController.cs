using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TimeWorkedManagementSystem.Contexts;
using TimeWorkedManagementSystem.Models;

namespace TimeWorkedManagementSystem.Controllers
{
    public class BreaksController : Controller
    {
        private readonly UserDbContext _context;

        public BreaksController(UserDbContext context)
        {
            _context = context;
        }

        // GET: Breaks
        public async Task<IActionResult> Index()
        {
              return _context.Breaks != null ? 
                          View(await _context.Breaks.ToListAsync()) :
                          Problem("Entity set 'UserDbContext.Breaks'  is null.");
        }

        // GET: Breaks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Breaks == null)
            {
                return NotFound();
            }

            var @break = await _context.Breaks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@break == null)
            {
                return NotFound();
            }

            return View(@break);
        }

        // GET: Breaks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Breaks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Start,End,UserId,ShiftId")] Break @break)
        {
            if (ModelState.IsValid)
            {
                @break.Id = Guid.NewGuid();
                _context.Add(@break);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@break);
        }

        // GET: Breaks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Breaks == null)
            {
                return NotFound();
            }

            var @break = await _context.Breaks.FindAsync(id);
            if (@break == null)
            {
                return NotFound();
            }
            return View(@break);
        }

        // POST: Breaks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Start,End,UserId,ShiftId")] Break @break)
        {
            if (id != @break.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@break);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BreakExists(@break.Id))
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
            return View(@break);
        }

        // GET: Breaks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Breaks == null)
            {
                return NotFound();
            }

            var @break = await _context.Breaks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@break == null)
            {
                return NotFound();
            }

            return View(@break);
        }

        // POST: Breaks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Breaks == null)
            {
                return Problem("Entity set 'UserDbContext.Breaks'  is null.");
            }
            var @break = await _context.Breaks.FindAsync(id);
            if (@break != null)
            {
                _context.Breaks.Remove(@break);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BreakExists(Guid id)
        {
          return (_context.Breaks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
