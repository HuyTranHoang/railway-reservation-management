namespace Application.Common.Models
{
    public class TrainDetailsDto
    {
        public TrainDto Train { get; set; }
        public List<CarriageDetailDto> Carriages { get; set; }
    }
    public class CarriageDetailDto
    {
        public CarriageDto Carriage { get; set; }
        public List<CompartmentDetailDto> Compartments { get; set; }
    }

    public class CompartmentDetailDto
    {
        public CompartmentDto Compartment { get; set; }
        public List<SeatDto> Seats { get; set; }
    }
}