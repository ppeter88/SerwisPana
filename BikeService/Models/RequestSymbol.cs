using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikeService.Models
{
    public class RequestSymbol
    {
        [Key]
        public int Id { get; set; }
        public string Period { get; set; }
        public int Number { get; set; }
        public bool IsFree { get; set; }
        public string Symbol { get; set; }
    }
}
