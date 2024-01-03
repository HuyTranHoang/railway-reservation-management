using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Common.Models.QueryParams
{
    public class CompartmentQueryParams : QueryParams
    {
        public int CarriageId { get; set; }
    }
}