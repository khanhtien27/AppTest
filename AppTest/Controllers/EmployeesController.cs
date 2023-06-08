using AppTest.Data;
using AppTest.Models;
using AppTest.Models.ModelDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace AppTest.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? Id)
        {
            if(Id == null)
            {
                var model = await _context.Users.ToListAsync();
                return View(model);
            }
            else
            {
                var model = await _context.Users.Where(e => e.IdDepartment == Id).ToListAsync();
                return View(model);
            }
        }

        public async Task<IActionResult> ChangeAvatar(string? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            else
            {
                var employee = await _context.Users.FindAsync(Id);
                if (employee == null)
                {
                    return NotFound();
                }
                return View(employee);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeAvatar(string Id, string img)
        {
            var model = await _context.Users.FindAsync(Id);
            if(model == null)
            {
                return NotFound();
            }
            else
            {
                model.Img = img;
                _context.Users.Update(model);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
        }


        //public async Task<IActionResult> TimeSheetByMonth()
        //{
        //    TimeSheetMonth timeSheetMonth = new TimeSheetMonth();
        //    return View(timeSheetMonth);
        //}

        public async Task<IActionResult> TimeSheetByMonth (int? Month)
        {
            
            var model = await _context.TimeSheet.Where(t => t.Start.Month == Month).ToListAsync();
            if(model == null)
            {
                return NotFound();
            }
            return View(model);
        }

    }
}
