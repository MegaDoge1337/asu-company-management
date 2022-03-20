using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyManagement.Models
{
    [Table("Employees")]
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Cipher { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public byte Gender { get; set; }
        public List<Job> Jobs { get; set; }
    }
}
