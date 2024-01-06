using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Persistence
{
    public interface IScheduleRepository : IRepository<Schedule>
    {
        Task<IQueryable<Schedule>> GetQueryWithTrainAndStationAsync();

    }
}