namespace Application.Common.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<TrainCompany, TrainCompanyDto>();

        CreateMap<Train, TrainDto>()
            .ForMember(dest => dest.TrainCompanyName,
                opt => opt.MapFrom(src => src.TrainCompany.Name))
            .ForMember(dest => dest.TrainCompanyLogo,
                opt => opt.MapFrom(src => src.TrainCompany.Logo));

        CreateMap<Passenger, PassengerDto>();

        CreateMap<SeatType, SeatTypeDto>();

        CreateMap<Carriage, CarriageDto>()
            .ForMember(dest => dest.TrainName,
                otp => otp.MapFrom(src => src.Train.Name))
            .ForMember(dest => dest.CarriageTypeName,
                opt => opt.MapFrom(src => src.CarriageType.Name));

        CreateMap<CarriageTemplate, CarriageTemplateDto>()
            .ForMember(dest => dest.CarriageTypeName,
                opt => opt.MapFrom(src => src.CarriageType.Name));

        CreateMap<Compartment, CompartmentDto>()
            .ForMember(dest => dest.CarriageName,
                otp => otp.MapFrom(src => src.Carriage.Name))
            .ForMember(dest => dest.TrainName,
                otp => otp.MapFrom(src => src.Carriage.Train.Name));

        // CreateMap<List<Compartment>, List<CompartmentDto>>();

        CreateMap<CompartmentTemplate, CompartmentTemplateDto>();

        CreateMap<CarriageType, CarriageTypeDto>();


        CreateMap<CancellationRule, CancellationRuleDto>();
        CreateMap<TrainStation, TrainStationDto>();

        CreateMap<Schedule, ScheduleDto>()
            .ForMember(dest => dest.TrainName,
            otp => otp.MapFrom(src => src.Train.Name))
            .ForMember(dest => dest.DepartureStationName,
            otp => otp.MapFrom(src => src.DepartureStation.Name))
            .ForMember(dest => dest.ArrivalStationName,
            otp => otp.MapFrom(src => src.ArrivalStation.Name))
            .ForMember(dest => dest.DepartureStationAddress,
            otp => otp.MapFrom(src => src.DepartureStation.Address))
            .ForMember(dest => dest.ArrivalStationAddress,
            otp => otp.MapFrom(src => src.ArrivalStation.Address))
            .ForMember(dest => dest.TrainCompanyName,
            otp => otp.MapFrom(src => src.Train.TrainCompany.Name))
            .ForMember(dest => dest.TrainCompanyLogo,
            otp => otp.MapFrom(src => src.Train.TrainCompany.Logo));

        CreateMap<Schedule, BookingDto>()
            .ForMember(dest => dest.TrainName,
            otp => otp.MapFrom(src => src.Train.Name))
            .ForMember(dest => dest.DepartureStationName,
            otp => otp.MapFrom(src => src.DepartureStation.Name))
            .ForMember(dest => dest.ArrivalStationName,
            otp => otp.MapFrom(src => src.ArrivalStation.Name))
            .ForMember(dest => dest.DepartureStationAddress,
            otp => otp.MapFrom(src => src.DepartureStation.Address))
            .ForMember(dest => dest.ArrivalStationAddress,
            otp => otp.MapFrom(src => src.ArrivalStation.Address))
            .ForMember(dest => dest.TrainCompanyName,
            otp => otp.MapFrom(src => src.Train.TrainCompany.Name))
            .ForMember(dest => dest.TrainCompanyLogo,
            otp => otp.MapFrom(src => src.Train.TrainCompany.Logo));

        CreateMap<RoundTrip, RoundTripDto>()
            .ForMember(dest => dest.TrainCompanyName,
                opt => opt.MapFrom(src => src.TrainCompany.Name))
            .ForMember(dest => dest.TrainCompanyLogo,
                opt => opt.MapFrom(src => src.TrainCompany.Logo));

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

        CreateMap<Seat, SeatDto>()
            .ForMember(dest => dest.SeatTypeName,
                opt => opt.MapFrom(src => src.SeatType.Name))
            .ForMember(dest => dest.CompartmentName,
                opt => opt.MapFrom(src => src.Compartment.Name));

        CreateMap<DistanceFare, DistanceFareDto>()
            .ForMember(dest => dest.TrainCompanyName,
                opt => opt.MapFrom(src => src.TrainCompany.Name))
            .ForMember(dest => dest.TrainCompanyLogo,
                opt => opt.MapFrom(src => src.TrainCompany.Logo));

        CreateMap<Payment, PaymentDto>()
            .ForMember(dest => dest.AspNetUserFullName,
                opt => opt.MapFrom(src => src.AspNetUser.FirstName + " " + src.AspNetUser.LastName))
            .ForMember(dest => dest.AspNetUserEmail,
                opt => opt.MapFrom(src => src.AspNetUser.Email));

        CreateMap<Cancellation, CancellationDto>()
            .ForMember(dest => dest.CancellationRuleFee,
                opt => opt.MapFrom(src => src.CancellationRule.Fee));

        // CreateMap<CarriageType, CarriageTypeDetailDto>();
        // CreateMap<Train, TrainDetailDto>();
        // CreateMap<Carriage, CarriageDetailDto>();
        // CreateMap<Compartment, CompartmentDetailDto>();
        // CreateMap<Seat, SeatDtoDetail>();

    }
}