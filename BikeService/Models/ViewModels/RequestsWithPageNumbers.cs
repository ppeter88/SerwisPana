using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeService.Models.ViewModels
{
    public class RequestsWithPageNumbers
    {
        public IList<Request> Requests { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
