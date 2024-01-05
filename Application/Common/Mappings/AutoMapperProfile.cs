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

        CreateMap<Carriage, CarriageDto>()
            .ForMember(dest => dest.TrainName,
                otp => otp.MapFrom(src => src.Train.Name))
            .ForMember(dest => dest.CarriageTypeName,
                opt => opt.MapFrom(src => src.CarriageType.Name));

        CreateMap<Compartment, CompartmentDto>()
            .ForMember(dest => dest.CarriageName,
                otp => otp.MapFrom(src => src.Carriage.Name));

        CreateMap<CarriageType, CarriageTypeDto>();


        CreateMap<CancellationRule, CancellationRuleDto>();
        CreateMap<TrainStation, TrainStationDto>();

        CreateMap<RoundTrip, RoundTripDto>()
            .ForMember(dest => dest.TrainCompanyName,
                opt => opt.MapFrom(src => src.TrainCompany.Name));

    }
}