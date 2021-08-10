using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BikeService.Models
{
    public class RequestPos
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int RequestId { get; set; }
        [ForeignKey("RequestId")]
        public virtual Request Request { get; set; }  

        [Required]
        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }

        [Required]
        public string ServiceName { get; set; }
        
        public int Quantity { get; set; }
        public decimal PriceNet { get; set; }
        public decimal PriceGross { get; set; }
        public decimal NetValue { get; set; }
        public decimal GrossValue { get; set; }
    }
}
