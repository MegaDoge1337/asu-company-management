using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyManagement.Models
{
    [Table("Payouts")]
    public class Payout
    {
        [Key]
        public int Id { get; set; }
        public Job Job { get; set; }
        public DateTime Date { get; set; }
    }
}
