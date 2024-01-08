using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Common.Models.QueryParams
{
    public class PaymentQueryParams : QueryParams
    {
        public string AspNetUserId { get; set; }
    }
}