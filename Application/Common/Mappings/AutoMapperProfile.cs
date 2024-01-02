using Application.Common.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<TrainCompany, TrainCompanyDto>();
        CreateMap<Train, TrainDto>();
        CreateMap<Passenger, PassengerDto>();
        CreateMap<SeatType, SeatTypeDto>();
    }
}