using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BikeService.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        public string Symbol { get; set; }
        public DateTime Date { get; set; }

        [Required]
        public int StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }
        
        [Required]
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        
        public decimal NetValue { get; set; }
        public decimal GrossValue { get; set; }
        public bool PriorityLine { get; set; }
        
        [Required]
        public int BikeId { get; set; }
        [ForeignKey("BikeId")]
        public virtual Bike Bike { get; set; }

        public string Description { get; set; }

        [Required]
        public int Reporter { get; set; }
        [ForeignKey("Reporter")]
        public virtual Employee EmployeeReporter { get; set; }

        [Required]
        public int Assignee { get; set; }
        [ForeignKey("Assignee")]
        public virtual Employee EmployeeAssignee { get; set; }

        public string ClosedBy { get; set; }
        public string PhotoUrl { get; set; }
        public string PhotoMinUrl { get; set; }
    }
}
