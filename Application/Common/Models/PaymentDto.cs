using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Common.Models
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public string AspNetUserName { get; set; }
        public string Status { get; set; }
    }
}