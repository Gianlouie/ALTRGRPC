using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALTRGRPC.Models
{
    public class Request
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public DateTime RequestTime { get; set; }
        public bool Serviced { get; set; }
    }
}
