namespace Application.Common.Models
{
    public class TrainDetailsDto
    {
        public TrainDto Train { get; set; }
        public List<CarriageDto> Carriages { get; set; }
        public List<CompartmentDto> Compartments { get; set; }
        public List<SeatDto> Seats { get; set; }
    }
}