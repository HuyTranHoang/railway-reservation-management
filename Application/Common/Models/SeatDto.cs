using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Common.Models
{
    public class SeatDto
    {
        public int Id { get; set; }
        public int SeatTypeId { get; set; }
        public string SeatTypeName { get; set; }
        public int CompartmentId { get; set; }
        public string CompartmentName { get; set; }
        public string Status { get; set; }
    }
}