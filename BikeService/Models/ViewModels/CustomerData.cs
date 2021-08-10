using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeService.Models.ViewModels
{
    public class CustomerData
    {
        public IEnumerable<Bike> BikesList { get; set; }
        public Customer Customer { get; set; }
    }
}
