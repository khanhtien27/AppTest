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
            if (Id == null)
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
            if (model == null)
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
        public async Task<IActionResult> TimeSheetByMonth(int? Month)
        {
            if (Month != null)
            {
                Dictionary<string, TimeSpan> ToltalTime = new Dictionary<string, TimeSpan>();

                var model = await _context.TimeSheet.Where(t => t.Start.Month == Month).Include(e => e.employee).AsNoTracking().ToListAsync();
                if (model.Count == 0)
                {
                    return View();
                }
                else
                {
                    for (int i = 0; i < model.Count; i++)
                    {
                        TimeSpan totalTime = new TimeSpan();
                        var item = model[i];
                        if (item.BreakStart == null)
                        {
                            totalTime = (TimeSpan)(item.End - item.Start);
                        }
                        else
                        {
                            totalTime = (TimeSpan)(item.BreakStart - item.Start) + (TimeSpan)(item.End - item.BreakEnd);
                        }

                        if (ToltalTime.TryAdd(item.IdEmployment, totalTime) == false)
                        {
                            var a = ToltalTime.TryGetValue(item.IdEmployment, out var b);
                            b += totalTime;
                            ToltalTime[item.IdEmployment] = b;
                        }
                    }
                    
                    if(ToltalTime.MaxBy(i => i.Value).Key != null)
                    {
                        var n = ToltalTime.MaxBy(i => i.Value);
                        var m = n.Key;
                        var time = n.Value;
                        ViewBag.Max = (time.TotalMinutes / 60).ToString("##0.##");
                        var employee = await _context.Users.FindAsync(m);
                        ViewBag.Employee = employee.FirstName + " " + employee.LastName;
                    }
                    return View(model);
                }
            }
            else
            {
                var model = await _context.TimeSheet.Include(e => e.employee).AsNoTracking().ToListAsync();
                if (model == null)
                {
                    return NotFound();
                }
                return View(model);
            }
        }

        public async Task<IActionResult> CalculateSalary(int? Month)
        {
            if (Month == null)
            {
                return View();
            }
            const double regularPayment = 3.125;
            Dictionary<string, TimeSpan> ToltalTime = new Dictionary<string, TimeSpan>();
            Dictionary<Employee, double> Salary = new Dictionary<Employee, double>();
            var model = await _context.TimeSheet.Where(t => t.Start.Month == Month).Include(e => e.employee).AsNoTracking().ToListAsync();
            if (model == null)
            {
                return NotFound();
            }

            for (int i = 0; i < model.Count; i++)
            {
                TimeSpan totalTime = new TimeSpan();
                var item = model[i];
                if (item.BreakStart == null)
                {
                    totalTime = (TimeSpan)(item.End - item.Start);
                }
                else
                {
                    totalTime = (TimeSpan)(item.BreakStart - item.Start) + (TimeSpan)(item.End - item.BreakEnd);
                }

                if (ToltalTime.TryAdd(item.IdEmployment, totalTime) == false)
                {
                    var a = ToltalTime.TryGetValue(item.IdEmployment, out var b);
                    b += totalTime;
                    ToltalTime[item.IdEmployment] = b;
                }
            }
            foreach (var employee in ToltalTime)
            {
                if (employee.Value.TotalMinutes / 60 < 40)
                {
                    var a = await _context.Users.FindAsync(employee.Key);
                    var b = (employee.Value.TotalMinutes / 60) * regularPayment;
                    Salary.Add(a, b);
                }
                else
                {
                    var a = await _context.Users.FindAsync(employee.Key);
                    var b = (employee.Value.TotalMinutes / 60) * regularPayment * 1.5;
                    Salary.Add(a, b);
                }
            }
            return View(Salary);
        }

        public async Task<IActionResult> Infor(string Id)
        {
            var model = await _context.Users.Include(i => i.department).FirstOrDefaultAsync(i => i.Id == Id);
            if (model == null) return NotFound();
            return View(model);
        }
    }
}
