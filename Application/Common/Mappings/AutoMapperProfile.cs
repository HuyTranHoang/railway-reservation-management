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

        CreateMap<Ticket, TicketDto>()
            .ForMember(dest => dest.PassengerName,
                otp => otp.MapFrom(src => src.Passenger.FullName))
            .ForMember(dest => dest.TrainName,
                opt => opt.MapFrom(src => src.Train.Name))
            .ForMember(dest => dest.CarriageName,
                opt => opt.MapFrom(src => src.Carriage.Name))
            .ForMember(dest => dest.SeatName,
                opt => opt.MapFrom(src => src.Seat.Name))
            .ForMember(dest => dest.ScheduleName,
                opt => opt.MapFrom(src => src.Schedule.Name))
            .ForMember(dest => dest.PassengerId,
                opt => opt.MapFrom(src => src.Payment.Id));

    }
}