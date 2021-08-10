using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeService.Models.ViewModels
{
    public class RequestWithPos
    {
        public Request Request { get; set; }
        public List<RequestPos> RequestPos { get; set; }
        public IEnumerable<Service> ServiceList { get; set; }
        public Customer Customer { get; set; }
        public IEnumerable<Employee> Assignee { get; set; }
        public IEnumerable<Employee> Reporter { get; set; }
        public string ReuestSymbol { get; set; }
        public IEnumerable<RequestImages> Images { get; set; }
        public IEnumerable<Bike> Bikes { get; set; }
    }
}
