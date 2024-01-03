using Application.Common.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<TrainCompany, TrainCompanyDto>();

        CreateMap<Train, TrainDto>()
            .ForMember(dest => dest.TrainCompanyName,
                opt => opt.MapFrom(src => src.TrainCompany.Name));

        CreateMap<Passenger, PassengerDto>();

        CreateMap<SeatType, SeatTypeDto>();
    }
}