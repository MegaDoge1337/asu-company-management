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
    public class PayoutsController : Controller
    {
        private readonly ILogger<PayoutsController> _logger;
        private AppDbContext _context;

        public PayoutsController(ILogger<PayoutsController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var payouts = _context.Payouts.Include(p => p.Job).ThenInclude(j => j.Employee);
            ViewBag.PayoutsCount = payouts.Count();
            return View(payouts);
        }

        public IActionResult AddPayout() 
        {
            var jobs = _context.Jobs.Include(j => j.Employee);
            ViewBag.Jobs = jobs;
            return View();
        }

        public IActionResult SavePayout(int jobId, DateTime payoutDate) 
        {
            var job = _context.Jobs.Find(jobId);
            var payout = new Payout();
            payout.Job = job;
            payout.Date = payoutDate;
            _context.Payouts.Add(payout);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditPayout(int id) 
        {
            var payout = _context.Payouts.Include(p => p.Job).FirstOrDefault(p => p.Id == id);
            ViewBag.Jobs = _context.Jobs;
            return View(payout);
        }

        [HttpPost]
        public IActionResult UpdatePayout(int id, int jobId, DateTime payoutDate) 
        {
            var job = _context.Jobs.Find(jobId);
            var payout = _context.Payouts.Include(p => p.Job).FirstOrDefault(p => p.Id == id);
            payout.Job = job;
            payout.Date = payoutDate;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AcceptDelete(int id) 
        {
            var payout = _context.Payouts.Include(p => p.Job).ThenInclude(j => j.Employee).FirstOrDefault(p => p.Id == id);
            ViewBag.Jobs = _context.Jobs;
            return View(payout);
        }

        [HttpPost]
        public IActionResult DeletePayout(int id) 
        {
            var payout = _context.Payouts.Find(id);
            _context.Payouts.Remove(payout);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
