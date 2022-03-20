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
    public class JobsController : Controller
    {
        private readonly ILogger<JobsController> _logger;
        private AppDbContext _context;

        public JobsController(ILogger<JobsController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var jobs = _context.Jobs.Include(j => j.Employee);
            ViewBag.JobsCount = jobs.Count();
            return View(jobs);
        }

        public IActionResult AddJob() 
        {
            var employees = _context.Employees;
            ViewBag.Employees = employees;
            return View();
        }

        [HttpPost]
        public IActionResult SaveJob(string title, int salary, int rate, DateTime employmentDate, int employeeId) 
        {
            var employee = _context.Employees.Find(employeeId);
            var job = new Job();
            job.Title = title;
            job.Salary = salary;
            job.Rate = rate;
            job.Employee = employee;
            job.EmploymentDate = employmentDate;
            job.IsFired = false;
            job.FiredDate = DateTime.MinValue;
            job.FiredReason = "";
            _context.Jobs.Add(job);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditJob(int id) 
        {
            var job = _context.Jobs.Find(id);
            var employees = _context.Employees;
            ViewBag.Employees = employees;
            return View(job);
        }

        [HttpPost]
        public IActionResult UpdateJob(
            int id, 
            string title, 
            int salary,
            int rate,
            DateTime employmentDate, 
            int employeeId, 
            string isFired,
            DateTime firedDate,
            string firedReason
        ) 
        {
            var employee = _context.Employees.Find(employeeId);
            var job = _context.Jobs.Find(id);
            job.Title = title;
            job.Salary = salary;
            job.Rate = rate;
            job.EmploymentDate = employmentDate;
            job.Employee = employee;
            job.IsFired = isFired != null;
            if (!job.IsFired)
            {
                job.FiredDate = DateTime.MinValue;
                job.FiredReason = "";
            }
            else 
            {
                job.FiredDate = firedDate;
                job.FiredReason = firedReason;
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AcceptDelete(int id) 
        {
            var job = _context.Jobs.Include(j => j.Employee).FirstOrDefault(j => j.Id == id);
            return View(job);
        }

        [HttpPost]
        public IActionResult DeleteJob(int id) 
        {
            var job = _context.Jobs.Include(j => j.Payouts).FirstOrDefault(j => j.Id == id);
            foreach (var payout in job.Payouts) 
            {
                _context.Payouts.Remove(payout);
            }
            _context.Jobs.Remove(job);
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
