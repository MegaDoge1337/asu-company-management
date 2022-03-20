using CompanyManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyManagement.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ILogger<EmployeesController> _logger;
        private AppDbContext _context;

        public EmployeesController(ILogger<EmployeesController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Genders = new List<string>();
            ViewBag.Genders.Add("не указано");
            ViewBag.Genders.Add("мужской");
            ViewBag.Genders.Add("женский");
            ViewBag.Genders.Add("трансформер");
            var employees = _context.Employees;
            ViewBag.EmployeesCount = employees.Count();
            return View(employees);
        }

        public IActionResult AddEmployee() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveEmployee(string cipher, string fullName, DateTime birthDate, byte gender)
        {
            var employee = new Employee();
            employee.Cipher = cipher;
            employee.FullName = fullName;
            employee.BirthDate = birthDate;
            employee.Gender = gender;
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AcceptDelete(int id) 
        {
            ViewBag.Genders = new List<string>();
            ViewBag.Genders.Add("не указано");
            ViewBag.Genders.Add("мужской");
            ViewBag.Genders.Add("женский");
            ViewBag.Genders.Add("трансформер");
            var employee = _context.Employees.Find(id);
            return View(employee);
        }

        [HttpPost]
        public IActionResult DeleteEmployee(int id)
        {
            var employee = _context.Employees.Include(e => e.Jobs).FirstOrDefault(e => e.Id == id);
            foreach (var job in employee.Jobs) 
            {
                foreach (var payout in _context.Payouts.Include(p => p.Job)) 
                {
                    if (job.Id == payout.Job.Id) 
                    {
                        _context.Payouts.Remove(payout);
                    }
                }
                _context.Jobs.Remove(job);
            }
            _context.Remove(employee);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditEmployee(int id)
        {
            var employee = _context.Employees.Include(e => e.Jobs).FirstOrDefault(e => e.Id == id);
            ViewBag.JobsCount = employee.Jobs.Count();
            return View(employee);
        }

        [HttpPost]
        public IActionResult UpdateEmployee(int id, string cipher, string fullName, DateTime birthDate, byte gender)
        {
            var employee = _context.Employees.Find(id);
            employee.Cipher = cipher;
            employee.FullName = fullName;
            employee.BirthDate = birthDate;
            employee.Gender = gender;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
