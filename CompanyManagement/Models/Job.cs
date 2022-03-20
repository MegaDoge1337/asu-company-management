using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyManagement.Models
{
    [Table("Jobs")]
    public class Job
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public int Salary { get; set; }
        public int Rate { get; set; }
        public Employee Employee { get; set; }
        public List<Payout> Payouts { get; set; }
        public DateTime EmploymentDate { get; set;}
        public bool IsFired { get; set; }
        public DateTime FiredDate { get; set; }
        public string FiredReason { get; set; }
    }
}
