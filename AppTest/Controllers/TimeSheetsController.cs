using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppTest.Data;
using AppTest.Models;

namespace AppTest.Controllers
{
    public class TimeSheetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TimeSheetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TimeSheets
        public async Task<IActionResult> Index(string? Id)
        {
            if(Id == null)
            {
                var applicationDbContext = _context.TimeSheet.Include(t => t.employee);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var applicationDbContext = _context.TimeSheet.Where(t => t.IdEmployment == Id).Include(t => t.employee);
                return View(await applicationDbContext.ToListAsync());
            }
          
        }

        // GET: TimeSheets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TimeSheet == null)
            {
                return NotFound();
            }

            var timeSheet = await _context.TimeSheet
                .Include(t => t.employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timeSheet == null)
            {
                return NotFound();
            }

            return View(timeSheet);
        }

        // GET: TimeSheets/Create
        public IActionResult Create()
        {
            ViewData["IdEmployment"] = new SelectList(_context.Users, "Id", "FirstName");
            return View();
        }

        // POST: TimeSheets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdEmployment,Start,BreakStart,BreakEnd,End")] TimeSheet timeSheet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(timeSheet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEmployment"] = new SelectList(_context.Users, "Id", "Id", timeSheet.IdEmployment);
            return View(timeSheet);
        }

        // GET: TimeSheets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TimeSheet == null)
            {
                return NotFound();
            }

            var timeSheet = await _context.TimeSheet.FindAsync(id);
            if (timeSheet == null)
            {
                return NotFound();
            }
            ViewData["IdEmployment"] = new SelectList(_context.Users, "Id", "FirstName", timeSheet.IdEmployment);
            return View(timeSheet);
        }

        // POST: TimeSheets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TimeSheet timeSheet)
        {


            if (id != timeSheet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timeSheet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimeSheetExists(timeSheet.Id))
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
            ViewData["IdEmployment"] = new SelectList(_context.Users, "Id", "Id", timeSheet.IdEmployment);
            return View(timeSheet);
        }

        // GET: TimeSheets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TimeSheet == null)
            {
                return NotFound();
            }

            var timeSheet = await _context.TimeSheet
                .Include(t => t.employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timeSheet == null)
            {
                return NotFound();
            }

            return View(timeSheet);
        }

        // POST: TimeSheets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TimeSheet == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TimeSheet'  is null.");
            }
            var timeSheet = await _context.TimeSheet.FindAsync(id);
            if (timeSheet != null)
            {
                _context.TimeSheet.Remove(timeSheet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimeSheetExists(int id)
        {
          return (_context.TimeSheet?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
