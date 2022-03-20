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
    public class ReportsController : Controller
    {
        private readonly ILogger<ReportsController> _logger;
        private AppDbContext _context;

        public ReportsController(ILogger<ReportsController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FindByJob(string jobTitle) 
        {
            ViewBag.JobTitles = _context.Jobs.Select(j => j.Title).Distinct();
            ViewBag.SelectedJobTitle = "";
            if (!string.IsNullOrEmpty(jobTitle)) 
            {
                ViewBag.SelectedJobTitle = jobTitle;
            }
            ViewBag.Genders = new List<string>();
            ViewBag.Genders.Add("не указано");
            ViewBag.Genders.Add("мужской");
            ViewBag.Genders.Add("женский");
            ViewBag.Genders.Add("трансформер");
            var employees = _context.Employees.Include(e => e.Jobs).Where(e => e.Jobs.Any(j => j.Title == jobTitle));
            ViewBag.EmployeesCount = employees.Count();
            return View(employees);
        }

        [HttpGet]
        public IActionResult PromotionReport(int employeeId) 
        {
            ViewBag.Employees = _context.Employees;
            ViewBag.SelectedEmployeeId = 0;
            if (employeeId != 0) 
            {
                var employee = _context.Employees.Include(e => e.Jobs).FirstOrDefault(e => e.Id == employeeId);
                var jobs = employee.Jobs.OrderBy(j => j.EmploymentDate).ToList();

                ViewBag.SelectedEmployeeId = employee.Id;
                ViewBag.Jobs = jobs;
                ViewBag.JobsCount = jobs.Count();
            }
            return View();
        }

        [HttpGet]
        public IActionResult FindEmploymentsInPeriod(DateTime startDate, DateTime endDate)
        {
            ViewBag.Genders = new List<string>();
            ViewBag.Genders.Add("не указано");
            ViewBag.Genders.Add("мужской");
            ViewBag.Genders.Add("женский");
            ViewBag.Genders.Add("трансформер");

            ViewBag.IsDatesDefined = true;
            if (startDate == DateTime.MinValue && endDate == DateTime.MinValue)
            {
                ViewBag.IsDatesDefined = false;
            }

            endDate = (endDate == DateTime.MinValue) ? DateTime.MaxValue : endDate;

            var employees = _context
                .Employees
                .Include(e => e.Jobs)
                .Where(e => e.Jobs.Any(j => j.EmploymentDate >= startDate && j.EmploymentDate <= endDate));
            ViewBag.EmployeesCount = employees.Count();
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            return View(employees);
        }

        [HttpGet]
        public IActionResult StatementOfCharges(int month, int year, int employeeId) 
        {
            var monthes = new Dictionary<int, string>();
            monthes.Add(1, "Январь");
            monthes.Add(2, "Февраль");
            monthes.Add(3, "Март");
            monthes.Add(4, "Апрель");
            monthes.Add(5, "Май");
            monthes.Add(6, "Июнь");
            monthes.Add(7, "Июль");
            monthes.Add(8, "Август");
            monthes.Add(9, "Сентябрь");
            monthes.Add(10, "Октябрь");
            monthes.Add(11, "Ноябрь");
            monthes.Add(12, "Декабрь");
            ViewBag.Monthes = monthes;
            ViewBag.IsMonthYearEmployeeDefined = false;
            ViewBag.Employees = _context.Employees;
            ViewBag.EmployeeId = employeeId;
            ViewBag.Month = month;
            ViewBag.Year = year;
            if (month == 0 && year == 0 && employeeId == 0)
            {
                ViewBag.IsMonthYearEmployeeDefined = true;
            }
            else 
            {
                var employee = _context.Employees.Include(j => j.Jobs).FirstOrDefault(e => e.Id == employeeId);

                string formattedMonth = month.ToString();
                string formattedYear = year.ToString();

                switch (formattedMonth.Length) 
                {
                    case 1:
                        formattedMonth = 0 + formattedMonth;
                        break;
                    default:
                        break;
                }

                switch (formattedYear.Length) 
                {
                    case 1:
                        formattedYear = "000" + formattedYear;
                        break;
                    case 2:
                        formattedYear = "00" + formattedYear;
                        break;
                    case 3:
                        formattedYear = "0" + formattedYear;
                        break;
                    default:
                        break;
                }
                
                var paymentDate = DateTime.Parse("01." + formattedMonth + "." + formattedYear);
                var jobs = employee
                    .Jobs
                    .Where(j => j.EmploymentDate < paymentDate)
                    .Where(j => (!j.IsFired) || (j.FiredDate > paymentDate));

                ViewBag.JobsCount = jobs.Count();
                ViewBag.EmployeeName = employee.FullName;
                ViewBag.SumSalary = (double) jobs.Sum(j => j.Salary * j.Rate);
                ViewBag.Prize = 0;
                if (employee.BirthDate.Month == paymentDate.Month) 
                {
                    ViewBag.Prize = ViewBag.SumSalary * 0.15;
                }
                ViewBag.NDFL = (ViewBag.SumSalary + ViewBag.Prize) * 0.13;
                ViewBag.Payout = (ViewBag.SumSalary + ViewBag.Prize) - ViewBag.NDFL;
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
