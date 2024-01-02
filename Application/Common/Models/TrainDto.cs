using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Common.Models
{
    public class TrainDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public int TrainCompanyId { get; set; }

        public string TrainCompany { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
    }
}